namespace RefDocGen.Tools.Xml;

/// <summary>
/// Class containing helper methods related to XPath
/// </summary>
internal class XPathTools
{
    /// <summary>
    /// Makes the XPath expression relative.
    /// </summary>
    /// <param name="xpath">The provided XPath expression.</param>
    /// <returns>Relative XPath expression.</returns>
    internal static string MakeRelative(string xpath)
    {
        return xpath.StartsWith('/')
            ? '.' + xpath
            : xpath;
    }
}
