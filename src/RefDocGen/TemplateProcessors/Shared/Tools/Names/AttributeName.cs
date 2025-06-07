using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.TemplateProcessors.Shared.Languages;

namespace RefDocGen.TemplateProcessors.Shared.Tools.Names;

/// <summary>
/// Static class used for retrieving names of attribute names.
/// </summary>
internal static class AttributeName
{
    /// <summary>
    /// Suffix of the attribute class name.
    /// </summary>
    private const string attributeSuffix = "Attribute";

    /// <summary>
    /// Get the name of the given attribute.
    /// </summary>
    /// <param name="language">Language, in which the attribute name is returned.</param>
    /// <param name="attribute">The attribute instance, whose name is retrieved.</param>
    /// <returns>Name of the attribute instance.</returns>
    internal static string Of(ILanguageConfiguration language, IAttributeData attribute)
    {
        string name = language.GetTypeName(attribute.Type);

        int attributeSuffixPosition = name.LastIndexOf(attributeSuffix, StringComparison.Ordinal);

        if (attributeSuffixPosition == -1)
        {
            return name;
        }
        else
        {
            // remove the 'Attribute' suffix.
            return name.Remove(attributeSuffixPosition, attributeSuffix.Length);
        }
    }
}

