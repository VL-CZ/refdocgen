using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(MyTestCollection.Name)]
public class HierarchyDocTests
{
    [Theory]
    [InlineData("Child", "Parent class.")]
    [InlineData("ChildChild", "IChild Print the object.")]
    [InlineData("ChildChildChild", "Before parent. Parent class. After parent.")]
    public void Test_ClassName(string typeName, string expectedSummaryDoc)
    {
        using var document = Tools.GetDocument($"MyLibrary.Hierarchy.{typeName}.html");

        var typeDocsSection = document.GetTypeDocsSection();
        var summaryDoc = Tools.GetSummaryDocContent(typeDocsSection);

        summaryDoc.ShouldBe(expectedSummaryDoc);
    }

    [Theory]
    [InlineData("Child")]
    [InlineData("ChildChild")]
    public void Test_Handle_Method(string typeName)
    {
        using var document = Tools.GetDocument($"MyLibrary.Hierarchy.{typeName}.html");

        var handleMethod = document.GetMember("Handle(System.Object)");
        var parameter = Tools.GetMemberParameters(handleMethod).First();

        var summaryDoc = Tools.GetSummaryDocContent(handleMethod);
        var returnsDoc = Tools.GetReturnsDoc(handleMethod);
        var paramDoc = Tools.GetParameterDoc(parameter);

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
        using var document = Tools.GetDocument($"MyLibrary.Hierarchy.{typeName}.html");

        var handleMethod = document.GetMember("Print(System.Object)");
        var parameter = Tools.GetMemberParameters(handleMethod).First();

        var summaryDoc = Tools.GetSummaryDocContent(handleMethod);
        var paramDoc = Tools.GetParameterDoc(parameter);

        summaryDoc.ShouldBe(expectedSummaryDoc);
        paramDoc.ShouldBe(expectedParamDoc);

        if (expectedRemarksDoc is not null)
        {
            var remarksDoc = Tools.GetRemarksDocContent(handleMethod);
            remarksDoc.ShouldBe(expectedRemarksDoc);
        }
    }

    [Fact]
    public void Test_PrintDataMethod()
    {
        using var document = Tools.GetDocument("MyLibrary.Hierarchy.ChildChild.html");

        var handleMethod = document.GetMember("PrintData");

        var summaryDoc = Tools.GetSummaryDocContent(handleMethod);

        summaryDoc.ShouldBe("IChild Print the object.");
    }
}
