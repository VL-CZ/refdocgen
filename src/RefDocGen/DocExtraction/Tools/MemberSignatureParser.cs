namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing methods for parsing type member signatures, contained in the XML documentation file.
/// </summary>
internal class MemberSignatureParser
{
    /// <summary>
    /// Parses the fully qualified member name and splits it into the type name, member name and its parameters.
    /// </summary>
    /// <param name="fullMemberName">Fully qualified name of the member, including the namespace and type.</param>
    /// <returns>Tuple containing
    /// <list type="bullet">
    ///     <item>Type name</item>
    ///     <item>Member name (without parameters).</item>
    ///     <item>
    ///     String containing comma-delimited member parameters wrapped in parentheses, e.g. '(System.String,System.Double)'.
    ///     Empty string if the member has no parameters.
    ///     </item>
    /// </list>
    /// </returns>
    internal static (string typeName, string memberName, string paramsString) Parse(string fullMemberName)
    {
        string[] splitValues = fullMemberName.Split('(');
        string memberNameWithoutParameters = splitValues[0]; // this is done because of methods

        string[] nameParts = memberNameWithoutParameters.Split('.');
        string memberName = nameParts[^1];
        string typeName = string.Join('.', nameParts, 0, nameParts.Length - 1);

        if (splitValues.Length > 1) // we need to add params
        {
            string paramsStrings = '(' + string.Join("", splitValues.Skip(1));
            return (typeName, memberName, paramsStrings);
        }
        else // no params
        {
            return (typeName, memberName, string.Empty);
        }
    }
}
