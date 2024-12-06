namespace RefDocGen.TemplateGenerators.Default.Templates;

/// <summary>
/// Represents different kinds of Razor templates.
/// </summary>
internal enum TemplateKind
{
    /// <summary>
    /// Razor template representing an object type.
    /// </summary>
    ObjectType,

    /// <summary>
    /// Razor template representing an enum type.
    /// </summary>
    EnumType,

    /// <summary>
    /// Razor template representing a delegate type.
    /// </summary>
    DelegateType,

    /// <summary>
    /// Razor template representing a namespace list.
    /// </summary>
    NamespaceList,

    /// <summary>
    /// Razor template representing a namespace detail.
    /// </summary>
    NamespaceDetail
}

/// <summary>
/// Class containing extension methods for <see cref="TemplateKind"/> enum.
/// </summary>
internal static class TemplateKindExtensions
{
    /// <summary>
    /// Get the name of the file containing the template, relative to the folder containing the templates.
    /// </summary>
    /// <param name="kind">Template kind, whose file name we retrieve.</param>
    /// <returns>The name of the file containing the template, relative to the folder containing the templates.</returns>
    internal static string GetFileName(this TemplateKind kind)
    {
        return kind switch
        {
            TemplateKind.ObjectType => "ObjectType.cshtml",
            TemplateKind.EnumType => "EnumType.cshtml",
            TemplateKind.DelegateType => "DelegateType.cshtml",
            TemplateKind.NamespaceList => "NamespaceList.cshtml",
            TemplateKind.NamespaceDetail => "NamespaceDetail.cshtml",
            _ => throw new ArgumentException("Illegal argument passed.")
        };
    }
}
