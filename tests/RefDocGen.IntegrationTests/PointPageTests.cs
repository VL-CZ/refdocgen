using AngleSharp.Dom;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(MyTestCollection.Name)]
public class PointPageTests : IDisposable
{
    private readonly IDocument document;

    public PointPageTests()
    {
        document = Tools.GetDocument("MyLibrary.Tools.Point.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void Test_X_Field()
    {
        var maxAgeField = document.GetMember("X");

        string fieldName = Tools.GetMemberNameContent(maxAgeField);
        fieldName.ShouldBe("internal readonly double X");

        string summaryDoc = Tools.GetSummaryDocContent(maxAgeField);
        summaryDoc.ShouldBe("X coordinate of the point.");
    }

    [Fact]
    public void Test_ProcessValuesMethod()
    {
        var memberElement = document.GetMember("op_Equality(MyLibrary.Tools.Point,MyLibrary.Tools.Point)");

        string fieldName = Tools.GetMemberNameContent(memberElement);
        fieldName.ShouldBe("public static bool operator ==(Point point, Point other)");

        string summaryDoc = Tools.GetSummaryDocContent(memberElement);
        summaryDoc.ShouldBe("Compare the 2 points.");

        var parameters = Tools.GetMemberParameters(memberElement);

        parameters.Length.ShouldBe(2);

        string param1Name = Tools.GetParameterName(parameters[0]);
        param1Name.ShouldBe("Point point");

        string param1Doc = Tools.GetParameterDoc(parameters[0]);
        param1Doc.ShouldBe("1st point.");

        string param2Name = Tools.GetParameterName(parameters[1]);
        param2Name.ShouldBe("Point other");

        string param2Doc = Tools.GetParameterDoc(parameters[1]);
        param2Doc.ShouldBe("2nd point.");

        string returnTypeName = Tools.GetReturnTypeName(memberElement);
        returnTypeName.ShouldBe("bool");

        string returnsDoc = Tools.GetReturnsDoc(memberElement);
        returnsDoc.ShouldBe("Are the 2 points equal?");
    }
}
