using AngleSharp.Dom;
using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class NamespaceListPageTests : IDisposable
{
    private readonly IDocument document;

    public NamespaceListPageTests()
    {
        document = DocumentationTools.GetPage("index.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void TestNamespaceNames()
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
    public void TestNamespaceTypes()
    {
        var ns = document.GetElementById("MyLibrary.Tools");

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
