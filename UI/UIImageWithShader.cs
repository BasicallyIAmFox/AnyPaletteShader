﻿//
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

using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;

namespace AnyPaletteShader.UI;

public sealed class UIImageWithShader : UIImage {
	public ShaderData ShaderData { get; }
	public bool ApplyShader { get; set; }

	public event ElementEvent? OnDraw;

	public UIImageWithShader(Asset<Texture2D> texture, ShaderData shaderData) : base(texture) {
		ShaderData = shaderData;

		ApplyShader = true;
		UseImmediateMode = true;
	}

	public UIImageWithShader(Texture2D nonReloadingTexture, ShaderData shaderData) : base(nonReloadingTexture) {
		ShaderData = shaderData;

		ApplyShader = true;
		UseImmediateMode = true;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch) {
		OnDraw?.Invoke(this);

		if (ApplyShader)
			ShaderData.Apply();

		base.DrawSelf(spriteBatch);
	}
}
