using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;
public interface IClassData
{
    /// <summary>
    /// Access modifier of the class.
    /// </summary>
    AccessModifier AccessModifier { get; }

    /// <summary>
    /// Name of the class.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Documentation comment provided to the class.
    /// </summary>
    XElement DocComment { get; }

    IReadOnlyList<IConstructorData> Constructors { get; }

    IReadOnlyList<IFieldData> Fields { get; }

    IReadOnlyList<IMethodData> Methods { get; }

    IReadOnlyList<IPropertyData> Properties { get; }
}
