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
using log4net;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace AnyPaletteShader.UI;

public static partial class PaletteConfig {
	private static ILog Log => AnyPaletteShader.Log;

	[MaybeNull] private static UIImage _paletteConfigButton;

	public static void Load() {
		Log.Info("Adding Palette Config Button to the UI");
		if (ModSupport.HasConciseModList)
			AddPaletteConfigButtonToUi_ConciseModList();
		else
			AddPaletteConfigButtonToUi();
	}

	private static void AddPaletteConfigButtonToUi() {
		var uiModItemType = typeof(UIModItem);

		var uiModItemOnInitialize = uiModItemType.GetMethod(nameof(UIModItem.OnInitialize), ReflectionUtilities.InstancePublic)!;
		var uiModItemDrawSelf = uiModItemType.GetMethod("DrawSelf", ReflectionUtilities.InstanceProtected)!;

		MonoModHooks.Modify(uiModItemOnInitialize, static il => {
			var c = new ILCursor(il);

			try {
				Modify(c);
			}
			catch (Exception e) {
				Log.Error($"Something went wrong while patching {nameof(UIModItem)}.{nameof(UIElement.OnInitialize)}");

				throw new ILPatchFailureException(AnyPaletteShader.Instance, il, e);
			}
		});

		MonoModHooks.Add(uiModItemDrawSelf, static (Action<UIModItem, SpriteBatch> orig, UIModItem self, SpriteBatch spriteBatch) => {
			orig(self, spriteBatch);

			if (self.ModName == AnyPaletteShader.Instance.Name && _paletteConfigButton!.IsMouseHovering)
				OnPaletteConfigButtonMouseHover(out self._tooltip);
		});

		return;

		static void Modify(ILCursor c) {
			/*
				// if (ModLoader.TryGetMod(ModName, out var result) && ConfigManager.Configs.ContainsKey(result))
				IL_0461: ldarg.0
				IL_0462: call instance string Terraria.ModLoader.UI.UIModItem::get_ModName()
				IL_0467: ldloca.s 7
				IL_0469: call bool Terraria.ModLoader.ModLoader::TryGetMod(string, class Terraria.ModLoader.Mod&)
				IL_046e: brfalse IL_0522

				IL_0473: ldsfld class [System.Runtime]System.Collections.Generic.IDictionary`2<class Terraria.ModLoader.Mod, class [System.Collections]System.Collections.Generic.List`1<class Terraria.ModLoader.Config.ModConfig>> Terraria.ModLoader.Config.ConfigManager::Configs
				IL_0478: ldloc.s 7
				IL_047a: callvirt instance bool class [System.Runtime]System.Collections.Generic.IDictionary`2<class Terraria.ModLoader.Mod, class [System.Collections]System.Collections.Generic.List`1<class Terraria.ModLoader.Config.ModConfig>>::ContainsKey(!0)
				IL_047f: brfalse IL_0522

				// bottomRightRowOffset -= 36;
				IL_0484: ldloc.s 6
				IL_0486: ldc.i4.s 36
				IL_0488: sub
				IL_0489: stloc.s 6
			 */

			int bottomRightRowOffsetIndex = -1, bottomRightRowOffsetNumber = -1;

			c.GotoNext(
				i => i.MatchCall(typeof(ModLoader), nameof(ModLoader.TryGetMod)),
				i => i.MatchBrfalse(out _)
			);

			int currentIndex = c.Index;
			{
				c.GotoNext(
					i => i.MatchCallvirt<IDictionary<Mod, List<ModConfig>>>("ContainsKey"),
					i => i.MatchBrfalse(out _)
				);

				c.GotoNext(i => i.MatchLdcI4(out bottomRightRowOffsetNumber));
				Debug.Assert(bottomRightRowOffsetNumber != -1);

				c.GotoNext(i => i.MatchStloc(out bottomRightRowOffsetIndex));
				Debug.Assert(bottomRightRowOffsetIndex != -1);
			}

			c.Index = currentIndex;

			// Emit IL for the button itself
			c.EmitLdarg0();
			c.EmitLdloca(bottomRightRowOffsetIndex);
			c.EmitLdcI4(bottomRightRowOffsetNumber);
			c.EmitDelegate(static (UIModItem uiModItem, ref int bottomRightRowOffset, int offsetNumber) => {
				if (uiModItem.ModName != AnyPaletteShader.Instance.Name)
					return;

				bottomRightRowOffset -= offsetNumber;

				// Construct palette config button
				_paletteConfigButton = new UIImage(UIAssets.ButtonPaletteConfig) {
					Width = { Pixels = 36f },
					Height = { Pixels = 36f },
					Left = { Pixels = bottomRightRowOffset - 5f, Precent = 1f },
					Top = { Pixels = 40f }
				};

				_paletteConfigButton.OnLeftClick += (_, _) => OnPaletteConfigButtonClick();

				uiModItem.Append(_paletteConfigButton);
			});
		}
	}

	private static void OnPaletteConfigButtonMouseHover(out string text) {
		text = AnyPaletteShader.Instance.GetLocalization("UI.PaletteConfigButton").Value;
	}

	private static void OnPaletteConfigButtonClick() {
		SoundEngine.PlaySound(in SoundID.MenuOpen);

		AnyPaletteShader.ApplyPaletteShader = false;

		Main.MenuUI.SetState(UIPaletteConfig.Instance);
		Main.menuMode = MenuID.FancyUI;
	}

	public static void Unload() {
	}
}
