using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding method-like type members.
/// </summary>
/// <typeparam name="T">Type of the member to which the doc is assigned.</typeparam>
/// <seealso cref="MethodLikeMemberData"/>
internal abstract class MethodLikeMemberDocHandler<T> : MemberDocHandler<ObjectTypeData, T>
    where T : MethodLikeMemberData
{
    /// <inheritdoc/>
    protected override void AddRemainingComments(T member, XElement memberDocComment)
    {
        // add parameter doc comments
        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);
        ParameterDocHelper.Add(paramElements, member.Parameters);

        // add exception doc comments (if present)
        var exceptionsDocComments = memberDocComment.Descendants(XmlDocIdentifiers.Exception);
        member.DocumentedExceptions = ExceptionDocHelper.Parse(exceptionsDocComments);
    }
}

