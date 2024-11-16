namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a callable type member (such as a method, property or a constructor).
/// </summary>
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
}
