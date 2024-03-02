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

namespace AnyPaletteShader.DataStructures;

// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading#overloadable-operators
public enum OperatorType {
	UnaryPlus,
	UnaryNegation,
	LogicalNot,
	OnesComplement,
	Increment,
	Decrement,
	True,
	False,

	Addition,
	Subtraction,
	Multiply,
	Division,
	Modulus,
	BitwiseAnd,
	BitwiseOr,
	ExclusiveOr,
	LeftShift,
	RightShift,
	UnsignedRightShift,

	Equality,
	Inequality,
	LessThan,
	GreaterThan,
	LessThanOrEqual,
	GreaterThanOrEqual,
}
