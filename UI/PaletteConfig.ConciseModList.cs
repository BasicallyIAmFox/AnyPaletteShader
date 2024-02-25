//
//    Copyright 2023-2024 BasicallyIAmFox
//
//    Licensed under the Apache License, Version 2.0 (the "License")
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using AnyPaletteShader.Utilities;
using ConciseModList;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace AnyPaletteShader.UI;

partial class PaletteConfig {
	[JITWhenModsEnabled(ModSupport.ConciseModListName)]
	private static void AddPaletteConfigButtonToUi_ConciseModList() {
		var conciseUIModItemType = typeof(ConciseUIModItem);

		var conciseUIModItemOnInitialize = conciseUIModItemType.GetMethod(nameof(ConciseUIModItem.OnInitialize), ReflectionUtilities.InstancePublic)!;
		var conciseUIModItemLeftClickEvent = conciseUIModItemType.GetMethod(nameof(ConciseUIModItem.LeftClickEvent), ReflectionUtilities.InstancePublic)!;
		var conciseUIModItemConfigButtonHover = conciseUIModItemType.GetMethod(nameof(ConciseUIModItem.ConfigButtonHover), ReflectionUtilities.InstancePublic)!;
		var conciseUIModItemMakeModInfoLines = conciseUIModItemType.GetMethod(nameof(ConciseUIModItem.MakeModInfoLines), ReflectionUtilities.InstancePublic)!;

		MonoModHooks.Add(conciseUIModItemOnInitialize,
			[method: JITWhenModsEnabled(ModSupport.ConciseModListName)]
		static (Action<ConciseUIModItem> orig, ConciseUIModItem self) => {
			orig(self);

			ConstructPaletteConfigButton(self);
		});

		MonoModHooks.Add(conciseUIModItemLeftClickEvent,
			[method: JITWhenModsEnabled(ModSupport.ConciseModListName)]
		static (Action<ConciseUIModItem, UIMouseEvent, UIElement> orig, ConciseUIModItem self, UIMouseEvent mouseEvent, UIElement element) => {
			if (self.ModName == AnyPaletteShader.Instance.Name && _paletteConfigButton?.IsMouseHovering is true)
				return;

			orig(self, mouseEvent, element);
		});

		MonoModHooks.Add(conciseUIModItemConfigButtonHover,
			[method: JITWhenModsEnabled(ModSupport.ConciseModListName)]
		static (Func<ConciseUIModItem, bool> orig, ConciseUIModItem self) => {
			if (self.ModName == AnyPaletteShader.Instance.Name && _paletteConfigButton?.IsMouseHovering is true) {
				OnPaletteConfigButtonMouseHover(out string text);

				Main.instance.MouseText(text);
				return true;
			}

			return orig(self);
		});

		MonoModHooks.Add(conciseUIModItemMakeModInfoLines,
			[method: JITWhenModsEnabled(ModSupport.ConciseModListName)]
		static (Func<ConciseUIModItem, List<(string Name, string Description)>> orig, ConciseUIModItem self) => {
			var lines = orig(self);

			if (self.ModName != AnyPaletteShader.Instance.Name)
				return lines;

			// We technically don't need to look for 'References', but let's do it anyway.
			int i = lines.FindIndex(line => line.Name.Equals("References", StringComparison.InvariantCulture));
			if (i == -1)
				i = lines.FindIndex(line => line.Name.Equals("MoreInfo", StringComparison.InvariantCulture));

			lines.Insert(i, ("OpenConfig", Language.GetTextValue("Mods.ConciseModList.ModsOpenConfig")));

			return lines;
		});

		return;

		[JITWhenModsEnabled(ModSupport.ConciseModListName)]
		static void ConstructPaletteConfigButton(UIModItem uiModItem) {
			if (uiModItem.ModName != AnyPaletteShader.Instance.Name)
				return;

			_paletteConfigButton = new UIImage(UIAssets.ButtonPaletteConfig2) {
				Width = { Pixels = 32f },
				Height = { Pixels = 32f },
				Left = { Pixels = -32f, Precent = 1f },
				Top = { Pixels = -32f, Precent = 1f }
			};

			_paletteConfigButton.OnLeftClick += (_, _) => OnPaletteConfigButtonClick();

			// Concise Mod List allows to use right click to open config
			uiModItem.OnRightClick += (_, _) => OnPaletteConfigButtonClick();

			// Concise Mod List optionally adds button.
			if (ConciseModConfig.Instance.ConfigButton)
				uiModItem.Append(_paletteConfigButton);
		}
	}
}
