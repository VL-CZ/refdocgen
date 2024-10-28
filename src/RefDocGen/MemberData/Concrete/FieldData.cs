using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Represents data of a field.
/// </summary>
/// <param name="FieldInfo"><see cref="System.Reflection.FieldInfo"/> object representing the field.</param>
internal record FieldData(FieldInfo FieldInfo) : IFieldData
{
    /// <inheritdoc/>
    public string Id => Name;

    /// <inheritdoc/>
    public string Name => FieldInfo.Name;

    /// <inheritdoc/>
    public string Type => FieldInfo.FieldType.Name;

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => FieldInfo.IsStatic;

    /// <inheritdoc/>
    public bool IsReadonly => FieldInfo.IsInitOnly;

    /// <inheritdoc/>
    public bool IsConstant => FieldInfo.IsLiteral;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;
}
