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

namespace AnyPaletteShader;

public readonly struct Optional<T>(T value) {
	private readonly T value = value;
	private readonly bool hasValue = true;

	public bool HasValue => hasValue;

	public T Value {
		get {
			if (!hasValue)
				throw new ArgumentException();

			return value;
		}
	}

	[return: MaybeNull]
	public T GetValueOrDefault() {
		return hasValue ? value : default;
	}

	[return: MaybeNull]
	public T GetValueOrDefault([MaybeNull] T defaultValue) {
		return hasValue ? value : defaultValue;
	}

	public static implicit operator Optional<T>(T value) {
		return new Optional<T>(value);
	}
}
