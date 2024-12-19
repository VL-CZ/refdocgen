using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Handlers.Tools;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction.Handlers.Members;

/// <summary>
/// Abstract class responsible for handling and adding XML doc comments to the corresponding executable type members.
/// <para>
/// See also <seealso cref="ExecutableMemberData"/> class.
/// </para>
/// </summary>
/// <typeparam name="T">Type of the member to which the doc is assigned.</typeparam>
internal abstract class ExecutableMemberDocHandler<T> : MemberDocHandler<ObjectTypeData, T>
    where T : ExecutableMemberData
{
    /// <inheritdoc/>
    protected override void AddRemainingComments(T member, XElement memberDocComment)
    {
        // add parameter doc comments
        var paramElements = memberDocComment.Descendants(XmlDocIdentifiers.Param);
        ParameterDocHelper.Add(paramElements, member.Parameters);

        // add exception doc comments (if present)
        var exceptionsDocComments = memberDocComment.Descendants(XmlDocIdentifiers.Exception);
        member.Exceptions = ExceptionDocHelper.Parse(exceptionsDocComments);
    }
}

