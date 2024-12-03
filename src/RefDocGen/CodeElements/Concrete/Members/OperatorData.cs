using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class OperatorData : MethodData, IOperatorData
{
    internal OperatorData(MethodInfo methodInfo, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
        : base(methodInfo, declaredTypeParameters)
    {
        Kind = nameToOperatorKind[methodInfo.Name];
    }

    /// <inheritdoc/>
    public OperatorKind Kind { get; }

    /// <summary>
    /// 
    /// </summary>
    private static readonly IReadOnlyDictionary<string, OperatorKind> nameToOperatorKind = new Dictionary<string, OperatorKind>
    {
        ["op_UnaryPlus"] = OperatorKind.UnaryPlus,
        ["op_UnaryNegation"] = OperatorKind.UnaryMinus,
        ["op_LogicalNot"] = OperatorKind.LogicalNegation,
        ["op_OnesComplement"] = OperatorKind.BitwiseComplement,
        ["op_Increment"] = OperatorKind.Increment,
        ["op_Decrement"] = OperatorKind.Decrement,
        ["op_True"] = OperatorKind.True,
        ["op_False"] = OperatorKind.False,

        ["op_Addition"] = OperatorKind.Addition,
        ["op_Subtraction"] = OperatorKind.Subtraction,
        ["op_Multiply"] = OperatorKind.Multiplication,
        ["op_Division"] = OperatorKind.Division,
        ["op_Modulus"] = OperatorKind.Remainder,
        ["op_BitwiseAnd"] = OperatorKind.BitwiseAnd,
        ["op_BitwiseOr"] = OperatorKind.BitwiseOr,
        ["op_ExclusiveOr"] = OperatorKind.ExclusiveOr,
        ["op_LeftShift"] = OperatorKind.LeftShift,
        ["op_RightShift"] = OperatorKind.RightShift,
        ["op_UnsignedRightShift"] = OperatorKind.UnsignedRightShift,

        ["op_Equality"] = OperatorKind.Equality,
        ["op_Inequality"] = OperatorKind.Inequality,
        ["op_LessThan"] = OperatorKind.LessThan,
        ["op_GreaterThan"] = OperatorKind.GreaterThan,
        ["op_LessThanOrEqual"] = OperatorKind.LessThanOrEqual,
        ["op_GreaterThanOrEqual"] = OperatorKind.GreaterThanOrEqual
    };

    /// <summary>
    /// A collection of allowed operator names.
    /// </summary>
    public static IEnumerable<string> AllowedOperatorNames => nameToOperatorKind.Keys;
}
