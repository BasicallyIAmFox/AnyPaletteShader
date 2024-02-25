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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Emit;
#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
#else
using static AnyPaletteShader.Graphics.Reflection;
#endif

namespace AnyPaletteShader.Graphics;

public readonly struct SpriteBatchSnapshot {
	public SpriteSortMode SortMode { get; }
	public BlendState BlendState { get; }
	public SamplerState SamplerState { get; }
	public DepthStencilState DepthStencilState { get; }
	public RasterizerState RasterizerState { get; }
	public Effect? Effect { get; }
	public Matrix TransformMatrix { get; }

	public SpriteBatchSnapshot(SpriteBatch spriteBatch) {
		SortMode = SpriteBatch__sortMode(spriteBatch);
		BlendState = SpriteBatch__blendState(spriteBatch);
		SamplerState = SpriteBatch__samplerState(spriteBatch);
		DepthStencilState = SpriteBatch__depthStencilState(spriteBatch);
		RasterizerState = SpriteBatch__rasterizerState(spriteBatch);
		Effect = SpriteBatch__customEffect(spriteBatch);
		TransformMatrix = SpriteBatch__transformMatrix(spriteBatch);

#if NET8_0_OR_GREATER
		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "sortMode")]
		extern static ref SpriteSortMode SpriteBatch__sortMode(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "blendState")]
		extern static ref BlendState SpriteBatch__blendState(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "samplerState")]
		extern static ref SamplerState SpriteBatch__samplerState(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "depthStencilState")]
		extern static ref DepthStencilState SpriteBatch__depthStencilState(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "rasterizerState")]
		extern static ref RasterizerState SpriteBatch__rasterizerState(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "customEffect")]
		extern static ref Effect? SpriteBatch__customEffect(SpriteBatch self);

		[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "transformMatrix")]
		extern static ref Matrix SpriteBatch__transformMatrix(SpriteBatch self);
#endif
	}

	public SpriteBatchSnapshot(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect? effect, Matrix matrix) {
		SortMode = sortMode;
		BlendState = blendState;
		SamplerState = samplerState;
		DepthStencilState = depthStencilState;
		RasterizerState = rasterizerState;
		Effect = effect;
		TransformMatrix = matrix;
	}
}

#if !NET8_0_OR_GREATER
file static class Reflection {
	public delegate ref SpriteSortMode d_SpriteBatch__sortMode(SpriteBatch self);
	public static readonly d_SpriteBatch__sortMode SpriteBatch__sortMode = CreateDelegate__SpriteBatch__sortMode();
	private static d_SpriteBatch__sortMode CreateDelegate__SpriteBatch__sortMode() {
		var dm = new DynamicMethod(string.Empty, typeof(SpriteSortMode).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("sortMode", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__sortMode>();
	}

	public delegate ref BlendState d_SpriteBatch__blendState(SpriteBatch self);
	public static readonly d_SpriteBatch__blendState SpriteBatch__blendState = CreateDelegate__SpriteBatch__blendState();
	private static d_SpriteBatch__blendState CreateDelegate__SpriteBatch__blendState() {
		var dm = new DynamicMethod(string.Empty, typeof(BlendState).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("blendState", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__blendState>();
	}

	public delegate ref SamplerState d_SpriteBatch__samplerState(SpriteBatch self);
	public static readonly d_SpriteBatch__samplerState SpriteBatch__samplerState = CreateDelegate__SpriteBatch__samplerState();
	private static d_SpriteBatch__samplerState CreateDelegate__SpriteBatch__samplerState() {
		var dm = new DynamicMethod(string.Empty, typeof(SamplerState).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("samplerState", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__samplerState>();
	}

	public delegate ref DepthStencilState d_SpriteBatch__depthStencilState(SpriteBatch self);
	public static readonly d_SpriteBatch__depthStencilState SpriteBatch__depthStencilState = CreateDelegate__SpriteBatch__depthStencilState();
	private static d_SpriteBatch__depthStencilState CreateDelegate__SpriteBatch__depthStencilState() {
		var dm = new DynamicMethod(string.Empty, typeof(DepthStencilState).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("depthStencilState", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__depthStencilState>();
	}

	public delegate ref RasterizerState d_SpriteBatch__rasterizerState(SpriteBatch self);
	public static readonly d_SpriteBatch__rasterizerState SpriteBatch__rasterizerState = CreateDelegate__SpriteBatch__rasterizerState();
	private static d_SpriteBatch__rasterizerState CreateDelegate__SpriteBatch__rasterizerState() {
		var dm = new DynamicMethod(string.Empty, typeof(RasterizerState).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("rasterizerState", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__rasterizerState>();
	}

	public delegate ref Effect? d_SpriteBatch__customEffect(SpriteBatch self);
	public static readonly d_SpriteBatch__customEffect SpriteBatch__customEffect = CreateDelegate__SpriteBatch__customEffect();
	private static d_SpriteBatch__customEffect CreateDelegate__SpriteBatch__customEffect() {
		var dm = new DynamicMethod(string.Empty, typeof(Effect).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("customEffect", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__customEffect>();
	}

	public delegate ref Matrix d_SpriteBatch__transformMatrix(SpriteBatch self);
	public static readonly d_SpriteBatch__transformMatrix SpriteBatch__transformMatrix = CreateDelegate__SpriteBatch__transformMatrix();
	private static d_SpriteBatch__transformMatrix CreateDelegate__SpriteBatch__transformMatrix() {
		var dm = new DynamicMethod(string.Empty, typeof(Matrix).MakeByRefType(), [typeof(SpriteBatch)]);
		var il = dm.GetILGenerator();

		il.Emit(OpCodes.Ldarg_0);
		il.Emit(OpCodes.Ldflda, typeof(SpriteBatch).GetField("transformMatrix", ReflectionUtilities.InstancePrivate)!);
		il.Emit(OpCodes.Ret);

		return dm.CreateDelegate<d_SpriteBatch__transformMatrix>();
	}
}
#endif
