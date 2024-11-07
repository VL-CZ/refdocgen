namespace RefDocGen.TemplateGenerators.Default.Tools.TypeName;

/// <summary>
/// Class containing extension methods for the <see cref="Type"/> class.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    /// Recursively retrieves the base element type of the specified <see cref="Type"/> instance.
    /// <para>
    /// Example: calling this method on <c>int[][]</c> returns <c>int</c>.
    /// </para>
    /// For additional info regarding element type, see <see href="https://learn.microsoft.com/en-us/dotnet/api/system.type.getelementtype?view=net-8.0"/>
    /// </summary>
    /// <param name="type">The <see cref="Type"/> instance to retrieve the base element type from.</param>
    /// <returns>The base element <see cref="Type"/> of the provided type; or the type itself if it has no element type.</returns>

    internal static Type GetBaseElementType(this Type type)
    {
        var elementType = type.GetElementType();

        return elementType is null ? type : elementType.GetBaseElementType();
    }
}
