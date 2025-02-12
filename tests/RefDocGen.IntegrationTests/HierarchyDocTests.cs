using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class HierarchyDocTests
{
    [Theory]
    [InlineData("Child", "Parent class.")]
    [InlineData("ChildChild", "IChild Print the object.")]
    [InlineData("ChildChildChild", "Before parent. Parent class. After parent.")]
    public void Test_ClassName(string typeName, string expectedSummaryDoc)
    {
        using var document = DocumentationTools.GetPage($"MyLibrary.Hierarchy.{typeName}.html");

        var typeDataSection = document.GetTypeDataSection();
        string summaryDoc = TypePageTools.GetSummaryDoc(typeDataSection);

        summaryDoc.ShouldBe(expectedSummaryDoc);
    }

    [Theory]
    [InlineData("Child")]
    [InlineData("ChildChild")]
    public void Test_Handle_Method(string typeName)
    {
        using var document = DocumentationTools.GetPage($"MyLibrary.Hierarchy.{typeName}.html");

        var handleMethod = document.GetMemberElement("Handle(System.Object)");
        var parameter = TypePageTools.GetMemberParameters(handleMethod).First();

        string summaryDoc = TypePageTools.GetSummaryDoc(handleMethod);
        string returnsDoc = TypePageTools.GetReturnsDoc(handleMethod);
        string paramDoc = TypePageTools.GetParameterDoc(parameter);

        summaryDoc.ShouldBe("Handle the object.");
        paramDoc.ShouldBe("The object to handle.");
        returnsDoc.ShouldBe("Boolean indicating whether the handling was successful.");
    }

    [Theory]
    [InlineData("Child", "IChild Print the object.", "Object to print.")]
    [InlineData("ChildChild", "ChildChild Print", "Object to print.", "IChild Print the object. Object to print.")]
    [InlineData("ChildChildChild", "ChildChild Print", "Object to print.", "IChild Print the object. Object to print.")]
    public void Test_Print_Method(string typeName, string expectedSummaryDoc, string expectedParamDoc, string? expectedRemarksDoc = null)
    {
        using var document = DocumentationTools.GetPage($"MyLibrary.Hierarchy.{typeName}.html");

        var handleMethod = document.GetMemberElement("Print(System.Object)");
        var parameter = TypePageTools.GetMemberParameters(handleMethod).First();

        string summaryDoc = TypePageTools.GetSummaryDoc(handleMethod);
        string paramDoc = TypePageTools.GetParameterDoc(parameter);

        summaryDoc.ShouldBe(expectedSummaryDoc);
        paramDoc.ShouldBe(expectedParamDoc);

        if (expectedRemarksDoc is not null)
        {
            string remarksDoc = TypePageTools.GetRemarksDoc(handleMethod);
            remarksDoc.ShouldBe(expectedRemarksDoc);
        }
    }

    [Fact]
    public void Test_PrintDataMethod()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Hierarchy.ChildChild.html");

        var handleMethod = document.GetMemberElement("PrintData");

        string summaryDoc = TypePageTools.GetSummaryDoc(handleMethod);

        summaryDoc.ShouldBe("IChild Print the object.");
    }
}
