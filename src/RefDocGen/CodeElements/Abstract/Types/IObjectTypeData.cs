using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Enum;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents data of a value, reference or interface type; including its members.
/// <para>
/// Note: This interface doesn't represent enum types - see <see cref="IEnumTypeData"/>.
/// </para>
/// </summary>
public interface IObjectTypeData : ITypeDeclaration
{
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
}
