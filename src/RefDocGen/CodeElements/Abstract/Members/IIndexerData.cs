namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of an indexer.
/// </summary>
public interface IIndexerData : IPropertyData
{
    /// <summary>
    /// Readonly list of index parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
