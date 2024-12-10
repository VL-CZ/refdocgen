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
    /// Collection of operators declared in the type.
    /// </summary>
    IReadOnlyList<IOperatorData> Operators { get; }

    /// <summary>
    /// Collection of indexers declared in the type.
    /// </summary>
    IReadOnlyList<IIndexerData> Indexers { get; }

    /// <summary>
    /// Collection of generic type parameters declared in the type, ordered by their index.
    /// </summary>
    IReadOnlyList<ITypeParameterDeclaration> TypeParameterDeclarations { get; }

    /// <summary>
    /// Base type of the type. Returns null, if the type has no base type (i.e. it's an interface or <see cref="object"/> type).
    /// </summary>
    ITypeNameData? BaseType { get; }

    /// <summary>
    /// Interfaces implemented by the current type.
    /// </summary>
    IReadOnlyList<ITypeNameData> Interfaces { get; }
}
