using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests.Tests.TypePage;

/// <summary>
/// This class tests that cyclic inheritdocs are resolved as null.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class CyclicDocTests
{
    [Theory]
    [InlineData("Cycle1")]
    [InlineData("Cycle2")]
    [InlineData("CycleReference")]
    public void NoSummaryDoc_IsPresent_WhenCrefInheritdocsFormCycle(string typeName)
    {
        using var document = DocumentationTools.GetApiPage($"RefDocGen.ExampleLibrary.CyclicDoc.{typeName}.html");

        var typeDataSection = document.GetTypeDataSection();
        typeDataSection.GetByDataIdOrDefault(DataId.SummaryDoc).ShouldBeNull(); // no summary doc should be present.
    }
}
