using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Class containing extension methods for <see cref="CustomAttributeData"/> class.
/// </summary>
internal static class CustomAttributeDataExtensions
{
    /// <summary>
    /// Checks whether the attribute is generated by a compiler.
    /// </summary>
    /// <param name="attribute">The attribute to check.</param>
    /// <returns><c>true</c> if the attribute is generated by the compiler; otherwise, <c>false</c>.</returns>
    internal static bool IsCompilerGenerated(this CustomAttributeData attribute)
    {
        return attribute.AttributeType.Namespace == "System.Runtime.CompilerServices";
    }
}
