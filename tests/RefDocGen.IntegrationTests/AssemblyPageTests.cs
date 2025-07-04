using AngleSharp.Dom;
using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the 'assembly' page.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class AssemblyPageTests : IDisposable
{
    /// <summary>
    /// The HTML page representing the <c>RefDocGen.ExampleLibrary</c> assembly.
    /// </summary>
    private readonly IDocument document;

    public AssemblyPageTests()
    {
        document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary-DLL.html");
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
            "namespace RefDocGen.ExampleLibrary",
            "namespace RefDocGen.ExampleLibrary.CyclicDoc",
            "namespace RefDocGen.ExampleLibrary.Hierarchy",
            "namespace RefDocGen.ExampleLibrary.Tools",
            "namespace RefDocGen.ExampleLibrary.Tools.Collections",
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
}
