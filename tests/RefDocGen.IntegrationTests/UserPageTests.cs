using AngleSharp.Dom;
using Shouldly;

namespace RefDocGen.IntegrationTests;

public class UserPageTests : IClassFixture<DocumentationFixture>
{
    private readonly IDocument document;

    public UserPageTests()
    {
        document = Shared.GetDocument("MyLibrary.User.html");
    }

    [Fact]
    public void IsAdultMethod_ContainsCorrectContent()
    {
        // Access elements using query selectors
        var isAdultMethod = document.GetMember("IsAdult");

        var methodNameElement = isAdultMethod.GetDataId(Shared.MemberName);

        string methodName = Shared.GetMemberNameContent(methodNameElement);
        methodName.ShouldBe("public bool IsAdult()");

        var summaryDocElement = isAdultMethod.GetDataId(Shared.SummaryDoc);
        string summaryDoc = Shared.ParseStringContent(summaryDocElement);

        summaryDoc.ShouldBe("Checks if the user is adult.");
    }

    [Fact]
    public void MaxAge_ContainsCorrectContent()
    {
        // Access elements using query selectors
        var maxAgeElement = document.GetMember("MaxAge");

        var fieldNameElement = maxAgeElement.GetDataId(Shared.MemberName);

        string fieldName = Shared.GetMemberNameContent(fieldNameElement);
        fieldName.ShouldBe("private const int MaxAge = 150");

        var summaryDocElement = maxAgeElement.GetDataId(Shared.SummaryDoc);
        string summaryDoc = Shared.ParseStringContent(summaryDocElement);

        summaryDoc.ShouldBe("Maximum age of the user.");
    }
}
