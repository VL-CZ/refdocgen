using RefDocGen.TemplateProcessors.Default.Templates.Components.LanguageSpecific;
using RefDocGen.Tools.Exceptions;

namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Enum representing the language specific fragment types.
/// </summary>
public enum LanguageSpecificComponent
{
    AttributeDeclaration,
    ConstructorDeclaration,
    DelegateMethodDeclaration,
    EnumMemberDeclaration,
    EventDeclaration,
    FieldDeclaration,
    GenericTypeLink,
    IndexerDeclaration,
    MethodDeclaration,
    OperatorDeclaration,
    ParameterDeclaration,
    PropertyDeclaration,
    TypeDeclaration,
    TypeParameterDeclaration,
    TypeParameterConstraints,
}
#pragma warning restore CS1591

/// <summary>
/// Class containing extension methods for the <see cref="LanguageSpecificComponent"/> enum.
/// </summary>
public static class LanguageSpecificComponentExtensions
{
    /// <summary>
    /// Gets the <see cref="Type"/> object describing the language specific component of the given kind, which is contained in the <paramref name="componentsFolderName"/> folder.
    /// </summary>
    /// <param name="componentsFolderName">Name of the folder inside the 'TemplateProcessors/Default/Templates/Components/LanguageSpecific' directory
    /// that contains the language-specific components.</param>
    /// <param name="componentKind">Kind of the language-specific component to return.</param>
    /// <returns>A <see cref="Type"/> object describing the language specific component of the given kind, specified by <paramref name="componentKind"/>,
    /// which is contained in <paramref name="componentsFolderName"/> folder.</returns>
    /// <exception cref="LanguageSpecificComponentNotFoundException">Thrown when the language-specific component is not found.</exception>
    public static Type GetFromFolder(this LanguageSpecificComponent componentKind, string componentsFolderName)
    {
        string? baseComponentNs = typeof(LanguageFragments).Namespace;
        string componentName = componentKind.ToString();

        string componentFullName = $"{baseComponentNs}.{componentsFolderName}.{componentsFolderName}{componentName}";

        return Type.GetType(componentFullName)
            ?? throw new LanguageSpecificComponentNotFoundException(componentFullName); // language-specific component not found -> throw an exception
    }
}
