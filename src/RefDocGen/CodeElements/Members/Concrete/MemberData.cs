using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Members.Concrete;

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

        if (containingType.TypeObject != memberInfo.DeclaringType)
        {
            IsInherited = true;
            InheritedFrom = memberInfo.DeclaringType?.GetTypeNameData();
        }
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
    public bool IsInherited { get; }

    /// <inheritdoc/>
    public ITypeNameData? InheritedFrom { get; }

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

    /// <summary>
    /// ID of the member kind.
    /// </summary>
    /// <remarks>
    /// For further info, see the table at <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>.
    /// </remarks>
    internal abstract string MemberKindId { get; }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return $"{MemberKindId}:{ContainingType}.{Id}";
    }
}
