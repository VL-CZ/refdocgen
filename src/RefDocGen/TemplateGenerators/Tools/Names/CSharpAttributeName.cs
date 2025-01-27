using RefDocGen.CodeElements.Abstract.Types.Attribute;

namespace RefDocGen.TemplateGenerators.Tools.Names;

/// <summary>
/// Static class used for retrieving names of attribute instances in C# format.
/// </summary>
internal static class CSharpAttributeName
{
    /// <summary>
    /// Suffix of the attribute class name.
    /// </summary>
    private const string attributeSuffix = "Attribute";

    /// <summary>
    /// Get the name of the given attribute instance in C# format.
    /// </summary>
    /// <param name="attribute">The attribute instance, whose name is retrieved.</param>
    /// <returns>Name of the attribute instance formatted according to C# conventions.</returns>
    internal static string Of(IAttributeData attribute)
    {
        var type = attribute.Type;
        string attributeName = type.ShortName;

        if (attributeName.EndsWith(attributeSuffix, StringComparison.InvariantCulture))
        {
            attributeName = attributeName[..^attributeSuffix.Length];
        }

        if (type.HasTypeParameters)
        {
            string genericParamsString = string.Join(", ", type.TypeParameters.Select(CSharpTypeName.Of));
            attributeName += '<' + genericParamsString + '>'; // add generic params to the type name
        }

        return attributeName;
    }
}

