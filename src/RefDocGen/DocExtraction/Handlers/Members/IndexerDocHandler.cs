using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding indexers.
/// </summary>
internal class IndexerDocHandler : MemberDocHandler<ObjectTypeData, IndexerData>
{
    /// <inheritdoc/>
    protected override void AddRemainingComments(IndexerData member, XElement memberDocComment)
    {
        // add 'value' doc comment
        if (memberDocComment.TryGetValueElement(out var valueNode))
        {
            member.ValueDocComment = valueNode;
        }

        // add 'param' doc comments
        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);
        ParameterDocHelper.Add(paramElements, member.Parameters);

        // add 'exception' doc comments
        var exceptionsDocComments = memberDocComment.Descendants(XmlDocIdentifiers.Exception);
        member.DocumentedExceptions = ExceptionDocHelper.Parse(exceptionsDocComments);
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, IndexerData> GetMembers(ObjectTypeData type)
    {
        return type.Indexers;
    }
}
