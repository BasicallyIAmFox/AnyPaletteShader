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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Emit;
using Terraria;
#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#else
using static AnyPaletteShader.Graphics.Reflection;
#endif

namespace AnyPaletteShader.Graphics;

public sealed class ScreenRenderTarget : RenderTarget2D {
	private bool MipMap { get; set; }

	public ScreenRenderTarget(GraphicsDevice graphicsDevice)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight) {
		Init(false);
	}

	public ScreenRenderTarget(GraphicsDevice graphicsDevice, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight, mipMap, preferredFormat, preferredDepthFormat) {
		Init(mipMap);
	}

	public ScreenRenderTarget(GraphicsDevice graphicsDevice, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage) {
		Init(mipMap);
	}

	private void Init(bool mipMap) {
		MipMap = mipMap;

		Main.graphics.GraphicsDevice.DeviceReset += OnDeviceReset;
	}

	private void OnDeviceReset(object? sender, EventArgs eventArgs) {
		var senderGd = (GraphicsDevice)sender!;

		var parameters = senderGd.PresentationParameters;

		DisposeTexture();
		InitializeTexture(parameters.BackBufferWidth, parameters.BackBufferHeight);

		return;

		void DisposeTexture() {
			// Copy pasted from Texture.Dispose

			TextureCollection__RemoveDisposedTexture(GraphicsDevice.Textures, this);
			TextureCollection__RemoveDisposedTexture(GraphicsDevice.VertexTextures, this);
			FNA3D.FNA3D_AddDisposeTexture(GraphicsDevice__GLDevice(GraphicsDevice), Texture__texture(this));
		}

		void InitializeTexture(int width, int height) {
			// Copy pasted from Texture2D constructor

			Texture2D__set_Width(this, width);
			Texture2D__set_Height(this, height);
			LevelCount = MipMap ? Texture__CalculateMipLevels(width, height, 0) : 1;

			Texture__texture(this) = FNA3D.FNA3D_CreateTexture2D(GraphicsDevice__GLDevice(GraphicsDevice), Format, Width, Height, LevelCount, 1);
		}

#if NET8_0_OR_GREATER
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "GLDevice")]
		extern static ref IntPtr GraphicsDevice__GLDevice(GraphicsDevice self);
		
		[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "CalculateMipLevels")]
		extern static int Texture__CalculateMipLevels(Texture? self, int width, int height = 0, int depth = 0);
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "texture")]
		extern static ref IntPtr Texture__texture(Texture self);

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Width")]
		extern static void Texture__set_Width(Texture2D self, int value);

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Height")]
		extern static void Texture__set_Height(Texture2D self, int value);
		
		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "RemoveDisposedTexture")]
		extern static void TextureCollection__RemoveDisposedTexture(TextureCollection self, Texture tex);
#endif
	}

	protected override void Dispose(bool disposing) {
		if (!IsDisposed)
			Main.graphics.GraphicsDevice.DeviceReset -= OnDeviceReset;

		// `IsDisposed` is set in base call.
		base.Dispose(disposing);
	}
}

#if !NET8_0_OR_GREATER
file static class Reflection {
	public delegate ref IntPtr d_GraphicsDevice__GLDevice(GraphicsDevice self);
	public static readonly d_GraphicsDevice__GLDevice GraphicsDevice__GLDevice = CreateDelegate__GraphicsDevice_GLDevice();
	private static d_GraphicsDevice__GLDevice CreateDelegate__GraphicsDevice_GLDevice() {
		var dm = new DynamicMethod(string.Empty, typeof(IntPtr).MakeByRefType(), [typeof(GraphicsDevice)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(GraphicsDevice).GetField("GLDevice", ReflectionUtilities.InstanceInternal)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_GraphicsDevice__GLDevice>();
	}

	public delegate int d_Texture__CalculateMipLevels(int width, int height, int depth);
	public static readonly d_Texture__CalculateMipLevels Texture__CalculateMipLevels = typeof(Texture)
		.GetMethod("CalculateMipLevels", ReflectionUtilities.StaticInternal, [typeof(int), typeof(int), typeof(int)])!
		.CreateDelegate<d_Texture__CalculateMipLevels>();

	public delegate ref IntPtr d_Texture__texture(Texture self);
	public static readonly d_Texture__texture Texture__texture = CreateDelegate__Texture_texture();
	private static d_Texture__texture CreateDelegate__Texture_texture() {
		var dm = new DynamicMethod(string.Empty, typeof(IntPtr).MakeByRefType(), [typeof(Texture)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(Texture).GetField("texture", ReflectionUtilities.InstanceInternal)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_Texture__texture>();
	}

	public delegate void d_Texture2D__set_Width(Texture2D self, int value);
	public static readonly d_Texture2D__set_Width Texture2D__set_Width = typeof(Texture2D)
		.GetMethod("set_Width", ReflectionUtilities.InstancePrivate, [typeof(int)])!
		.CreateDelegate<d_Texture2D__set_Width>();

	public delegate void d_Texture2D__set_Height(Texture2D self, int value);
	public static readonly d_Texture2D__set_Height Texture2D__set_Height = typeof(Texture2D)
		.GetMethod("set_Height", ReflectionUtilities.InstancePrivate, [typeof(int)])!
		.CreateDelegate<d_Texture2D__set_Height>();

	public delegate void d_TextureCollection__RemoveDisposedTexture(TextureCollection self, Texture tex);
	public static readonly d_TextureCollection__RemoveDisposedTexture TextureCollection__RemoveDisposedTexture = typeof(TextureCollection)
		.GetMethod("RemoveDisposedTexture", ReflectionUtilities.InstanceInternal, [typeof(Texture)])!
		.CreateDelegate<d_TextureCollection__RemoveDisposedTexture>();
}
#endif
