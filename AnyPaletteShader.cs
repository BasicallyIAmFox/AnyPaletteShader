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

using AnyPaletteShader.Graphics;
using AnyPaletteShader.Patches;
using AnyPaletteShader.UI;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AnyPaletteShader;

public sealed class AnyPaletteShader : Mod {
	public static AnyPaletteShader Instance { get; private set; } = null!;

	public static ILog Log => ModContent.GetInstance<AnyPaletteShader>().Logger;

	/// <summary>
	/// When <see langword="true"/>, it will apply palette shader to the screen.
	/// </summary>
	public static bool ApplyPaletteShader { get; set; } = true;

	public AnyPaletteShader() {
		Instance = this;
	}

	public override void Load() {
		if (Main.dedServ)
			return;

		UIAssets.Load();

		ModSupport.Load();

		RenderTargets.Load();

		AddPaletteConfigButtonIlPatch.Load();

		PatchDrawing();
	}

	private static void PatchDrawing() {
		Log.Info("Patching drawing");

		RenderTargetOverrider.Patch();

		On_Main.DoDraw += static (orig, self, time) => {
			// Draw everything to the screen target
			Main.spriteBatch.GraphicsDevice.SetRenderTarget(RenderTargets.ScreenTarget);
			using (RenderTargetOverrider.OverrideDefault(RenderTargets.ScreenTarget!))
				orig(self, time);
			Main.spriteBatch.GraphicsDevice.SetRenderTarget(null);

			DrawScreen();
		};

		return;

		static void DrawScreen() {
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

			if (ApplyPaletteShader)
				PaletteShader.Instance.Apply();

			Main.spriteBatch.Draw(RenderTargets.ScreenTarget, Vector2.Zero, Color.White);

			Main.spriteBatch.End();
		}
	}

	public override void Unload() {
		// No need to manually unload drawing patches done in PatchDrawing; they should be undone by tML.

		RenderTargets.Unload();

		ModSupport.Unload();

		UIAssets.Unload();

		Instance = null!;
	}
}
