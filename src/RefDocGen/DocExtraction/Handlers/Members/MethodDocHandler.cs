using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodDocHandler : ExecutableMemberDocHandler<MethodData>
{
    private ObjectTypeData typeData = null!; // TODO: huge code smell

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, MethodData> GetMembers(ObjectTypeData type)
    {
        typeData = type;
        return type.Methods;
    }

    /// <inheritdoc/>
    protected override void AddRemainingComments(MethodData member, XElement memberDocComment)
    {
        base.AddRemainingComments(member, memberDocComment);

        // add return value doc comment (if present)
        if (memberDocComment.TryGetReturnsElement(out var returnsNode))
        {
            member.ReturnValueDocComment = returnsNode;
        }

        // inheritdoc - TODO: update
        if (memberDocComment.TryGetElement("inheritdoc", out var _))
        {
            var ih = DocCommentExtractor.inheritDocHandler;

            ih.AddFromParent(typeData, member);
        }
    }
}
