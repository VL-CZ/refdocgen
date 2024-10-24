namespace RefDocGen.DocExtraction.Tools;

/// <summary>
/// Class containing methods for extracting data from full member names.
/// </summary>
internal class MemberStringExtractor
{
    /// <summary>
    /// Extracts the type name, member name and its parameters from a fully qualified member name.
    /// </summary>
    /// <param name="fullMemberName">Fully qualified name of the member, including the namespace and type.</param>
    /// <returns>Tuple containing
    /// <list type="bullet">
    ///     <item>Type name</item>
    ///     <item>Member name (without parameters).</item>
    ///     <item>
    ///     String containing comma-delimited member parameters (e.g. '(System.String,System.Double)' ).
    ///     Empty string if the member has no parameters.
    ///     </item>
    /// </list>
    /// </returns>
    internal static (string typeName, string memberName, string paramsString) SplitFullMemberName(string fullMemberName)
    {
        string[] splitValues = fullMemberName.Split('(');
        string memberNameWithoutParameters = splitValues[0]; // this is done because of methods

        // TODO: method overloading, we need to distinguish between the overloaded methods

        string[] nameParts = memberNameWithoutParameters.Split('.');
        string memberName = nameParts[^1];
        string typeName = string.Join('.', nameParts, 0, nameParts.Length - 1);

        if (splitValues.Length > 1)
        {
            string paramsStrings = '(' + string.Join("", splitValues.Skip(1));
            return (typeName, memberName, paramsStrings);
        }
        else
        {
            return (typeName, memberName, string.Empty);
        }
    }
}
