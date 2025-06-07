using System.Reflection;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

/// <summary>
/// Contains additoinal tools related to types.
/// </summary>
internal class TypeTools
{
    /// <summary>
    /// Gets a <see cref="ITypeNameData"/> instance based on the provided <paramref name="typeId"/>.
    /// </summary>
    /// <param name="typeId">The ID of the type to be found.</param>
    /// <returns>A <see cref="ITypeNameData"/> instance based on the provided <paramref name="typeId"/> or <see langword="null"/> if the type was not found.</returns>
    internal static ITypeNameData? GetType(string typeId)
    {
        try
        {
            return Type.GetType(typeId, false)?.GetTypeNameData();
        }
        catch (Exception ex) when (ex is ArgumentNullException or TargetInvocationException or TypeLoadException or ArgumentException)
        {
            return null;
        }
    }
}
