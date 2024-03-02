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
using AnyPaletteShader.IO;
using AnyPaletteShader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;

namespace AnyPaletteShader.Graphics;

public sealed class PaletteShader : ShaderData {
	public static PaletteShader Instance { get; } = new(
		shader: new Ref<Effect>(AnyPaletteShader.Instance.Assets.Request<Effect>("Effects/PaletteShader", AssetRequestMode.ImmediateLoad).Value),
		passName: "FilterMyShader"
	);

	private Texture2D? palTex;

	private PaletteShader(Ref<Effect> shader, string passName) : base(shader, passName) {
		palTex = PaletteIO.LoadAsTexture2D(PaletteIO.PalettePath);
	}

	public PaletteShader UsePalette(Palette palette) {
		ThreadUtilities.RunOnMainThreadAndWait(() => {
			palTex?.Dispose();

			palTex = PaletteIO.SaveAndLoad(palette, PaletteIO.PalettePath);
		});

		return this;
	}

	public override void Apply() {
		if (palTex != null) {
			Main.graphics.GraphicsDevice.Textures[1] = palTex;
			Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
			Shader.Parameters["palWid"].SetValue(palTex.Width);
		}
		else {
			Shader.Parameters["palWid"].SetValue(0);
		}

		base.Apply();
	}
}
