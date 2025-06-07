using System.Globalization;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Class responsible for formatting literal values to its C# format.
/// </summary>
internal class CSharpLiteralValueFormatter
{
    /// <summary>
    /// Format the literal value into a string.
    /// </summary>
    /// <param name="literalValue">Literal value to format.</param>
    /// <returns>String representation of the literal value.</returns>
    internal static string Format(object? literalValue)
    {
        if (literalValue is null)
        {
            return "null";
        }
        else if (literalValue is string s)
        {
            return $"\"{s}\"";
        }
        else if (literalValue is Type type)
        {
            return $"typeof({type})";
        }
        else if (literalValue is IFormattable f)
        {
            return f.ToString(null, CultureInfo.InvariantCulture);
        }
        else
        {
            return literalValue.ToString() ?? "";
        }
    }
}
