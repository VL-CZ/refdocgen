namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing constants representing the member identifiers in the XML documentation file.
/// <para>
/// See also <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
/// </para>
/// </summary>
internal class MemberTypeId
{
    /// <summary>
    /// Field identifier.
    /// </summary>
    internal const string Field = "F";

    /// <summary>
    /// Property identifer.
    /// </summary>
    internal const string Property = "P";

    /// <summary>
    /// Method identifer.
    /// </summary>
    internal const string Method = "M";

    /// <summary>
    /// Event identifier.
    /// </summary>
    internal const string Event = "E";

    /// <summary>
    /// Type identifier.
    /// </summary>
    internal const string Type = "T";

    /// <summary>
    /// Namespace identifier.
    /// </summary>
    internal const string Namespace = "N";
}

