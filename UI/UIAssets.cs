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
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Runtime.CompilerServices;

namespace AnyPaletteShader.UI;

public static class UIAssets {
	private static ILog Log => AnyPaletteShader.Log;

	public static Asset<Texture2D> ButtonAdd = null!;
	public static Asset<Texture2D> ButtonRemove = null!;
	public static Asset<Texture2D> ButtonTriangleDown = null!;
	public static Asset<Texture2D> ButtonTriangleUp = null!;
	public static Asset<Texture2D> ButtonPaletteConfig = null!;
	public static Asset<Texture2D> ButtonPaletteConfig2 = null!;

	public static Asset<Texture2D> PalettePreview = null!;

	internal static void Load() {
		Log.Info("Loading UI Assets");

		RequestTexture2DImmediateUI(out ButtonAdd);
		RequestTexture2DImmediateUI(out ButtonRemove);
		RequestTexture2DImmediateUI(out ButtonTriangleDown);
		RequestTexture2DImmediateUI(out ButtonTriangleUp);
		RequestTexture2DImmediateUI(out ButtonPaletteConfig);
		RequestTexture2DImmediateUI(out ButtonPaletteConfig2);

		RequestTexture2DImmediateUI(out PalettePreview);

		return;

		static void RequestTexture2DImmediateUI(out Asset<Texture2D> value, [CallerArgumentExpression(nameof(value))] string path = "") {
			value = AnyPaletteShader.Instance.Assets.Request<Texture2D>($"Assets/UI/{path}", AssetRequestMode.ImmediateLoad);
		}
	}

	internal static void Unload() {
		DisposeAndNullify(ref ButtonAdd!);
		DisposeAndNullify(ref ButtonRemove!);
		DisposeAndNullify(ref ButtonTriangleDown!);
		DisposeAndNullify(ref ButtonTriangleUp!);
		DisposeAndNullify(ref ButtonPaletteConfig!);
		DisposeAndNullify(ref ButtonPaletteConfig2!);

		DisposeAndNullify(ref PalettePreview!);

		return;

		static void DisposeAndNullify<T>(ref T? value) where T : class, IDisposable {
			if (value is not null)
				ThreadUtilities.RunOnMainThreadAndWait(value.Dispose);

			value = null;
		}
	}
}
