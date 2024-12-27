using System.Globalization;

namespace RefDocGen.TemplateGenerators.Tools;

#pragma warning disable IDE0046

/// <summary>
/// Class responsible for formatting literal values.
/// </summary>
internal class LiteralValueFormatter
{
    /// <summary>
    /// Format the literal value into a string.
    /// </summary>
    /// <param name="literalValue">Literal value to format.</param>
    /// <returns>String representation of the literal value.</returns>
    internal static string? Format(object? literalValue)
    {
        if (literalValue is null)
        {
            return "null";
        }
        else if (literalValue is string s)
        {
            return $"\"{s}\"";
        }
        else if (literalValue is IFormattable f)
        {
            return f.ToString(null, CultureInfo.InvariantCulture);
        }
        else
        {
            return literalValue.ToString();
        }
    }
}

#pragma warning restore IDE0046