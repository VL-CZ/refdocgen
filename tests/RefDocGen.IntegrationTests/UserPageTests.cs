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
    public void Test_ClassName()
    {
        string className = Tools.GetTypeName(document);
        className.ShouldBe("public class User");
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

    [Theory]
    [InlineData("FirstName", "public string FirstName { get; internal set; }", "First name of the user.")]
    [InlineData("LastName", "public string LastName { get; init; }", "Last name of the user.")]
    [InlineData("Animals", "public List<Animal> Animals { get; }", "List of owned animals.")]

    public void Test_Properties(string propertyName, string expectedSignature, string expectedSummaryDoc)
    {
        var propertyElement = document.GetMember(propertyName);

        string propertySignature = Tools.GetMemberNameContent(propertyElement);
        propertySignature.ShouldBe(expectedSignature);

        string summaryDoc = Tools.GetSummaryDocContent(propertyElement);
        summaryDoc.ShouldBe(expectedSummaryDoc);
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

        parameters.Length.ShouldBe(5);

        List<(string signature, string? doc)> expectedValues = [
            ("in int inValue", "An input value."),
            ("ref int refValue", "A reference value."),
            ("string s1", null),
            ("out int outValue", "An output value."),
            ("double d2", null)
        ];

        for (int i = 0; i < expectedValues.Count; i++)
        {
            string paramName = Tools.GetParameterName(parameters[i]);
            paramName.ShouldBe(expectedValues[i].signature);

            string paramDoc = Tools.GetParameterDoc(parameters[i]);
            paramDoc.ShouldBe(expectedValues[i].doc);
        }
    }
}
