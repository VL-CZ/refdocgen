using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class CyclicDocTests
{
    [Theory]
    [InlineData("Cycle1")]
    [InlineData("Cycle2")]
    [InlineData("CycleReference")]
    public void Test_Cycle(string typeName)
    {
        using var document = Tools.GetDocument($"MyLibrary.CyclicDoc.{typeName}.html");

        var typeDocsSection = document.GetTypeDataSection();
        typeDocsSection.GetByDataIdOrDefault(DataId.SummaryDoc).ShouldBeNull();
    }
}
