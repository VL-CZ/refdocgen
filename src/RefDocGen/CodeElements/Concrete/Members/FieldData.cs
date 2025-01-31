using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Attribute;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a field.
/// </summary>
internal class FieldData : MemberData, IFieldData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldData"/> class.
    /// </summary>
    /// <param name="fieldInfo"><see cref="System.Reflection.FieldInfo"/> object representing the field.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="containingType">Type that contains the member.</param>
    /// <param name="attributes">Collection of attributes applied to the field.</param>
    internal FieldData(
        FieldInfo fieldInfo,
        TypeDeclaration containingType,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes) : base(fieldInfo, containingType, attributes)
    {
        FieldInfo = fieldInfo;
        Type = fieldInfo.FieldType.GetTypeNameData(availableTypeParameters);
    }

    /// <inheritdoc/>
    public FieldInfo FieldInfo { get; }

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierHelper.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => FieldInfo.IsStatic;

    /// <inheritdoc/>
    public bool IsReadonly => FieldInfo.IsInitOnly;

    /// <inheritdoc/>
    public bool IsConstant => FieldInfo.IsLiteral;

    /// <inheritdoc/>
    public object? ConstantValue => IsConstant
        ? FieldInfo.GetRawConstantValue()
        : DBNull.Value;

    /// <inheritdoc/>
    public bool IsRequired => FieldInfo.IsDefined(typeof(RequiredMemberAttribute), false);
}
