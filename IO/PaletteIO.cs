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
using AnyPaletteShader.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ModLoader;

namespace AnyPaletteShader.IO;

public static class PaletteIO {
	public static string PalettePath => Path.Combine(Main.SavePath, nameof(AnyPaletteShader), "Palette.png");
	public static string PreviewPath => Path.Combine(Main.SavePath, nameof(AnyPaletteShader), "Preview.png");
	
	public static bool LoadAsTexture2D(string path, [NotNullWhen(true)] out Texture2D? texture) {
		try {
			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			
			using var handle = File.OpenHandle(path, mode: FileMode.Open, access: FileAccess.Read);
			using var file = new FileStream(handle, FileAccess.Read);
			
			var tex = default(Texture2D);
			
			ThreadUtilities.RunOnMainThreadAndWait(() => {
				// ReSharper disable once AccessToDisposedClosure
				tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, file);
			});
			
			Debug.Assert(tex != null);
			
			texture = tex;
			return true;
		}
		catch (FileNotFoundException) {
		}
		
		texture = default;
		return false;
	}
	
	public static bool LoadAsPalette(string path, out Palette palette) {
		if (LoadAsTexture2D(path, out var tex)) {
			var colors = new Color[tex.Width * tex.Height];
			
			ThreadUtilities.RunOnMainThreadAndWait(() => {
				tex.GetData(colors);
				tex.Dispose();
			});
			
			palette = new Palette(colors);
			return true;
		}
		
		palette = default;
		return false;
	}
	
	public static Texture2D? LoadFromConfigAsTexture2D() {
		var config = ModContent.GetInstance<PaletteConfig>();
		
		if (config is { Palettes: { Count: > 0 } palettes }) {
			var tex = default(Texture2D);
			
			ThreadUtilities.RunOnMainThreadAndWait(() => {
				tex = new Texture2D(Main.graphics.GraphicsDevice, palettes.Count, 1);
				tex.SetData(palettes.ToArray());
			});
			
			Debug.Assert(tex != null);
			
			return tex;
		}
		
		return null;
	}
	
	public static Texture2D Save(Palette palette, string path) {
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		
		using var handle = File.OpenHandle(path, mode: FileMode.Create, access: FileAccess.Write);
		using var file = new FileStream(handle, FileAccess.Write);
		
		var tex = default(Texture2D);
		
		// This is safe.
		// `ImmutableArray<T>` is explicitly just a `T[]` under the hood.
		// `Palette` struct is always expected to be just one `ImmutableArray<Color>`.
		var paletteColors = Unsafe.As<Palette, Color[]>(ref palette);
		
		ThreadUtilities.RunOnMainThreadAndWait(() => {
			tex = new Texture2D(Main.graphics.GraphicsDevice, palette.Count, 1);
			tex.SetData(paletteColors);
			
			// ReSharper disable once AccessToDisposedClosure
			tex.SaveAsPng(file, palette.Count, 1);
		});
		
		Debug.Assert(tex != null);
		
		return tex;
	}
}
