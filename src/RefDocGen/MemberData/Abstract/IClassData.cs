using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a class.
/// </summary>
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

    /// <summary>
    /// Collection of constructors declared in the class.
    /// </summary>
    IReadOnlyList<IConstructorData> Constructors { get; }

    /// <summary>
    /// Collection of fields declared in the class.
    /// </summary>
    IReadOnlyList<IFieldData> Fields { get; }

    /// <summary>
    /// Collection of methods declared in the class.
    /// </summary>
    IReadOnlyList<IMethodData> Methods { get; }

    /// <summary>
    /// Collection of properties declared in the class.
    /// </summary>
    IReadOnlyList<IPropertyData> Properties { get; }
}
