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
using System;

namespace AnyPaletteShader.Extensions;

public static partial class ILCursorExtensions {
	public static LocalVariableIndex AddLocalVariable(this ILCursor cursor, Type type) {
		int index = cursor.Body.Variables.Count - 1;
		var variableType = cursor.Context.Import(type);

		cursor.Body.Variables.Add(new VariableDefinition(variableType));

		return new LocalVariableIndex(index);
	}

	public static void HijackIncomingLabels(this ILCursor cursor) {
		cursor.EmitNop();

		foreach (var label in cursor.IncomingLabels) {
			cursor.MarkLabel(label);
		}
	}

	/// <summary>
	/// Emits an if statement. <br/> Emitted C# code equivalement is:
	/// <code>
	/// if (condition()) {
	///		block();
	///	}
	/// </code>
	/// </summary>
	/// <param name="cursor"></param>
	/// <param name="condition"></param>
	/// <param name="block"></param>
	public static void EmitIfStatement(
			this ILCursor cursor,
			// TODO: Should those be just `Action`?
			// Reason: 'cursor' already exists as
			// variable or parameter in caller code.
			Action<ILCursor> condition,
			Action<ILCursor> block
		) {
		var skip = cursor.DefineLabel();

		condition(cursor);

		cursor.EmitBrfalse(skip);

		block(cursor);

		cursor.MarkLabel(skip);
	}
}
