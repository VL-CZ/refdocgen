using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class CyclicDocTests
{
    [Theory]
    [InlineData("Cycle1")]
    [InlineData("Cycle2")]
    [InlineData("CycleReference")]
    public void Test_Cycle1(string typeName)
    {
        using var document = Tools.GetDocument($"MyLibrary.CyclicDoc.{typeName}.html");

        var typeDocsSection = document.GetTypeDocsSection();
        typeDocsSection.GetByDataIdOrDefault(Tools.SummaryDoc).ShouldBeNull();
    }
}
