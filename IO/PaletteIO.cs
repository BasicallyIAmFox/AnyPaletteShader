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
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;

namespace AnyPaletteShader.IO;

public static class PaletteIO {
	private static string PalettePath => Path.Combine(Main.SavePath, nameof(AnyPaletteShader), "Palette.png");

	public static Texture2D? LoadAsTexture2D() {
		try {
			using var handle = File.OpenHandle(
				path: PalettePath,
				mode: FileMode.Open,
				access: FileAccess.Read
			);

			using var file = new FileStream(handle, FileAccess.Read);

			return Texture2D.FromStream(Main.graphics.GraphicsDevice, file);
		}
		catch (FileNotFoundException) {
			return null;
		}
	}

	public static Texture2D SaveAndLoad(Palette palette) {
		using var handle = File.OpenHandle(
			path: PalettePath,
			mode: FileMode.Create,
			access: FileAccess.Write
		);

		using var file = new FileStream(handle, FileAccess.Write);

		var tex = new Texture2D(Main.graphics.GraphicsDevice, palette.Count, 1);
		tex.SetData(palette.AsArray());
		tex.SaveAsPng(file, palette.Count, 1);

		return tex;
	}
}
