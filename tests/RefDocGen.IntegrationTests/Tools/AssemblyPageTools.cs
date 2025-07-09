using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// A class that contains additional tool methods related to the <c>assembly</c> page and API Homepage.
/// </summary>
internal class AssemblyPageTools
{
    /// <summary>
    /// Gets the name of the type row.
    /// </summary>
    /// <param name="element">The HTML element representing the type row.</param>
    /// <returns>The type row name.</returns>
    internal static string GetTypeRowName(IElement element)
    {
        return element.GetParsedContent(DataId.TypeRowName);
    }

    /// <summary>
    /// Retrieves the names of the namespaces from the given document.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of strings, each representing a namespace name.</returns>
    internal static string[] GetNamespaceNames(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataIds(DataId.NamespaceName).Select(e => e.GetParsedContent())];
    }

    /// <summary>
    /// Retrieves the names of the assemblies from the given document.
    /// </summary>
    /// <param name="document">The HTML page representing the assembly or API homepage.</param>
    /// <returns>An array of strings, each representing an assembly name.</returns>
    internal static string[] GetAssemblyNames(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataIds(DataId.AssemblyName).Select(e => e.GetParsedContent())];
    }

    /// <summary>
    /// Retrieves the names of the types defined within a given element.
    /// </summary>
    /// <param name="element">The HTML element containing the type rows.</param>
    /// <returns>An array of strings, each representing a type name within the element.</returns>
    internal static string[] GetNamespaceTypeNames(IElement element)
    {
        return [.. element.GetByDataIds(DataId.TypeRowElement).Select(GetTypeRowName)];
    }

    /// <summary>
    /// Gets the API homepage title.
    /// </summary>
    /// <param name="document">The API homepage.</param>
    /// <returns>The title of API homepage.</returns>
    internal static string GetApiHomepageTitle(IDocument document)
    {
        return document.DocumentElement.GetParsedContent(DataId.ApiHomepageTitle);
    }
}
