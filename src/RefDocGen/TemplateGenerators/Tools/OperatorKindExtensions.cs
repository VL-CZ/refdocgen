using RefDocGen.CodeElements;

namespace RefDocGen.TemplateGenerators.Tools;

/// <summary>
/// Class containing extension methods for <see cref="OperatorKind"/> enum.
/// </summary>
public static class OperatorKindExtensions
{
    /// <summary>
    /// Get name of the operator kind (e.g. 'operator +')
    /// </summary>
    /// <param name="operatorKind">The kind of operator, whose name we obtain.</param>
    /// <returns>The name of the operator kind (e.g. 'operator +')</returns>
    public static string GetName(this OperatorKind operatorKind)
    {
        string opName = operatorKind switch
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

        return $"operator {opName}";
    }
}

