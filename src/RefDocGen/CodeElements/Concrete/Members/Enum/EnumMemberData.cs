using RefDocGen.CodeElements.Abstract.Members.Enum;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members.Enum;

/// <summary>
/// Class representing data of an enum member.
/// </summary>
internal class EnumMemberData : IEnumMemberData
{
    /// <summary>
    /// Initialize a new instance of the <see cref="EnumMemberData"/> class.
    /// </summary>
    /// <param name="fieldInfo"><see cref="System.Reflection.FieldInfo"/> object representing the enum member.</param>
    public EnumMemberData(FieldInfo fieldInfo)
    {
        FieldInfo = fieldInfo;
    }

    /// <inheritdoc/>
    public FieldInfo FieldInfo { get; }

    /// <inheritdoc/>
    public string Id => Name;

    /// <inheritdoc/>
    public string Name => FieldInfo.Name;

    /// <inheritdoc/>
    public XElement SummaryDocComment { get; internal set; } = XmlDocElements.EmptySummary;
}
