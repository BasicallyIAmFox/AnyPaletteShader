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

using AnyPaletteShader.Utilities;
using log4net;
using System.Diagnostics.CodeAnalysis;
using Terraria;

namespace AnyPaletteShader.Graphics;

public static class RenderTargets {
	private static ILog Log => AnyPaletteShader.Log;

	[MaybeNull] public static ScreenRenderTarget ScreenTarget { get; private set; }

	public static void Load() {
		ThreadUtilities.RunOnMainThreadAndWait(static () => {
			ScreenTarget = new ScreenRenderTarget(Main.graphics.GraphicsDevice);

			Log.Info("Initialized render targets");
		});
	}

	public static void Unload() {
		if (ScreenTarget != null) {
			ThreadUtilities.RunOnMainThreadAndWait(static () => {
				ScreenTarget.Dispose();
				ScreenTarget = null!;
			});
		}
	}
}
