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

/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace AnyPaletteShader;

public class PaletteConfig : ModConfig
{
	public static PaletteConfig Instance => ModContent.GetInstance<PaletteConfig>();
	public override ConfigScope Mode => ConfigScope.ClientSide;

	[Header("Modifications")]
	[Label("Enter palette here")]
	[DefaultListValue(typeof(Color), "255, 255, 255, 255")]
	public List<Color> Palettes = new();

	public override void OnChanged() {
		AnyPaletteShader.ColorList = Palettes.ToList();

		var encounteredItems = new HashSet<Color>();
		AnyPaletteShader.ColorList.RemoveAll(item => !encounteredItems.Add(item));

		AnyPaletteShader.ColorList.Sort((x, y) => {
			int z = Utils.Clamp((int)MathF.Round(x.R * 0.3f + x.G * 0.59f + x.B * 0.11f), 0, byte.MaxValue);
			int w = Utils.Clamp((int)MathF.Round(y.R * 0.3f + y.G * 0.59f + y.B * 0.11f), 0, byte.MaxValue);
			return w.CompareTo(z);
		});

		Main.QueueMainThreadAction(() => {
			///Filters.Scene[AnyPaletteShader.PaletteShaderKey]
			PaletteShaderData.texture = null;
			if (AnyPaletteShader.ColorList.Count > 0)
			{
				string palettePath = Path.Combine(Main.SavePath, nameof(AnyPaletteShader));
				Directory.CreateDirectory(palettePath);

				using var s = File.Open(Path.Combine(palettePath, "Palette.png"), FileMode.Create);

				var paletteTexture = new Texture2D(Main.graphics.GraphicsDevice, AnyPaletteShader.ColorList.Count, 1);
				paletteTexture.SetData(AnyPaletteShader.ColorList.ToArray());
				paletteTexture.SaveAsPng(s, AnyPaletteShader.ColorList.Count, 1);

				PaletteShaderData.texture = paletteTexture;
			}
		});
	}
}*/

/*
		if (previewPalette == null) {
			string palettePath = Path.Combine(Main.SavePath, nameof(AnyPaletteShader));
			Directory.CreateDirectory(palettePath);
			palettePath = Path.Combine(palettePath, "Palette.png");

			if (File.Exists(palettePath)) {
				Texture2D paletteTexture;
				using (var paletteFile = File.Open(palettePath, FileMode.Open)) {
					paletteTexture = Texture2D.FromStream(Main.instance.GraphicsDevice, paletteFile);
				}
				previewPalette = paletteTexture;
			}
		}
*/