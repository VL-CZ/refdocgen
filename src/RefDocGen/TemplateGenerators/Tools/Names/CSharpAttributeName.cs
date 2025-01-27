using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Tools.Names;

/// <summary>
/// Static class used for retrieving type names in C# format.
/// </summary>
internal static class CSharpAttributeName
{
    private const string attributeSuffix = "Attribute";

    /// <summary>
    /// Get the name of the given attribute in C# format.
    /// </summary>
    /// <param name="attribute">The attribute, whose name is retrieved.</param>
    /// <returns>Name of the attribute formatted according to C# conventions.</returns>
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

