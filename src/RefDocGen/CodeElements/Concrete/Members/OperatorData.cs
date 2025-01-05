using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of an operator.
/// </summary>
internal class OperatorData : MethodData, IOperatorData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorData"/> class.
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/> object representing the operator.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    internal OperatorData(MethodInfo methodInfo, TypeDeclaration containingType, IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters)
        : base(methodInfo, containingType, availableTypeParameters)
    {
        Kind = methodNameToOperatorKind[methodInfo.Name];
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public bool IsConversionOperator => Kind is OperatorKind.ExplicitConversion or OperatorKind.ImplicitConversion;

    /// <inheritdoc/>
    public OperatorKind Kind { get; }

    /// <summary>
    /// Dictionary containing key-value pairs of the operator mehod name and the corresponding operator kind.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, OperatorKind> methodNameToOperatorKind = new Dictionary<string, OperatorKind>
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
        ["op_GreaterThanOrEqual"] = OperatorKind.GreaterThanOrEqual,

        ["op_Explicit"] = OperatorKind.ExplicitConversion,
        ["op_Implicit"] = OperatorKind.ImplicitConversion,
    };

    /// <summary>
    /// A collection of all corresponding operator method names.
    /// </summary>
    public static IEnumerable<string> MethodNames => methodNameToOperatorKind.Keys;
}
