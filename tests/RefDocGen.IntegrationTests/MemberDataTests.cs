using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class tests that attributes and exceptions are present in the resulting documentation at member level.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class MemberDataTests
{
    [Fact]
    public void Attributes_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.User.html");

        string[] attributes = TypePageTools.GetAttributes(document.GetMemberElement("PrintProfile(System.String)"));

        string[] expectedAttributes = ["[Obsolete]"];

        attributes.ShouldBe(expectedAttributes);
    }

    [Fact]
    public void Exceptions_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MyCollection`1.html");

        var exceptions = TypePageTools.GetExceptions(document.GetMemberElement("Add(`0)"));

        exceptions.Length.ShouldBe(2);

        string e1type = TypePageTools.GetExceptionType(exceptions[0]);
        string e1doc = TypePageTools.GetExceptionDoc(exceptions[0]);

        string e2type = TypePageTools.GetExceptionType(exceptions[1]);
        string e2doc = TypePageTools.GetExceptionDoc(exceptions[1]);

        e1type.ShouldBe("System.NotImplementedException");
        e1doc.ShouldBeEmpty();

        e2type.ShouldBe("System.ArgumentNullException");
        e2doc.ShouldBe("If the argument is null.");
    }
}
