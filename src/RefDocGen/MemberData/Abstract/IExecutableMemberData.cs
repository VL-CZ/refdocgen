namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of an executable type member, i.e. method or a constructor.
/// Note that properties are excluded from this definition.
/// </summary>
public interface IExecutableMemberData : ICallableMemberData
{
    /// <summary>
    /// Readonly list of method parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
