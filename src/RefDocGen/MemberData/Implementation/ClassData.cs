using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Implementation;

/// <summary>
/// Represents data of a class.
/// </summary>
/// <param name="Name">Name of the class.</param>
/// <param name="AccessModifier">Access modifier of the class.</param>
/// <param name="Constructors">Dictionary of constructors declared in the class; keys are the corresponding constructor IDs</param>
/// <param name="Fields">Dictionary of fields declared in the class; keys are the corresponding field IDs.</param>
/// <param name="Properties">Dictionary of properties declared in the class; keys are the corresponding property IDs.</param>
/// <param name="Methods">Dictionary of methods declared in the class; keys are the corresponding method IDs.</param>
internal record ClassData(string Name, AccessModifier AccessModifier, Dictionary<string, ConstructorData> Constructors,
    Dictionary<string, FieldData> Fields, Dictionary<string, PropertyData> Properties, Dictionary<string, MethodData> Methods) : IClassData
{
    /// <inheritdoc/>
    public XElement DocComment { get; init; } = XmlDocElementFactory.EmptySummary;

    IReadOnlyList<IConstructorData> IClassData.Constructors => Constructors.Values.ToList();

    IReadOnlyList<IFieldData> IClassData.Fields => Fields.Values.ToList();

    IReadOnlyList<IMethodData> IClassData.Methods => Methods.Values.ToList();

    IReadOnlyList<IPropertyData> IClassData.Properties => Properties.Values.ToList();
}
