using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class MemberDataTests
{
    [Fact]
    public void Test_Attributes()
    {
        using var document = TestTools.GetDocumentationPage("MyLibrary.User.html");

        string[] attributes = TestTools.GetAttributes(document.GetMemberElement("PrintProfile(System.String)"));

        string[] expectedAttributes = ["[Obsolete]"];

        attributes.ShouldBe(expectedAttributes);
    }

    [Fact]
    public void Test_Exceptions()
    {
        using var document = TestTools.GetDocumentationPage("MyLibrary.Tools.Collections.MyCollection`1.html");

        var exceptions = TestTools.GetExceptions(document.GetMemberElement("Add(`0)"));

        exceptions.Length.ShouldBe(2);

        string e1type = TestTools.GetExceptionType(exceptions[0]);
        string e1doc = TestTools.GetExceptionDoc(exceptions[0]);

        string e2type = TestTools.GetExceptionType(exceptions[1]);
        string e2doc = TestTools.GetExceptionDoc(exceptions[1]);

        e1type.ShouldBe("System.NotImplementedException");
        e1doc.ShouldBeEmpty();

        e2type.ShouldBe("System.ArgumentNullException");
        e2doc.ShouldBe("If the argument is null.");
    }
}
