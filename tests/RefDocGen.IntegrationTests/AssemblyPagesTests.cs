using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for both the assembly page and the API Homepage.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class AssemblyPagesTests
{
    [Theory]
    [InlineData("index")]
    [InlineData("RefDocGen.ExampleLibrary-DLL")]
    public void NamespaceNames_Match(string pageName)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");
        string[] namespaces = AssemblyPageTools.GetNamespaceNames(document);

        string[] expected = [
            "namespace RefDocGen.ExampleLibrary",
            "namespace RefDocGen.ExampleLibrary.CyclicDoc",
            "namespace RefDocGen.ExampleLibrary.Hierarchy",
            "namespace RefDocGen.ExampleLibrary.Tools",
            "namespace RefDocGen.ExampleLibrary.Tools.Collections"
        ];

        namespaces.ShouldBe(expected);
    }

    [Theory]
    [InlineData("index")]
    [InlineData("RefDocGen.ExampleLibrary-DLL")]
    public void NamespaceTypeNames_Match(string pageName)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");
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
        using var document = DocumentationTools.GetApiPage("index.html"); // use the API Homepage
        string[] assemblies = AssemblyPageTools.GetAssemblyNames(document);

        string[] expected = ["assembly RefDocGen.ExampleLibrary"];

        assemblies.ShouldBe(expected);
    }
}
