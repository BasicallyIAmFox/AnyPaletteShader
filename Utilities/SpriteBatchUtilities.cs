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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;

namespace AnyPaletteShader.Utilities;

public static class SpriteBatchUtilities {
	public static void Begin(this SpriteBatch spriteBatch, SpriteBatchSnapshot snapshot) {
		spriteBatch.Begin(snapshot.SortMode, snapshot.BlendState, snapshot.SamplerState, snapshot.DepthStencilState, snapshot.RasterizerState, snapshot.Effect, snapshot.TransformMatrix);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public readonly struct Temporary_UseBegin : IDisposable {
		private readonly SpriteBatch spriteBatch;
		private readonly SpriteBatchSnapshot _old;

		public Temporary_UseBegin(
				SpriteBatch spriteBatch,
				Optional<SpriteSortMode> sortMode,
				Optional<BlendState> blendState,
				Optional<SamplerState> samplerState,
				Optional<DepthStencilState> depthStencilState,
				Optional<RasterizerState> rasterizerState,
				Optional<Effect?> effect,
				Optional<Matrix> matrix
			) {
			this.spriteBatch = spriteBatch;
			_old = new SpriteBatchSnapshot(spriteBatch);

			spriteBatch.End();
			spriteBatch.Begin(
				sortMode.GetValueOrDefault(_old.SortMode),
				blendState.GetValueOrDefault(_old.BlendState),
				samplerState.GetValueOrDefault(_old.SamplerState),
				depthStencilState.GetValueOrDefault(_old.DepthStencilState),
				rasterizerState.GetValueOrDefault(_old.RasterizerState),
				effect.GetValueOrDefault(_old.Effect),
				matrix.GetValueOrDefault(_old.TransformMatrix)
			);
		}

		public void Dispose() {
			spriteBatch.End();
			spriteBatch.Begin(_old);
		}
	}

	public static Temporary_UseBegin UseBegin(
		this SpriteBatch spriteBatch,
			Optional<SpriteSortMode> sortMode = default,
			Optional<BlendState> blendState = default,
			Optional<SamplerState> samplerState = default,
			Optional<DepthStencilState> depthStencilState = default,
			Optional<RasterizerState> rasterizerState = default,
			Optional<Effect?> effect = default,
			Optional<Matrix> matrix = default
		) {
		return new Temporary_UseBegin(spriteBatch, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
	}
}
