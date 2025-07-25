using AngleSharp.Dom;
using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the API homepage.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class ApiHomePageTests : IDisposable
{
    /// <summary>
    /// The HTML page representing the API homepage.
    /// </summary>
    private readonly IDocument document;

    public ApiHomePageTests()
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
        string[] namespaces = AssemblyPageTools.GetNamespaceNames(document);

        string[] expected = [
            "namespace RefDocGen.ExampleFSharpLibrary",
            "namespace RefDocGen.ExampleLibrary",
            "namespace RefDocGen.ExampleLibrary.CyclicDoc",
            "namespace RefDocGen.ExampleLibrary.Hierarchy",
            "namespace RefDocGen.ExampleLibrary.Tools",
            "namespace RefDocGen.ExampleLibrary.Tools.Collections",
            "namespace RefDocGen.ExampleVbLibrary",
        ];

        namespaces.ShouldBe(expected);
    }

    [Fact]
    public void NamespaceTypeNames_Match()
    {
        var ns = document.GetNamespaceElement("RefDocGen.ExampleLibrary.Tools");

        string[] nsTypes = AssemblyPageTools.GetNamespaceTypeNames(ns);

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

    [Fact]
    public void AssemblyNames_Match()
    {
        string[] assemblies = AssemblyPageTools.GetAssemblyNames(document);

        string[] expected = [
            "assembly RefDocGen.ExampleFSharpLibrary",
            "assembly RefDocGen.ExampleLibrary",
            "assembly RefDocGen.ExampleVbLibrary"
        ];

        assemblies.ShouldBe(expected);
    }

    [Fact]
    public void Title_Matches()
    {
        string title = AssemblyPageTools.GetApiHomepageTitle(document);

        title.ShouldBe("RefDocGen API");
    }
}
