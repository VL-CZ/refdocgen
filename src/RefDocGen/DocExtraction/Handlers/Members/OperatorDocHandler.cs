using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding operators.
/// </summary>
internal class OperatorDocHandler : MethodLikeMemberDocHandler<OperatorData>
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

        // add type parameter doc comments
        var typeParamElements = memberDocComment.Descendants(XmlDocIdentifiers.TypeParam);
        TypeParameterDocHelper.Add(typeParamElements, member.TypeParameters);
    }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, OperatorData> GetMembers(ObjectTypeData type)
    {
        return type.Operators;
    }
}
