using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Tools;
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
        // add 'summary' doc comment
        if (docComment.TryGetSummaryElement(out var summaryNode))
        {
            type.SummaryDocComment = summaryNode;
        }

        // add 'remarks' doc comment
        if (docComment.TryGetRemarksElement(out var remarksNode))
        {
            type.RemarksDocComment = remarksNode;
        }

        // add 'seealso' doc comments
        type.SeeAlsoDocComments = docComment.Elements(XmlDocIdentifiers.SeeAlso);

        // add raw doc comment
        type.RawDocComment = docComment;

        // add 'typeparam' doc comments
        var typeParamElements = docComment.Descendants(XmlDocIdentifiers.TypeParam);
        TypeParameterDocHelper.Add(typeParamElements, type.AllTypeParameters);
    }
}
