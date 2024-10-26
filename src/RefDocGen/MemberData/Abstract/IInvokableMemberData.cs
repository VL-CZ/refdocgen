namespace RefDocGen.MemberData.Abstract;

public interface IInvokableMemberData : ICallableMemberData
{
    /// <summary>
    /// Readonly list of method parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
