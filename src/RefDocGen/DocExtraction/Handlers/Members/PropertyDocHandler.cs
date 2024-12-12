using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding properties.
/// </summary>
internal class PropertyDocHandler : MemberDocHandler<PropertyData>
{
    /// <inheritdoc/>
    protected override void AddRemainingComments(PropertyData member, XElement memberDocComment)
    {
        // add 'value' doc comment
        if (memberDocComment.TryGetValueElement(out var valueNode))
        {
            member.ValueDocComment = valueNode;
        }

        // add 'exception' doc comments
        var exceptionsDocComments = memberDocComment.Descendants(XmlDocIdentifiers.Exception);
        member.Exceptions = ExceptionDocHelper.Parse(exceptionsDocComments);
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, PropertyData> GetMembers(ObjectTypeData type)
    {
        return type.Properties;
    }
}
