using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a type.
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
    /// Checks if the type is abstract. True for abstract classes and interfaces.
    /// </summary>
    bool IsAbstract { get; }

    bool IsInterface { get; }

    bool IsValueType { get; }

    bool IsStatic { get; }

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
