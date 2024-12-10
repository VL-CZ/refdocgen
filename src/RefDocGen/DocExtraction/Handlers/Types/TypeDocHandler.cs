using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Types;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding types.
/// </summary>
internal class TypeDocHandler
{
    /// <summary>
    /// Adds documentation to the given type.
    /// </summary>
    /// <param name="type">Type to which the documentation is added.</param>
    /// <param name="docComment">Doc comment for the type.</param>
    internal virtual void AddDocumentation(TypeDeclaration type, XElement docComment)
    {
        // add summary doc comment
        if (docComment.TryGetSummaryElement(out var summaryNode))
        {
            type.DocComment = summaryNode;
        }

        var typeParamElements = docComment.Descendants(XmlDocIdentifiers.TypeParam);

        // add type param doc comments
        foreach (var typeParamDocComment in typeParamElements)
        {
            AddTypeParamDocumentation(type, typeParamDocComment);
        }
    }

    /// <summary>
    /// Add the doc comment to the corresponding type parameter.
    /// </summary>
    /// <param name="type">The type containing the parameter.</param>
    /// <param name="docComment">XML node of the doc comment for the given type parameter.</param>
    private void AddTypeParamDocumentation(TypeDeclaration type, XElement docComment)
    {
        if (docComment.TryGetNameAttribute(out var nameAttr))
        {
            string typeParamName = nameAttr.Value;

            if (type.TypeParameterDeclarations.TryGetValue(typeParamName, out var typeParam))
            {
                typeParam.DocComment = docComment;
            }
        }
    }
}
