using System.Reflection;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Contains additoinal tools related to types.
/// </summary>
internal class TypeTools
{
    /// <summary>
    /// Gets a <see cref="ITypeNameData"/> instance based on the provided <paramref name="typeId"/>, or <c>null</c> if not found.
    /// </summary>
    /// <param name="typeId">The ID of the type to be found.</param>
    /// <param name="typeRegistry">A registry of declared types</param>
    /// <returns>A <see cref="ITypeNameData"/> instance based on the provided <paramref name="typeId"/> or <see langword="null"/> if the type was not found.</returns>
    internal static ITypeNameData? GetType(string typeId, ITypeRegistry? typeRegistry)
    {
        // try to find it in the type registry
        if (typeRegistry?.GetDeclaredType(typeId) is ITypeDeclaration type)
        {
            return type.TypeObject.GetTypeNameData();
        }

        try
        {
            // try to get it from loaded assemblies
            return Type.GetType(typeId, false)?.GetTypeNameData();
        }
        catch (Exception ex) when (ex is ArgumentNullException or TargetInvocationException or TypeLoadException or ArgumentException)
        {
            return null;
        }
    }
}
