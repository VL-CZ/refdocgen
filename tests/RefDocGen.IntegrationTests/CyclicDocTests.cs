using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class CyclicDocTests
{
    [Theory]
    [InlineData("Cycle1")]
    [InlineData("Cycle2")]
    [InlineData("CycleReference")]
    public void Test_Cyclic_InheritDoc(string typeName)
    {
        using var document = TestTools.GetDocumentationPage($"MyLibrary.CyclicDoc.{typeName}.html");

        var typeDataSection = document.GetTypeDataSection();
        typeDataSection.GetByDataIdOrDefault(DataId.SummaryDoc).ShouldBeNull(); // no summary doc should be present.
    }
}
