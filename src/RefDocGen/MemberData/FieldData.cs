using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a field.
/// </summary>
/// <param name="FieldInfo"><see cref="System.Reflection.FieldInfo"/> object representing the field.</param>
public record FieldData(FieldInfo FieldInfo) : IMemberData
{
    /// <inheritdoc/>
    public string Name => FieldInfo.Name;

    /// <summary>
    /// Type of the field.
    /// </summary>
    public string Type => FieldInfo.FieldType.Name;

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(FieldInfo.IsPrivate, FieldInfo.IsFamily,
        FieldInfo.IsAssembly, FieldInfo.IsPublic, FieldInfo.IsFamilyAndAssembly, FieldInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => FieldInfo.IsStatic;

    /// <summary>
    /// Checks if the field is readonly.
    /// </summary>
    public bool IsReadonly => FieldInfo.IsInitOnly;

    /// <summary>
    /// Checks if the field is constant.
    /// </summary>
    public bool IsConstant => FieldInfo.IsLiteral;

    /// <summary>
    /// Gets the XMl doc comment for this field.
    /// </summary>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummaryNode;
}
