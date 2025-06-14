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
    [InlineData("RefDocGen.TestingLibrary.User", "Class representing an user of our app.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.IMyCollection-1", "My collection interface.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "Struct representing a point.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Season", "Represents season of a year.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.ObjectPredicate", "Predicate about an object.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1.MyCollectionEnumerator", "Custom collection enumerator.")]
    public void SummaryDoc_Matches(string pageName, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetTypeDataSection());

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void RemarksDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetTypeDataSection());

        remarksDoc.ShouldBe("This class is abstract, use inheritance.");
    }

    [Fact]
    public void SeeAlsoDocs_Match()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.Collections.MyStringCollection.html");

        string[] seeAlsoDocs = TypePageTools.GetSeeAlsoDocs(document.GetTypeDataSection());

        string[] expectedDocs = ["My collection class", "ICollection<T>"];

        seeAlsoDocs.ShouldBe(expectedDocs);
    }
}
