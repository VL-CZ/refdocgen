using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a type member.
/// </summary>
internal abstract class MemberData : IMemberData
{
    /// <summary>
    /// <see cref="MemberInfo"/> object representing the member.
    /// </summary>
    private readonly MemberInfo memberInfo;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemberData"/> class.
    /// </summary>
    /// <param name="memberInfo"><see cref="MemberInfo"/> object representing the member.</param>
    protected MemberData(MemberInfo memberInfo)
    {
        this.memberInfo = memberInfo;
    }

    /// <inheritdoc/>
    public virtual string Id => Name;

    /// <inheritdoc/>
    public virtual string Name => memberInfo.Name;

    /// <inheritdoc/>
    public abstract AccessModifier AccessModifier { get; }

    /// <inheritdoc/>
    public abstract bool IsStatic { get; }

    /// <inheritdoc/>
    public XElement SummaryDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public XElement RemarksDocComment { get; internal set; } = XmlDocElements.EmptyRemarks;
}
