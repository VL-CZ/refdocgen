namespace RefDocGen.TestingLibrary.Tools.Collections;

/// <summary>
/// My collection interface.
/// </summary>
/// <typeparam name="T"></typeparam>
internal interface IMyCollection<T> : ICollection<T>
{
    /// <summary>
    /// Execute the given operation on each element.
    /// </summary>
    /// <param name="operation">An operation to execute.</param>
    void Execute(Func<T, T> operation);

    /// <summary>
    /// Add range of items into the collection.
    /// </summary>
    /// <param name="range">Range of items to add.</param>
    public void AddRange(IEnumerable<T> range);

    /// <summary>
    /// Checks if an item can be added into the collection.
    /// </summary>
    public bool CanAdd();
}
