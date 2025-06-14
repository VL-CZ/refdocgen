using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of various member documentation comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class MemberDocCommentTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.User", "MaxAge", "Maximum age of the user.")]
    [InlineData("RefDocGen.TestingLibrary.User", "FirstName", "First name of the user.")]
    [InlineData("RefDocGen.TestingLibrary.User", ".ctor(System.String,System.Int32)", "Initializes a new user using the provided username and age.")]
    [InlineData("RefDocGen.TestingLibrary.Animal", "GetAverageLifespan(System.String)", "Static method returning the average lifespan of an animal.")]
    [InlineData("RefDocGen.TestingLibrary.Dog", "Owner", "Dog's owner; NULL if the dog doesn't have any owner.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.IMyCollection-1", "AddRange(System.Collections.Generic.IEnumerable(-0))", "Add range of items into the collection.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Season", "Summer", "Represents summer.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", "OnTemperatureChange", "Temperature change event.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1",
        "System.Collections.IEnumerable.GetEnumerator",
        "Returns an enumerator that iterates through the collection.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.MyPredicate-1", "delegate-method", "Predicate about a generic type T.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1.MyCollectionEnumerator", "Reset", "Resets the enumerator.")]
    public void SummaryDoc_Matches(string pageName, string memberId, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetMemberElement(memberId));

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void RemarksDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetMemberElement("weight"));

        remarksDoc.ShouldBe("The weight is in kilograms (kg).");
    }

    [Fact]
    public void ValueDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.User.html");

        string valueDoc = TypePageTools.GetValueDoc(document.GetMemberElement("Age"));

        valueDoc.ShouldBe("The age of the user.");
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Animal", "GetAverageLifespan(System.String)", "int", "The average lifespan.")]
    [InlineData("RefDocGen.TestingLibrary.User", "GetAnimalsByType", "Dictionary<string, List<Animal>>", "Dictionary of animals, indexed by their type.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Equality(RefDocGen.TestingLibrary.Tools.Point,RefDocGen.TestingLibrary.Tools.Point)", "bool", "Are the 2 points equal?")]
    public void ReturnsDoc_Matches(string pageName, string memberId, string expectedReturnType, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string returnType = TypePageTools.GetReturnTypeName(document.GetMemberElement(memberId));
        string returnsDoc = TypePageTools.GetReturnsDoc(document.GetMemberElement(memberId));

        returnType.ShouldBe(expectedReturnType);
        returnsDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void SeeAlsoDocs_Match()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.User.html");

        string[] seeAlsoDocs = TypePageTools.GetSeeAlsoDocs(document.GetMemberElement("username"));

        string[] expectedDocs = [
            "http://www.google.com",
            "max age constant",
            "FieldInfo.IsLiteral",
            "Point",
            "!:notFound"
            ];

        seeAlsoDocs.ShouldBe(expectedDocs);
    }
}
