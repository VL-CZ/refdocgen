using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Members;

namespace RefDocGen.TemplateGenerators.Tools.TypeName;

/// <summary>
/// Static class used for retrieving operator names in C# format.
/// </summary>
public static class CSharpOperatorName
{
    /// <summary>
    /// Get name of the operator in C# format.
    /// </summary>
    /// <param name="operatorData">The operator, whose name we obtain.</param>
    /// <returns>The name of the operator in C# format (e.g. 'operator +')</returns>
    public static string Of(IOperatorData operatorData)
    {
        string? opName = operatorData.IsConversionOperator
            ? CSharpTypeName.Of(operatorData.ReturnType)
            : operatorData.Kind switch
            {
                OperatorKind.UnaryPlus => "+",
                OperatorKind.UnaryMinus => "-",
                OperatorKind.LogicalNegation => "!",
                OperatorKind.BitwiseComplement => "~",
                OperatorKind.Increment => "++",
                OperatorKind.Decrement => "--",
                OperatorKind.True => "true",
                OperatorKind.False => "false",

                OperatorKind.Addition => "+",
                OperatorKind.Subtraction => "-",
                OperatorKind.Multiplication => "*",
                OperatorKind.Division => "/",
                OperatorKind.Remainder => "%",
                OperatorKind.BitwiseAnd => "&",
                OperatorKind.BitwiseOr => "|",
                OperatorKind.ExclusiveOr => "^",
                OperatorKind.LeftShift => "<<",
                OperatorKind.RightShift => ">>",
                OperatorKind.UnsignedRightShift => ">>>",

                OperatorKind.Equality => "==",
                OperatorKind.Inequality => "!=",
                OperatorKind.LessThan => "<",
                OperatorKind.GreaterThan => ">",
                OperatorKind.LessThanOrEqual => "<=",
                OperatorKind.GreaterThanOrEqual => ">=",

                _ => throw new ArgumentOutOfRangeException(nameof(operatorData), operatorData, "Unknown operator kind"),
            };

        return $"operator {opName}";
    }
}

