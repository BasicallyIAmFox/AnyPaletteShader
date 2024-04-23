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
using Terraria;
using Terraria.Graphics.Shaders;

namespace AnyPaletteShader.Graphics;

public sealed class PaletteShaderData : ShaderData {
	public static PaletteShaderData Instance { get; private set; } = new PaletteShaderData();

	private const string PassName = "FilterMyShader";
	private const string PaletteWidthShaderParameter = "palWid";

	private Texture2D? _palTex;

	private PaletteShaderData() : base(AnyPaletteShader.Instance.Assets.Request<Effect>("Effects/PaletteShader"), PassName) {
		_palTex = PaletteIO.LoadFromConfigAsTexture2D();
	}

	public void UsePalette(Palette palette) {
		ThreadUtilities.RunOnMainThreadAndWait(() => {
			_palTex?.Dispose();
			
			_palTex = PaletteIO.Save(palette, PaletteIO.PalettePath);
		});
	}

	public override void Apply() {
		if (_palTex != null) {
			Main.graphics.GraphicsDevice.Textures[1] = _palTex;
			Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;

			Shader.Parameters[PaletteWidthShaderParameter].SetValue(_palTex.Width);
		}
		else {
			Shader.Parameters[PaletteWidthShaderParameter].SetValue(0);
		}

		base.Apply();
	}
}
