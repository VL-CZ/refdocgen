namespace RefDocGen.Tools;

/// <summary>
/// Class containing extension methods related to dictionaries.
/// </summary>
internal static class DictionaryExtensions
{
    /// <summary>
    /// Merges two dictionaries into a new dictionary containing key-value pairs from both dictionaries.
    /// </summary>
    /// <remarks>
    /// If a key exists in both <paramref name="dictionary"/> and <paramref name="other"/>, 
    /// the value from <paramref name="dictionary"/> is used in the resulting dictionary.
    /// </remarks>
    /// <typeparam name="TKey">The type of the dictionary keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary values.</typeparam>
    /// <param name="dictionary">The primary dictionary, tts key-value pairs take precedence.</param>
    /// <param name="other">The dictionary to merge with.</param>
    /// <returns>
    /// A new dictionary containing key-value pairs from both dictionaries.
    /// </returns>
    internal static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, IReadOnlyDictionary<TKey, TValue> other)
        where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(dictionary);

        foreach ((var key, var value) in other)
        {
            _ = result.TryAdd(key, value); // if the key is already present, don't add anything
        }

        return result;
    }
}