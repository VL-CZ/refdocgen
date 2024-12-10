using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Tools;

namespace RefDocGen.CodeElements.Concrete.Types.Enum;

/// <summary>
/// Class representing data of an enum, including its members.
/// </summary>
internal class EnumTypeData : TypeNameBaseData, IEnumTypeData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type.</param>
    /// <param name="members">Dictionary containing the enum members; keys are the corresponding member IDs.</param>
    public EnumTypeData(Type type, IReadOnlyDictionary<string, EnumMemberData> members) : base(type)
    {
        Members = members;
    }

    /// <inheritdoc/>
    public override string Id => TypeId.Of(this);

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <summary>
    /// Access modifier of the enum.
    /// </summary>
    public AccessModifier AccessModifier =>
        AccessModifierExtensions.GetAccessModifier(
            TypeObject.IsNestedPrivate,
            TypeObject.IsNestedFamily,
            TypeObject.IsNestedAssembly || TypeObject.IsNotPublic,
            TypeObject.IsPublic || TypeObject.IsNestedPublic,
            TypeObject.IsNestedFamANDAssem,
            TypeObject.IsNestedFamORAssem);

    /// <summary>
    /// Dictionary containing the enum members; keys are the corresponding member IDs.
    /// </summary>
    public IReadOnlyDictionary<string, EnumMemberData> Members { get; }

    /// <inheritdoc/>
    IReadOnlyList<IEnumMemberData> IEnumTypeData.Members => Members.Values.ToList();

    /// <inheritdoc/>
    bool ITypeDeclaration.HasTypeParameters => false;

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterDeclaration> ITypeDeclaration.TypeParameterDeclarations => [];
}
