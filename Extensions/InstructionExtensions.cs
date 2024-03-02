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

using AnyPaletteShader.DataStructures;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace AnyPaletteShader.Extensions;

public static class InstructionExtensions {
	public static bool MatchLdloc(this Instruction instruction, out LocalVariableIndex variableIndex) {
		bool value = instruction.MatchLdloc(out int index);

		variableIndex = new LocalVariableIndex((uint)index);

		return value;
	}

	public static bool MatchLdloc(this Instruction instruction, LocalVariableIndex variableIndex) {
		return instruction.MatchLdloc(variableIndex.ToInt());
	}

	public static bool MatchLdloca(this Instruction instruction, out LocalVariableIndex variableIndex) {
		bool value = instruction.MatchLdloca(out int index);

		variableIndex = new LocalVariableIndex((uint)index);

		return value;
	}

	public static bool MatchStloc(this Instruction instruction, out LocalVariableIndex variableIndex) {
		bool value = instruction.MatchStloc(out int index);

		variableIndex = new LocalVariableIndex((uint)index);

		return value;
	}

	public static bool MatchStloc(this Instruction instruction, LocalVariableIndex variableIndex) {
		return instruction.MatchStloc(variableIndex.ToInt());
	}
}

partial class ILCursorExtensions {
	public static ILCursor EmitLdloc(this ILCursor cursor, LocalVariableIndex variableIndex) {
		return cursor.EmitLdloc(variableIndex.ToInt());
	}

	public static ILCursor EmitLdloca(this ILCursor cursor, LocalVariableIndex variableIndex) {
		return cursor.EmitLdloca(variableIndex.ToInt());
	}

	public static ILCursor EmitStloc(this ILCursor cursor, LocalVariableIndex variableIndex) {
		return cursor.EmitStloc(variableIndex.ToInt());
	}
}
