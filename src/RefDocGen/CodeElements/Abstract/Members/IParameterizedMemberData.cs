namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a type member that may have parameters (such as a method, constructor or indexer).
/// </summary>
/// <remarks>
/// Note that properties are excluded from this definition.
/// </remarks>
public interface IParameterizedMemberData : IMemberData
{
    /// <summary>
    /// Readonly list of the member parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
