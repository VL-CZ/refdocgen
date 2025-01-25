using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
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
    /// Initializes a new instance of the <see cref="MemberData"/> class.
    /// </summary>
    /// <param name="memberInfo"><see cref="MemberInfo"/> object representing the member.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the member.</param>
    protected MemberData(MemberInfo memberInfo, TypeDeclaration containingType, IReadOnlyList<IAttributeData> attributes)
    {
        MemberInfo = memberInfo;
        ContainingType = containingType;
        Attributes = attributes;
    }

    /// <inheritdoc/>
    public virtual string Id => Name;

    /// <inheritdoc/>
    public virtual string Name => MemberInfo.Name;

    /// <inheritdoc/>
    public abstract AccessModifier AccessModifier { get; }

    /// <inheritdoc/>
    public abstract bool IsStatic { get; }

    /// <inheritdoc/>
    public XElement SummaryDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public XElement RemarksDocComment { get; internal set; } = XmlDocElements.EmptyRemarks;

    /// <inheritdoc/>
    public IEnumerable<XElement> SeeAlsoDocComments { get; internal set; } = [];

    /// <inheritdoc/>
    public MemberInfo MemberInfo { get; }

    /// <inheritdoc cref="IMemberData.ContainingType"/>
    public TypeDeclaration ContainingType { get; }

    /// <inheritdoc/>
    ITypeDeclaration IMemberData.ContainingType => ContainingType;

    /// <inheritdoc/>
    public IReadOnlyList<IAttributeData> Attributes { get; }

    /// <summary>
    /// Raw doc comment provided to the member.
    ///
    /// <para>
    /// <see langword="null"/> if the member isn't documented.
    /// </para>
    /// </summary>
    internal XElement? RawDocComment { get; set; }
}
