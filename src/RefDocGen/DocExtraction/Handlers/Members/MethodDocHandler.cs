using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Class responsible for handling and adding XML doc comments to the corresponding methods.
/// </summary>
internal class MethodDocHandler : MethodLikeMemberDocHandler<MethodData>
{
    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, MethodData> GetMembers(ObjectTypeData type)
    {
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

        // add type parameter doc comments
        var typeParamElements = memberDocComment.Descendants(XmlDocIdentifiers.TypeParam);
        TypeParameterDocHelper.Add(typeParamElements, member.TypeParameters);
    }
}
