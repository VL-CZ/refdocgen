using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Types;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding delegate types.
/// </summary>
internal class DelegateTypeDocHandler : TypeDocHandler
{
    /// <summary>
    /// Adds documentation to the given delegate type.
    /// </summary>
    /// <param name="d">Delegate type to which the documentation is added.</param>
    /// <param name="docComment">Doc comment for the delegate.</param>
    internal void AddDocumentation(DelegateTypeData d, XElement docComment)
    {
        base.AddDocumentation(d, docComment);

        // add 'returns' doc comment (if present)
        if (docComment.TryGetReturnsElement(out var returnsNode))
        {
            d.ReturnValueDocComment = returnsNode;
        }

        // add param doc comments
        var paramElements = docComment.Descendants(XmlDocIdentifiers.Param);
        ParameterDocHelper.Add(paramElements, d.Parameters);

        // add exception doc comments
        var exceptionsDocComments = docComment.Descendants(XmlDocIdentifiers.Exception);
        d.Exceptions = ExceptionDocHelper.GetFrom(exceptionsDocComments);
    }
}
