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

using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Terraria.ModLoader;

namespace AnyPaletteShader.Graphics;

public sealed class RenderTargetOverrider {
	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly struct OverrideDefaultRenderTarget : IDisposable {
		public OverrideDefaultRenderTarget(RenderTarget2D? value) {
			_overrideDefaultValue = value;
		}

		public void Dispose() {
			_overrideDefaultValue = null;
		}
	}

	private static RenderTarget2D? _overrideDefaultValue;

	public static OverrideDefaultRenderTarget OverrideDefault(RenderTarget2D? value) {
		return new OverrideDefaultRenderTarget(value);
	}

	internal static void Patch() {
		var setRenderTargetsMethodInfo = typeof(GraphicsDevice).GetRuntimeMethod(nameof(GraphicsDevice.SetRenderTarget), [ typeof(RenderTarget2D) ]);

		Debug.Assert(setRenderTargetsMethodInfo != null);

		MonoModHooks.Add(setRenderTargetsMethodInfo, (Action<GraphicsDevice, RenderTarget2D?> orig, GraphicsDevice self, RenderTarget2D? renderTarget) => {
			// renderTarget is null whenever it tries to set to 'draw to screen' thing.
			renderTarget ??= _overrideDefaultValue;

			orig(self, renderTarget);
		});
	}
}
