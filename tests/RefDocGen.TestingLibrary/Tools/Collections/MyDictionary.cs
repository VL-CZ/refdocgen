namespace RefDocGen.TestingLibrary.Tools.Collections;

/// <summary>
/// A simple custom dictionary.
/// </summary>
/// <typeparam name="TKey">Type of the key.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
internal class MyDictionary<TKey, TValue> : IMyDictionary<TKey, TValue>
    where TKey : class, IComparable, IEquatable<TKey>, new()
    where TValue : struct, IDisposable
{

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
    }

    /// <inheritdoc/>
    public TValue Get(TKey key)
    {
        throw new NotImplementedException();
    }
}
