namespace RefDocGen.ExampleLibrary.Tools;

/// <summary>
/// Extension class for <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Zips the two provided strings.
    /// </summary>
    /// <param name="s1">The first string.</param>
    /// <param name="s2">The second string.</param>
    /// <returns>A zipped string.</returns>
    /// <seealso cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
    public static string ZipWith(this string s1, string s2)
    {
        return s1 + s2;
    }
}
