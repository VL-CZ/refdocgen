using System.Xml.Linq;
using RefDocGen.CodeElements.Abstract.Members;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents data of a type, including its members.
/// </summary>
public interface ITypeData : ITypeNameData
{
    /// <summary>
    /// Access modifier of the type.
    /// </summary>
    AccessModifier AccessModifier { get; }

    /// <summary>
    /// Documentation comment provided to the type.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Checks if the type is abstract.
    /// True for abstract classes and interfaces.
    /// </summary>
    bool IsAbstract { get; }

    /// <summary>
    /// Kind of the type.
    /// </summary>
    TypeKind Kind { get; }

    /// <summary>
    /// Checks if the type is sealed (i.e. cannot be inherited)
    /// </summary>
    bool IsSealed { get; }

    /// <summary>
    /// Collection of constructors declared in the type.
    /// </summary>
    IReadOnlyList<IConstructorData> Constructors { get; }

    /// <summary>
    /// Collection of fields declared in the type.
    /// </summary>
    IReadOnlyList<IFieldData> Fields { get; }

    /// <summary>
    /// Collection of methods declared in the type.
    /// </summary>
    IReadOnlyList<IMethodData> Methods { get; }

    /// <summary>
    /// Collection of properties declared in the type.
    /// </summary>
    IReadOnlyList<IPropertyData> Properties { get; }
}
