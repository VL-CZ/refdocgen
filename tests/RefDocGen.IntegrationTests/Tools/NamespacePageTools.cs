using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// A class that contains additional tool methods related to all <c>namespace</c> pages.
/// </summary>
internal class NamespacePageTools
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
    /// Retrieves the classes defined within a given namespace.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of HTML elements, each representing a class within the namespace.</returns>
    internal static IElement[] GetNamespaceClasses(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceClasses).GetByDataIds(DataId.TypeRowElement)];
    }

    /// <summary>
    /// Retrieves the interfaces defined within a given namespace.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of HTML elements, each representing an interface within the namespace.</returns>
    internal static IElement[] GetNamespaceInterfaces(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceInterfaces).GetByDataIds(DataId.TypeRowElement)];
    }

    /// <summary>
    /// Retrieves the delegates defined within a given namespace.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of HTML elements, each representing a delegate within the namespace.</returns>
    internal static IElement[] GetNamespaceDelegates(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceDelegates).GetByDataIds(DataId.TypeRowElement)];
    }

    /// <summary>
    /// Retrieves the enums defined within a given namespace.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of HTML elements, each representing an enum within the namespace.</returns>
    internal static IElement[] GetNamespaceEnums(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceEnums).GetByDataIds(DataId.TypeRowElement)];
    }

    /// <summary>
    /// Retrieves the structs defined within a given namespace.
    /// </summary>
    /// <param name="document">The HTML page representing the namespace detail.</param>
    /// <returns>An array of HTML elements, each representing a struct within the namespace.</returns>
    internal static IElement[] GetNamespaceStructs(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceStructs).GetByDataIds(DataId.TypeRowElement)];
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
    /// Retrieves the names of the types defined within a given element.
    /// </summary>
    /// <param name="element">The HTML element containing the type rows.</param>
    /// <returns>An array of strings, each representing a type name within the element.</returns>
    internal static string[] GetNamespaceTypeNames(IElement element)
    {
        return [.. element.GetByDataIds(DataId.TypeRowElement).Select(GetTypeRowName)];
    }

}
