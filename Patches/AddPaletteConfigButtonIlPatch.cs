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

using AnyPaletteShader.DataStructures;
using AnyPaletteShader.Extensions;
using AnyPaletteShader.UI;
using AnyPaletteShader.Utilities;
using log4net;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace AnyPaletteShader.Patches;

internal static partial class AddPaletteConfigButtonIlPatch {
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

			if (IsPaletteConfigModItem(self) && _paletteConfigButton?.IsMouseHovering is true)
				PaletteConfig.OnPaletteConfigButtonMouseHover(out self._tooltip);
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

				// 		_configButton = new UIImage(UICommon.ButtonModConfigTexture)
				// 		{
				// 			Width = 
				// 			{
				// 				Pixels = 36f
				// 			},
				// 			Height = 
				// 			{
				// 				Pixels = 36f
				// 			},
				// 			Left = 
				// 			{
				// 				Pixels = (float)bottomRightRowOffset - 5f,
				// 				Precent = 1f
				// 			},
				// 			Top = 
				// 			{
				// 				Pixels = 40f
				// 			}
				// 		};
				IL_048b: ldarg.0
				IL_048c: call class [ReLogic]ReLogic.Content.Asset`1<class [FNA]Microsoft.Xna.Framework.Graphics.Texture2D> Terraria.ModLoader.UI.UICommon::get_ButtonModConfigTexture()
				IL_0491: newobj instance void Terraria.GameContent.UI.Elements.UIImage::.ctor(class [ReLogic]ReLogic.Content.Asset`1<class [FNA]Microsoft.Xna.Framework.Graphics.Texture2D>)
				IL_0496: dup
				IL_0497: ldflda valuetype Terraria.UI.StyleDimension Terraria.UI.UIElement::Width
				IL_049c: ldc.r4 36
				IL_04a1: stfld float32 Terraria.UI.StyleDimension::Pixels
				IL_04a6: dup
				IL_04a7: ldflda valuetype Terraria.UI.StyleDimension Terraria.UI.UIElement::Height
				IL_04ac: ldc.r4 36
				IL_04b1: stfld float32 Terraria.UI.StyleDimension::Pixels
				IL_04b6: dup
				IL_04b7: ldflda valuetype Terraria.UI.StyleDimension Terraria.UI.UIElement::Left
				IL_04bc: ldloc.s 6
				IL_04be: conv.r4
				IL_04bf: ldc.r4 5
				IL_04c4: sub
				IL_04c5: stfld float32 Terraria.UI.StyleDimension::Pixels
				IL_04ca: dup
				IL_04cb: ldflda valuetype Terraria.UI.StyleDimension Terraria.UI.UIElement::Left
				IL_04d0: ldc.r4 1
				IL_04d5: stfld float32 Terraria.UI.StyleDimension::Precent
				IL_04da: dup
				IL_04db: ldflda valuetype Terraria.UI.StyleDimension Terraria.UI.UIElement::Top
				IL_04e0: ldc.r4 40
				IL_04e5: stfld float32 Terraria.UI.StyleDimension::Pixels
				IL_04ea: stfld class Terraria.GameContent.UI.Elements.UIImage Terraria.ModLoader.UI.UIModItem::_configButton
			 */

			var bottomRightRowOffsetIndex = default(LocalVariableIndex);
			var isOurModIndex = c.AddLocalVariable(typeof(bool));
			var temporaryElementIndex = c.AddLocalVariable(typeof(UIElement));

			var configButtonSkipLabel = default(ILLabel);
			var paletteButtonRouteLabel = c.DefineLabel();

			// Initialize `isOurMod` local variable.
			c.EmitLdarg0();
			c.EmitDelegate(IsPaletteConfigModItem);
			c.EmitStloc(isOurModIndex);

			// IL_0469 - IL_046e
			c.GotoNext(
				i => i.MatchCall(typeof(ModLoader), nameof(ModLoader.TryGetMod)),
				i => i.MatchBrfalse(out configButtonSkipLabel)
			);
			Debug.Assert(configButtonSkipLabel != null);

			// IL_0461 - IL_0462
			c.GotoPrev(
				i => i.MatchLdarg0(),
				i => i.MatchCall(typeof(UIModItem), $"get_{nameof(UIModItem.ModName)}")
			);

			// If it's our mod, we can go straight to palette button route.
			c.EmitLdloc(isOurModIndex);
			c.EmitBrtrue(paletteButtonRouteLabel);

			// IL_047a - IL_047f
			c.GotoNext(MoveType.After,
				i => i.MatchCallvirt<IDictionary<Mod, List<ModConfig>>>("ContainsKey"),
				i => i.MatchBrfalse(configButtonSkipLabel)
			);

			// Retrieve index of `bottomRightRowOffset` variable
			// IL_0484
			c.GotoNext(i => i.MatchLdloc(out bottomRightRowOffsetIndex));
			Debug.Assert(!bottomRightRowOffsetIndex.IsMissing);

			// Mark palette button route to IL_0484
			c.MarkLabel(paletteButtonRouteLabel);
			c.HijackIncomingLabels();

			// IL_0489
			c.GotoNext(i => i.MatchStloc(bottomRightRowOffsetIndex));

			// IL_04ea
			c.GotoNext(i => i.MatchStfld(typeof(UIModItem), nameof(UIModItem._configButton)));

			// Store in a temporary element.
			c.EmitStloc(temporaryElementIndex);

			// Modify 'config button' to our selfish desires if its for our mod.
			c.EmitLdarg0();
			c.EmitLdloc(isOurModIndex);
			c.EmitDelegate(static (UIImage buildingElement, UIModItem instance, bool isOurMod) => {
				if (isOurMod) {
					buildingElement.SetImage(UIAssets.ButtonPaletteConfig);

					buildingElement.OnLeftClick += (_, _) => PaletteConfig.OnPaletteConfigButtonClick();

					instance.Append(buildingElement);

					_paletteConfigButton = buildingElement;
					return true;
				}

				return false;
			});
			c.EmitBrtrue(configButtonSkipLabel);

			// Oh damn, it's not for our mod.
			// Load temporary element for config button.
			c.EmitLdloc(temporaryElementIndex);
		}

		/*
		static void Modify(ILCursor c) {
			var bottomRightRowOffsetIndex = default(LocalVariableIndex);

			// IL_0469 - IL_046e
			c.GotoNext(
				i => i.MatchCall(typeof(ModLoader), nameof(ModLoader.TryGetMod)),
				i => i.MatchBrfalse(out _)
			);

			// Store current index, so later we can go back.
			int currentIndex = c.Index;

			// Go further as we have two primary goals:
			// 1. Obtain bottomRightRowOffset local variable index.
			// 2. Obtain copy of instructions that subtract a value from
			// bottomRightRowOffset. If no modifications are done by
			// other mods, we expect it to subtract `36` from the value.
			Instruction[] subtractionOffsetInstructions;
			{
				// IL_047a - IL_047f
				c.GotoNext(
					i => i.MatchCallvirt<IDictionary<Mod, List<ModConfig>>>("ContainsKey"),
					i => i.MatchBrfalse(out _)
				);

				int startOfSubtractingOffsetIIndex, endOfSubtractingOffsetIIndex;
				{
					c.GotoNext(MoveType.After, i => i.MatchLdloc(out bottomRightRowOffsetIndex));

					Debug.Assert(!bottomRightRowOffsetIndex.IsMissing);

					startOfSubtractingOffsetIIndex = c.Index;

					c.GotoNext(MoveType.After, i => i.MatchStloc(bottomRightRowOffsetIndex));

					endOfSubtractingOffsetIIndex = c.Index;
				}

				// Copy instructions from IL_0484 to IL_0489 (both inclusive) to the array, so later we can re-emit them.
				//
				// Could use `CopyTo` method, however, that method seems to be bugged
				// for this kind of usage as it copies full-length to destination array.
				//
				// A better way probably would be adding a local variable in IL and jumping to
				// those instructions instead, afterwards jumping back.
				subtractionOffsetInstructions = c.Instrs.ToArray()[startOfSubtractingOffsetIIndex..endOfSubtractingOffsetIIndex];
			}

			// Go back right before IL_0469
			c.Index = currentIndex;

			c.EmitIfStatement(c => {
				// this
				c.EmitLdarg0();

				// IsPaletteConfigModItem(...)
				c.EmitDelegate(IsPaletteConfigModItem);
			}, c => {
				// Emit instructions for subtracting from `bottomRightRowOffset`
				c.EmitRange(clone: false, subtractionOffsetInstructions);
				
				// Emit IL for constructing the palette config button
				c.EmitLdarg0();
				c.EmitLdloc(bottomRightRowOffsetIndex);
				c.EmitDelegate(ConstructPaletteConfigButton);
			});
		}
		*/

		/*static void ConstructPaletteConfigButton(UIModItem uiModItem, int bottomRightRowOffset) {
			_paletteConfigButton = new UIImage(UIAssets.ButtonPaletteConfig) {
				Width = { Pixels = 36f },
				Height = { Pixels = 36f },
				Left = { Pixels = bottomRightRowOffset - 5f, Precent = 1f },
				Top = { Pixels = 40f }
			};

			_paletteConfigButton.OnLeftClick += (_, _) => PaletteConfig.OnPaletteConfigButtonClick();

			uiModItem.Append(_paletteConfigButton);
		}*/
	}

	public static bool IsPaletteConfigModItem(UIModItem modItem) {
		return modItem.ModName == AnyPaletteShader.Instance.Name;
	}
}
