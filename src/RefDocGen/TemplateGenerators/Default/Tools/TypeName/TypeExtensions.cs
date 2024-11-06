using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools.TypeName;

/// <summary>
/// Class containing extension methods for <see cref="TypeName"/>
/// </summary>
internal static class TypeExtensions
{
    private static Type GetBaseElementType(this Type type)
    {
        var elementType = type.GetElementType();

        return elementType is null ? type : elementType.GetBaseElementType();
    }

    internal static Type GetBaseElementType(this ITypeNameData type)
    {
        return type.TypeObject.GetBaseElementType();
    }
}
