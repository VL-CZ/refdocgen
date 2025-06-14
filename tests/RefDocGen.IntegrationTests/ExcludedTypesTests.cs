using RefDocGen.IntegrationTests.Fixtures;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class tests that the types in namespaces marked as excluded are excluded from the documentation.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class ExcludedTypesTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Exclude.ClassToExclude")]
    [InlineData("RefDocGen.TestingLibrary.Exclude.Sub.AnotherClassToExclude")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Exclude.ToolToExclude")]
    public void Type_IsNotPresent_WhenContainedInExcludedNamespace(string excludedTypeName)
    {
        Should.Throw<FileNotFoundException>(() => DocumentationTools.GetApiPage($"{excludedTypeName}.html"));
    }
}
