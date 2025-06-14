namespace RefDocGen.ExampleLibrary.Tools.Collections;

/// <summary>
/// Interface representing a custom dictionary.
/// </summary>
/// <typeparam name="TKey">Type of the key.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
internal interface IMyDictionary<TKey, TValue>
{

    /// <summary>
    /// Add a key-value pair into the dictionary.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void Add(TKey key, TValue value);

    /// <summary>
    /// Get the value by its key.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <returns>Value with the given key.</returns>
    TValue Get(TKey key);
}
