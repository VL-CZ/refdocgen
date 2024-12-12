using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding operators.
/// </summary>
internal class OperatorDocHandler : ExecutableMemberDocHandler<OperatorData>
{
    /// <inheritdoc/>
    protected override void AddRemainingComments(OperatorData member, XElement memberDocComment)
    {
        base.AddRemainingComments(member, memberDocComment);

        // add return value doc comment (if present)
        if (memberDocComment.TryGetReturnsElement(out var returnsNode))
        {
            member.ReturnValueDocComment = returnsNode;
        }
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, OperatorData> GetMembers(ObjectTypeData type)
    {
        return type.Operators;
    }
}
