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
using AnyPaletteShader.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;

namespace AnyPaletteShader;

// TODO: Replace this with actual UI
public sealed class PaletteConfig : ModConfig {
	public override ConfigScope Mode => ConfigScope.ClientSide;
	
	// ReSharper disable UnassignedField.Global
	
	[DefaultValue(true)]
	public bool ApplyFilter;

	[DefaultListValue(typeof(Color), "255, 255, 255, 255")]
	public List<Color> Palettes = [];
	
	// ReSharper restore UnassignedField.Global
	
	public override void OnChanged() {
		var comparer = new ColorByPercentComparer(0.3f, 0.59f, 0.11f);
		
		AnyPaletteShader.ApplyPaletteShader = ApplyFilter;
		
		{
			Palettes = [.. Palettes.ToImmutableSortedSet(comparer)];

			// Do not allow 1 color in palette to prevent soft-locks.
			if (Palettes.Count == 1)
				return;

			PaletteShaderData.Instance.UsePalette(new Palette(Palettes));
		}
	}
}

file sealed class ColorByPercentComparer(float r, float g, float b) : IComparer<Color> {
	public int Compare(Color x, Color y) {
		int left = Utils.Clamp((int)MathF.Round(x.R * r + x.G * g + x.B * b), 0, byte.MaxValue);
		int right = Utils.Clamp((int)MathF.Round(y.R * r + y.G * g + y.B * b), 0, byte.MaxValue);

		return left.CompareTo(right);
	}
}
