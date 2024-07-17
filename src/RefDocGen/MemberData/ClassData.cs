using RefDocGen.DocExtraction;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a class.
/// </summary>
/// <param name="Name">Name of the class.</param>
/// <param name="AccessModifier">Access modifier of the class.</param>
/// <param name="Fields">Array of fields declared in the class.</param>
/// <param name="Properties">Array of properties declared in the class.</param>
/// <param name="Methods">Array of methods declared in the class.</param>
public record ClassData(string Name, AccessModifier AccessModifier, FieldData[] Fields, PropertyData[] Properties, MethodData[] Methods)
{
    /// <summary>
    /// Documentation comment provided to the class.
    /// </summary>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummary;
}
