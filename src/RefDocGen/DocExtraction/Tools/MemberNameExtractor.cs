namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing methods for extracting member names.
/// </summary>
internal class MemberNameExtractor
{
    /// <summary>
    /// Extracts the type name and member name from a fully qualified member name.
    /// </summary>
    /// <param name="fullMemberName">Fully qualified name of the member, including the namespace and type.</param>
    /// <returns>Tuple containing type and member name.</returns>
    internal static (string typeName, string memberName) GetTypeAndMemberName(string fullMemberName)
    {
        string memberNameWithoutParameters = fullMemberName.Split('(')[0]; // this is done because of methods

        // TODO: method overloading, we need to distinguish between the overloaded methods

        string[] nameParts = memberNameWithoutParameters.Split('.');
        string memberName = nameParts[^1];
        string typeName = string.Join('.', nameParts, 0, nameParts.Length - 1);

        return (typeName, memberName);
    }
}
