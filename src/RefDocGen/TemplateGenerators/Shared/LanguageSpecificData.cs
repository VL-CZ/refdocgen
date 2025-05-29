using System.Text.Json.Serialization;

namespace RefDocGen.TemplateGenerators.Shared;

/// <summary>
/// Represents data stored in multiple programming languages (e.g., C#, VB.NET, F#).
/// </summary>
/// <typeparam name="T">The type of the data associated with each language.</typeparam>
public class LanguageSpecificData<T>
{
    /// <summary>
    /// A dictionary storing the data, indexed by language identifiers.
    /// </summary>
    [JsonInclude]
    internal readonly Dictionary<string, T> data = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageSpecificData{T}"/> class
    /// with the specified language-data mapping.
    /// </summary>
    /// <param name="data">The dictionary containing language-specific data, indexed by language identifiers.</param>
    public LanguageSpecificData(Dictionary<string, T> data)
    {
        this.data = data;
    }

    /// <summary>
    /// Gets the data associated with the given language.
    /// </summary>
    /// <param name="language">The language identifier (equal to the <see cref="ILanguageSpecificData.LanguageId"/> value).</param>
    /// <returns>The data associated with the given <paramref name="language"/>.</returns>
    public T this[string language] => data[language];

    /// <summary>
    /// Gets the data associated with C# language.
    /// </summary>
    [JsonIgnore]
    public T CSharpData => data[CSharpLanguageData.languageId];
}
