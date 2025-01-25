using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Concrete.Types;
using System.Reflection;

namespace RefDocGen.CodeElements.Concrete.Members.Enum;

/// <summary>
/// Class representing data of an enum member.
/// </summary>
internal class EnumMemberData : MemberData, IEnumMemberData
{
    /// <summary>
    /// Initialize a new instance of the <see cref="EnumMemberData"/> class.
    /// </summary>
    /// <param name="fieldInfo"><see cref="System.Reflection.FieldInfo"/> object representing the enum member.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the enum member.</param>
    public EnumMemberData(FieldInfo fieldInfo, TypeDeclaration containingType, IReadOnlyList<IAttributeData> attributes)
        : base(fieldInfo, containingType, attributes)
    {
        FieldInfo = fieldInfo;
        Value = fieldInfo.GetRawConstantValue() ?? 0;
    }

    /// <inheritdoc/>
    public FieldInfo FieldInfo { get; }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(
        FieldInfo.IsPrivate,
        FieldInfo.IsFamily,
        FieldInfo.IsAssembly,
        FieldInfo.IsPublic,
        FieldInfo.IsFamilyAndAssembly,
        FieldInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => false;

    /// <inheritdoc/>
    public object Value { get; }
}
