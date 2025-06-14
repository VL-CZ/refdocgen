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
    [InlineData("RefDocGen.TestingLibrary-DLL")]
    public void NamespaceNames_Match(string pageName)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");
        string[] namespaces = AssemblyPageTools.GetNamespaceNames(document);

        string[] expected = [
            "namespace RefDocGen.TestingLibrary",
            "namespace RefDocGen.TestingLibrary.CyclicDoc",
            "namespace RefDocGen.TestingLibrary.Hierarchy",
            "namespace RefDocGen.TestingLibrary.Tools",
            "namespace RefDocGen.TestingLibrary.Tools.Collections"
        ];

        namespaces.ShouldBe(expected);
    }

    [Theory]
    [InlineData("index")]
    [InlineData("RefDocGen.TestingLibrary-DLL")]
    public void NamespaceTypeNames_Match(string pageName)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");
        var ns = document.GetNamespaceElement("RefDocGen.TestingLibrary.Tools");

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

        string[] expected = ["assembly RefDocGen.TestingLibrary"];

        assemblies.ShouldBe(expected);
    }
}
