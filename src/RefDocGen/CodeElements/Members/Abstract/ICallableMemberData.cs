using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of a callable type member (such as a method, property or an event).
///
/// <para>
/// Note that these members can typically be declared in an interface or possibly overriden by child classes.
/// </para>
/// </summary>
/// <remarks>
/// Note that constructors and fields are excluded from this definition.
/// </remarks>
public interface ICallableMemberData : IMemberData
{
    /// <summary>
    /// Checks whether the member can be overridden.
    /// <para>True for virtual non-final methods</para>
    /// </summary>
    bool IsOverridable { get; }

    /// <summary>
    /// Checks whether the member overrides another member.
    /// </summary>
    bool OverridesAnotherMember { get; }

    /// <summary>
    /// Checks whether the member is abstract.
    /// </summary>
    bool IsAbstract { get; }

    /// <summary>
    /// Checks whether the member is final.
    /// <para>
    ///     For further information, see <see href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isfinal"></see>
    /// </para>
    /// </summary>
    bool IsFinal { get; }

    /// <summary>
    /// Checks whether the member is sealed, i.e. virtual that cannot be further overriden.
    /// </summary>
    bool IsSealed { get; }

    /// <summary>
    /// Checks whether the member is asynchronous.
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// Checks whether the member is virtual.
    /// <para>True for virtual and abstract members.</para>
    /// </summary>
    bool IsVirtual { get; }

    /// <summary>
    /// Represents a collection of exceptions documented for the member.
    /// </summary>
    /// <remarks>
    /// This collection includes only the exceptions explicitly documented using the <c>exception</c> XML tag. 
    /// It does not include all possible exceptions that might occur during execution.
    /// </remarks>
    IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; }

    /// <summary>
    /// Checks if the member is an explicitely impletemented member of an interface.
    /// </summary>
    bool IsExplicitImplementation { get; }

    /// <summary>
    /// Type of the interface that explicitly declares the member.
    /// 
    /// <para>
    /// <c>null</c>, if the member is not explicitly declared.
    /// </para>
    /// </summary>
    /// <remarks>
    /// For further info, see <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation"/>
    /// </remarks>
    ITypeNameData? ExplicitInterfaceType { get; }

    /// <summary>
    /// If the member overrides another member, this property returns the base type that originally declared the member.
    /// <c>null</c> is returned if the member doesn't override anything.
    /// </summary>
    ITypeNameData? BaseDeclaringType { get; }

    /// <summary>
    /// Returns the interface types, whose part of contract is implemented by this member.
    /// Note that explicitly implemented interfaces are not included (see <see cref="ExplicitInterfaceType"/>).
    /// </summary>
    IEnumerable<ITypeNameData> ImplementedInterfaces { get; }
}
