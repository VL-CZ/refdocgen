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
    [InlineData("RefDocGen.ExampleLibrary.User", "MaxAge", "Maximum age of the user.")]
    [InlineData("RefDocGen.ExampleLibrary.User", "FirstName", "First name of the user.")]
    [InlineData("RefDocGen.ExampleLibrary.User", ".ctor(System.String,System.Int32)", "Initializes a new user using the provided username and age.")]
    [InlineData("RefDocGen.ExampleLibrary.Animal", "GetAverageLifespan(System.String)", "Static method returning the average lifespan of an animal.")]
    [InlineData("RefDocGen.ExampleLibrary.Dog", "Owner", "Dog's owner; NULL if the dog doesn't have any owner.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection-1", "AddRange(System.Collections.Generic.IEnumerable(-0))", "Add range of items into the collection.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Season", "Summer", "Represents summer.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.WeatherStation", "OnTemperatureChange", "Temperature change event.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1",
        "System.Collections.IEnumerable.GetEnumerator",
        "Returns an enumerator that iterates through the collection.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.MyPredicate-1", "delegate-method", "Predicate about a generic type T.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1.MyCollectionEnumerator", "Reset", "Resets the enumerator.")]
    [InlineData("RefDocGen.ExampleFSharpLibrary.FSharpMathTools", "Add(System.Int32,System.Int32)", "Adds two integers.")]
    [InlineData("RefDocGen.ExampleVbLibrary.VbMathTools", "Add(System.Int32,System.Int32)", "Adds two integers.")]
    public void SummaryDoc_Matches(string pageName, string memberId, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetMemberElement(memberId));

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void RemarksDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetMemberElement("weight"));

        remarksDoc.ShouldBe("The weight is in kilograms (kg).");
    }

    [Fact]
    public void ExampleDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.User.html");

        string remarksDoc = TypePageTools.GetExampleDoc(document.GetMemberElement("ToYaml"));

        remarksDoc.ShouldBe("user.ToYaml()");
    }

    [Fact]
    public void ValueDoc_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.User.html");

        string valueDoc = TypePageTools.GetValueDoc(document.GetMemberElement("Age"));

        valueDoc.ShouldBe("The age of the user.");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Animal", "GetAverageLifespan(System.String)", "int", "The average lifespan.")]
    [InlineData("RefDocGen.ExampleLibrary.User", "GetAnimalsByType", "Dictionary<string, List<Animal>>", "Dictionary of animals, indexed by their type.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Point", "op_Equality(RefDocGen.ExampleLibrary.Tools.Point,RefDocGen.ExampleLibrary.Tools.Point)", "bool", "Are the 2 points equal?")]
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
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.User.html");

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
