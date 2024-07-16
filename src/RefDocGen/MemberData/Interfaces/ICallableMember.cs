namespace RefDocGen.MemberData.Interfaces;

public interface ICallableMember
{
    bool IsStatic { get; }
    bool IsOverridable { get; }
    bool OverridesAnotherMember { get; }
    bool IsAbstract { get; }
    bool IsFinal { get; }
    bool IsSealed { get; }
    bool IsAsync { get; }
}
