using AngleSharp.Dom;
using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for 'namespace list' page.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class NamespaceListPageTests : IDisposable
{
    /// <summary>
    /// The HTML page representing the namespace list.
    /// </summary>
    private readonly IDocument document;

    public NamespaceListPageTests()
    {
        document = DocumentationTools.GetApiPage("index.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void NamespaceNames_Match()
    {
        string[] classes = NamespacePageTools.GetNamespaceNames(document);

        string[] expected = [
            "namespace MyLibrary",
            "namespace MyLibrary.CyclicDoc",
            "namespace MyLibrary.Hierarchy",
            "namespace MyLibrary.Tools",
            "namespace MyLibrary.Tools.Collections"
        ];

        classes.ShouldBe(expected);
    }

    [Fact]
    public void NamespaceTypeNames_Match()
    {
        var ns = document.GetNamespaceElement("MyLibrary.Tools");

        string[] nsTypes = NamespacePageTools.GetNamespaceTypeNames(ns);

        string[] expected = [
            "enum HarvestingSeason",
            "interface IContravariant<T>",
            "interface ICovariant<T>",
            "delegate MyPredicate<T>",
            "delegate ObjectPredicate",
            "struct Point",
            "enum Season",
            "class StringExtensions",
            "class WeatherStation"
        ];

        nsTypes.ShouldBe(expected);
    }

}
