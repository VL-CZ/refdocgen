using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents data of a value, reference or interface type; including its members.
/// <para>
/// Note: This interface doesn't represent enum types - see <see cref="IEnumTypeData"/>.
/// </para>
/// </summary>
public interface IObjectTypeData : ITypeNameData
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

    /// <summary>
    /// Base class of the current class
    /// </summary>
    ITypeNameData? BaseType { get; }

    /// <summary>
    /// Interfaces implemented by the current type.
    /// </summary>
    IReadOnlyList<ITypeNameData> Interfaces { get; }
}
