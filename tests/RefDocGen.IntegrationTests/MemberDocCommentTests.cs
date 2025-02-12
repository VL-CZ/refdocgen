using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class MemberDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.User", "MaxAge", "Maximum age of the user.")]
    [InlineData("MyLibrary.User", "FirstName", "First name of the user.")]
    [InlineData("MyLibrary.User", "#ctor(System.String,System.Int32)", "Initializes a new user using the provided username and age.")]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "Static method returning the average lifespan of an animal.")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "AddRange(System.Collections.Generic.IEnumerable{`0})", "Add range of items into the collection.")]
    [InlineData("MyLibrary.Tools.Season", "Summer", "Represents summer.")]
    [InlineData("MyLibrary.Tools.WeatherStation", "OnTemperatureChange", "Temperature change event.")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1",
        "System#Collections#IEnumerable#GetEnumerator",
        "Returns an enumerator that iterates through the collection.")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "delegate-method", "Predicate about a generic type T.")]
    public void Test_Summary(string pageName, string memberId, string expectedDoc)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetMemberElement(memberId));

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetMemberElement("weight"));

        remarksDoc.ShouldBe("The weight is in kilograms (kg).");
    }

    [Fact]
    public void Test_ValueDoc()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.User.html");

        string valueDoc = TypePageTools.GetValueDoc(document.GetMemberElement("Age"));

        valueDoc.ShouldBe("The age of the user.");
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "int", "The average lifespan.")]
    [InlineData("MyLibrary.User", "GetAnimalsByType", "Dictionary<string, List<Animal>>", "Dictionary of animals, indexed by their type.")]
    [InlineData("MyLibrary.Tools.Point", "op_Equality(MyLibrary.Tools.Point,MyLibrary.Tools.Point)", "bool", "Are the 2 points equal?")]
    public void Test_ReturnsDoc(string pageName, string memberId, string expectedReturnType, string expectedDoc)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string returnType = TypePageTools.GetReturnTypeName(document.GetMemberElement(memberId));
        string returnsDoc = TypePageTools.GetReturnsDoc(document.GetMemberElement(memberId));

        returnType.ShouldBe(expectedReturnType);
        returnsDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.User.html");

        string[] seeAlsoDocs = TypePageTools.GetSeeAlsoDocs(document.GetMemberElement("username"));

        string[] expectedDocs = [
            "http://www.google.com",
            "max age constant",
            "System.Reflection.FieldInfo.IsLiteral",
            "Point",
            "!:notFound"
            ];

        seeAlsoDocs.ShouldBe(expectedDocs);
    }
}
