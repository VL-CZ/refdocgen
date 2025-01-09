namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing constants representing the code element identifiers in the XML documentation file.
/// <para>
/// See also <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
/// </para>
/// </summary>
internal class CodeElementId
{
    /// <summary>
    /// Field identifier.
    /// </summary>
    /// <remarks>
    /// Note that this identifier is also used for enum members.
    /// </remarks>
    internal const string Field = "F";

    /// <summary>
    /// Property identifer.
    /// </summary>
    /// <remarks>
    /// Note that this identifier is also used for indexers.
    /// </remarks>
    internal const string Property = "P";

    /// <summary>
    /// Method identifer.
    /// </summary>
    /// <remarks>
    /// Note that this identifier is also used for constructors and operators.
    /// </remarks>
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

    /// <summary>
    /// Checks if the identifier represents a type member.
    /// </summary>
    /// <param name="id">The provided identifier.</param>
    /// <returns><c>true</c> if identifier represents a type member, <c>false</c> otherwise.</returns>
    internal static bool IsMember(string id)
    {
        return id is Field or Property or Method or Event;
    }
}

