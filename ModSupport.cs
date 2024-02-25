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

using Terraria.ModLoader;

namespace AnyPaletteShader;

public static class ModSupport {
	public const string ConciseModListName = "ConciseModList";
	public static bool HasConciseModList => ConciseModList != null;
	public static Mod? ConciseModList => _conciseModList;
	private static Mod? _conciseModList;

	internal static void Load() {
		ModLoader.TryGetMod(ConciseModListName, out _conciseModList);
	}

	internal static void Unload() {
		_conciseModList = null;
	}
}
