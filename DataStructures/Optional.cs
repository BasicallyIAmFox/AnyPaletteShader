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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AnyPaletteShader.DataStructures;

public readonly struct Optional<T> {
	// Fields

	private readonly T value;
	private readonly bool hasValue;

	// Properties

	public T Value {
		get {
			if (!hasValue)
				Throw();

			return value;

			[DoesNotReturn]
			[MethodImpl(MethodImplOptions.NoInlining)]
			static void Throw() {
				throw new InvalidOperationException();
			}
		}
	}

	public bool HasValue => hasValue;

	// Constructors

	public Optional() {
		// This sets both `value` and `hasValue` to their default value,
		// thus `hasValue` will always be `false`.
		this = default;
	}

	public Optional(T value) {
		this.value = value;
		hasValue = true;
	}

	// Methods

	public T? GetValueOrDefault() {
		return hasValue ? value : default;
	}

	public T? GetValueOrDefault(T defaultValue) {
		return hasValue ? value : defaultValue;
	}

	// Conversions

	public static implicit operator Optional<T>(T value) {
		return new Optional<T>(value);
	}

	public static explicit operator T(Optional<T> value) {
		return value.Value;
	}
}
