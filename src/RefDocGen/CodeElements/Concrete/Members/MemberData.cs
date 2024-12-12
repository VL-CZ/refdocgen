using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

internal abstract class MemberData : IMemberData
{
    private readonly MemberInfo memberInfo;

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
