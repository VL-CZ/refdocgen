using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Types;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding delegate types.
/// </summary>
internal class DelegateTypeDocHandler
{
    /// <summary>
    /// Adds documentation to the given delegate type.
    /// </summary>
    /// <param name="d">Delegate type to which the documentation is added.</param>
    /// <param name="docComment">Doc comment for the delegate.</param>
    internal void AddDocumentation(DelegateTypeData d, XElement docComment)
    {
        if (docComment.TryGetSummaryElement(out var summaryNode))
        {
            d.DocComment = summaryNode;
        }

        if (docComment.TryGetReturnsElement(out var returnsNode))
        {
            d.ReturnValueDocComment = returnsNode;
        }

        // add param doc comments
        var paramElements = docComment.Descendants(XmlDocIdentifiers.Param);

        foreach (var paramElement in paramElements)
        {
            if (paramElement.TryGetNameAttribute(out var nameAttr))
            {
                string paramName = nameAttr.Value;
                var parameter = d.Parameters.FirstOrDefault(p => p.Name == paramName);

                if (parameter is null)
                {
                    // TODO: log parameter not found
                    return;
                }

                parameter.DocComment = paramElement;
            }
        }

        // add type param doc comments
        var typeParamElements = docComment.Descendants(XmlDocIdentifiers.TypeParam);

        foreach (var typeParamDocComment in typeParamElements)
        {
            if (typeParamDocComment.TryGetNameAttribute(out var nameAttr))
            {
                string typeParamName = nameAttr.Value;

                if (d.TypeParameterDeclarations.TryGetValue(typeParamName, out var typeParam))
                {
                    typeParam.DocComment = typeParamDocComment;
                }
            }
        }
    }
}
