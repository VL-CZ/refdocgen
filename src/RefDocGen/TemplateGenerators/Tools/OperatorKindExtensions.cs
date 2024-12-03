using RefDocGen.CodeElements;

namespace RefDocGen.TemplateGenerators.Tools;

/// <summary>
/// Class containing extension methods for <see cref="OperatorKind"/> enum.
/// </summary>
public static class OperatorKindExtensions
{
    /// <summary>
    /// Get the string representation of the operator kind.
    /// </summary>
    /// <param name="operatorKind">The kind of operator, whose name we obtain.</param>
    /// <returns>The string representing the operator kind.</returns>
    public static string ToOperatorString(this OperatorKind operatorKind)
    {
        return operatorKind switch
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
            _ => throw new ArgumentOutOfRangeException(nameof(operatorKind), operatorKind, "Unknown operator kind"),
        };
    }
}

