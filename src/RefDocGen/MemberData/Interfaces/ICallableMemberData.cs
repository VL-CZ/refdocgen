namespace RefDocGen.MemberData.Interfaces;

/// <summary>
/// Represents data of a callable type member (such as a method or property)
/// </summary>
public interface ICallableMemberData : IMemberData
{
    /// <summary>
    /// Checks whether the member is static.
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Checks whether the member can be overridden.
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
    /// </summary>
    bool IsFinal { get; }

    /// <summary>
    /// Checks whether the member is sealed.
    /// </summary>
    bool IsSealed { get; }

    /// <summary>
    /// Checks whether the member is asynchronous.
    /// </summary>
    bool IsAsync { get; }
}
