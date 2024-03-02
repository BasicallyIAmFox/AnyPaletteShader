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
using System;
using System.Reflection;

namespace AnyPaletteShader.Extensions;

public static class ReflectionExtensions {
	public static MethodInfo? GetOperator(this Type type, OperatorType operatorType) {
		return type.GetMethod($"op_{operatorType}");
	}

	public static MethodInfo? GetOperator(this Type type, OperatorType operatorType, BindingFlags bindingAttr) {
		return type.GetMethod($"op_{operatorType}", bindingAttr);
	}

	public static MethodInfo? GetOperator(this Type type, OperatorType operatorType, BindingFlags bindingAttr, Type[] types) {
		return type.GetMethod($"op_{operatorType}", bindingAttr, types);
	}
}
