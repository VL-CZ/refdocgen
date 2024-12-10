using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Tools;

/// <summary>
/// Class providing methods for getting IDs of the selected types.
/// </summary>
internal class TypeId
{
    /// <summary>
    /// Get the ID of the given <paramref name="type"/>
    /// </summary>
    /// <param name="type">The type, whose ID is returned.</param>
    /// <returns>The ID of the given <paramref name="type"/></returns>
    internal static string Of(ITypeDeclaration type)
    {
        string name = type.FullName;

        if (type.HasTypeParameters)
        {
            name = name + '`' + type.TypeParameterDeclarations.Count;
        }

        return name;
    }
}
