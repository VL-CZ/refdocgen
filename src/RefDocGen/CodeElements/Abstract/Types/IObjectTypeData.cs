using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;

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
    /// Indicates whether the type is a byref-like structure.
    /// </summary>
    /// <remarks>
    /// For further info, see <see href="https://learn.microsoft.com/en-us/dotnet/api/system.type.isbyreflike"/>
    /// </remarks>
    bool IsByRefLike { get; }

    /// <summary>
    /// Collection of constructors contained in the type.
    /// </summary>
    IEnumerable<IConstructorData> Constructors { get; }

    /// <summary>
    /// Collection of fields contained in the type.
    /// </summary>
    IEnumerable<IFieldData> Fields { get; }

    /// <summary>
    /// Collection of methods contained in the type.
    /// </summary>
    IEnumerable<IMethodData> Methods { get; }

    /// <summary>
    /// Collection of properties contained in the type.
    /// </summary>
    IEnumerable<IPropertyData> Properties { get; }

    /// <summary>
    /// Collection of operators contained in the type.
    /// </summary>
    IEnumerable<IOperatorData> Operators { get; }

    /// <summary>
    /// Collection of indexers contained in the type.
    /// </summary>
    IEnumerable<IIndexerData> Indexers { get; }

    /// <summary>
    /// Collection of indexers contained in the type.
    /// </summary>
    IEnumerable<IEventData> Events { get; }

    IEnumerable<ITypeDeclaration> NestedTypes { get; }
}
