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

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace AnyPaletteShader.DataStructures;

/// <summary>Represents array of colors.</summary>
/// <seealso cref="Color"/>
public readonly struct Palette {
	// Do not add more fields.
	private readonly ImmutableArray<Color> _colors;
	
	/// <summary>Gets the number of colors in the palette.</summary>
	public int Count => _colors.Length;
	
	public Palette(IEnumerable<Color> colors) : this([.. colors]) {
	}
	
	public Palette(ImmutableArray<Color> colors) {
		_colors = colors;
	}
}
