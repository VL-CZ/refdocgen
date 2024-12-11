namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of an executable type member, i.e. method, constructor or an indexer.
/// <para>
/// Note that properties are excluded from this definition.
/// </para>
/// </summary>
public interface IExecutableMemberData : ICallableMemberData
{
    /// <summary>
    /// Readonly list of the member parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
