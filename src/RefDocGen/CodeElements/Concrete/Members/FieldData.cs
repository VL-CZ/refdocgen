using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using System.Reflection;

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
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    internal FieldData(FieldInfo fieldInfo, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters) : base(fieldInfo)
    {
        FieldInfo = fieldInfo;
        Type = fieldInfo.FieldType.GetNameData(declaredTypeParameters);
    }

    /// <inheritdoc/>
    public FieldInfo FieldInfo { get; }

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => FieldInfo.IsStatic;

    /// <inheritdoc/>
    public bool IsReadonly => FieldInfo.IsInitOnly;

    /// <inheritdoc/>
    public bool IsConstant => FieldInfo.IsLiteral;
}
