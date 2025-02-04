using AngleSharp.Dom;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(MyTestCollection.Name)]
public class UserPageTests : IDisposable
{
    private readonly IDocument document;

    public UserPageTests()
    {
        document = Tools.GetDocument("MyLibrary.User.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void Test_MaxAgeField()
    {
        var maxAgeField = document.GetMember("MaxAge");

        string fieldName = Tools.GetMemberNameContent(maxAgeField);
        fieldName.ShouldBe("private const int MaxAge = 150");

        string summaryDoc = Tools.GetSummaryDocContent(maxAgeField);
        summaryDoc.ShouldBe("Maximum age of the user.");
    }

    [Fact]
    public void Test_AnimalsProperty()
    {
        var animalsProperty = document.GetMember("Animals");

        string fieldName = Tools.GetMemberNameContent(animalsProperty);
        fieldName.ShouldBe("public List<Animal> Animals { get; }");

        string summaryDoc = Tools.GetSummaryDocContent(animalsProperty);
        summaryDoc.ShouldBe("List of owned animals.");
    }

    [Fact]
    public void Test_IsAdultMethod()
    {
        var isAdultMethod = document.GetMember("IsAdult");

        string methodName = Tools.GetMemberNameContent(isAdultMethod);
        methodName.ShouldBe("public bool IsAdult()");

        string summaryDoc = Tools.GetSummaryDocContent(isAdultMethod);
        summaryDoc.ShouldBe("Checks if the user is adult.");

        string returnTypeName = Tools.GetReturnTypeName(isAdultMethod);
        returnTypeName.ShouldBe("bool");

        string returnsDoc = Tools.GetReturnsDoc(isAdultMethod);
        returnsDoc.ShouldBe("True if adult, false otherwise.");
    }

    [Fact]
    public void Test_ProcessValuesMethod()
    {
        var memberElement = document.GetMember("ProcessValues(System.Int32@,System.Int32@,System.String,System.Int32@,System.Double)");

        string fieldName = Tools.GetMemberNameContent(memberElement);
        fieldName.ShouldBe("internal static void ProcessValues(in int inValue, ref int refValue, string s1, out int outValue, double d2 = 150)");

        string summaryDoc = Tools.GetSummaryDocContent(memberElement);
        summaryDoc.ShouldBe("A method with ref, in, and out parameters for testing purposes.");

        var parameters = Tools.GetMemberParameters(memberElement);

        string firstParamName = Tools.GetParameterName(parameters[0]);
        firstParamName.ShouldBe("in int inValue");

        string firstParamDoc = Tools.GetParameterDoc(parameters[0]);
        firstParamDoc.ShouldBe("An input value.");
    }
}
