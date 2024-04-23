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
using System.Runtime.CompilerServices;
using Terraria;

namespace AnyPaletteShader.Graphics;

public sealed class ScreenRenderTarget : RenderTarget2D {
	private bool MipMap { get; set; }
	
	public ScreenRenderTarget(GraphicsDevice graphicsDevice)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight) {
		// Mip mapping by default is `false`
		Init(mipMap: false);
	}
	
	/*
	public ScreenRenderTarget(GraphicsDevice graphicsDevice, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight, mipMap, preferredFormat, preferredDepthFormat) {
		Init(mipMap);
	}

	public ScreenRenderTarget(GraphicsDevice graphicsDevice, bool mipMap, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
		: base(graphicsDevice, Main.screenWidth, Main.screenHeight, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage) {
		Init(mipMap);
	}
	*/
	
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
			// Copied and pasted from Texture.Dispose
			
			TextureCollection__RemoveDisposedTexture(GraphicsDevice.Textures, this);
			TextureCollection__RemoveDisposedTexture(GraphicsDevice.VertexTextures, this);
			FNA3D.FNA3D_AddDisposeTexture(GraphicsDevice__GLDevice(GraphicsDevice), Texture__texture(this));
		}
		
		void InitializeTexture(int width, int height) {
			// Copied and pasted from Texture2D constructor
			
			Texture2D__set_Width(this, width);
			Texture2D__set_Height(this, height);
			LevelCount = MipMap ? Texture__CalculateMipLevels(this, width, height) : 1;
			
			Texture__texture(this) = FNA3D.FNA3D_CreateTexture2D(GraphicsDevice__GLDevice(GraphicsDevice), Format, Width, Height, LevelCount, 1);
		}
		
		// ReSharper disable InconsistentNaming
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "GLDevice")]
		static extern ref nint GraphicsDevice__GLDevice(GraphicsDevice self);
		
		[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "CalculateMipLevels")]
		static extern int Texture__CalculateMipLevels(Texture? self, int width, int height = 0, int depth = 0);
		
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "texture")]
		static extern ref nint Texture__texture(Texture self);

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Width")]
		static extern void Texture2D__set_Width(Texture2D self, int value);

		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "set_Height")]
		static extern void Texture2D__set_Height(Texture2D self, int value);
		
		[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "RemoveDisposedTexture")]
		static extern void TextureCollection__RemoveDisposedTexture(TextureCollection self, Texture tex);
		// ReSharper restore InconsistentNaming
	}
	
	protected override void Dispose(bool disposing) {
		if (!IsDisposed)
			Main.graphics.GraphicsDevice.DeviceReset -= OnDeviceReset;
		
		// `IsDisposed` is set in base call.
		base.Dispose(disposing);
	}
}
