using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of various type documentation comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.User", "Class representing an user of our app.")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "My collection interface.")]
    [InlineData("MyLibrary.Tools.Point", "Struct representing a point.")]
    [InlineData("MyLibrary.Tools.Season", "Represents season of a year.")]
    [InlineData("MyLibrary.Tools.ObjectPredicate", "Predicate about an object.")]
    public void SummaryDoc_Matches(string pageName, string expectedDoc)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetTypeDataSection());

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void RemarksDoc_Matches()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetTypeDataSection());

        remarksDoc.ShouldBe("This class is abstract, use inheritance.");
    }

    [Fact]
    public void SeeAlsoDocs_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MyStringCollection.html");

        string[] seeAlsoDocs = TypePageTools.GetSeeAlsoDocs(document.GetTypeDataSection());

        string[] expectedDocs = ["My collection class", "System.Collections.Generic.ICollection`1"]; // TODO: update to ICollection<T>

        seeAlsoDocs.ShouldBe(expectedDocs);
    }
}
