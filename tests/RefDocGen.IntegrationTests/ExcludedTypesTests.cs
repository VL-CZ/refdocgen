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
    [InlineData("MyLibrary.Exclude.ClassToExclude")]
    [InlineData("MyLibrary.Exclude.Sub.AnotherClassToExclude")]
    [InlineData("MyLibrary.Tools.Exclude.ToolToExclude")]
    public void Check_That_The_Type_Is_Excluded(string excludedTypeName)
    {
        Should.Throw<FileNotFoundException>(() => DocumentationTools.GetApiPage($"{excludedTypeName}.html"));
    }
}
