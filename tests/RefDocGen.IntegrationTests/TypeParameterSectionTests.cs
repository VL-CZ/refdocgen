using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the 'Type parameters' section - i.e. the individual parameter signatures and doc comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeParameterSectionTests
{
    [Theory]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "T", "The type of the items in the collection.")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "T", "The type of the object.")]
    public void TypeSection_WithSingleTypeParameter_Matches(string pageName, string parameterSignature, string expectedDoc)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(1);

        string paramSignature = TypePageTools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe(parameterSignature);

        string paramDoc = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void MethodSection_WithSingleTypeParameter_Matches()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MyCollection`1.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetMemberElement("AddGeneric``1(``0)"));

        parameters.Length.ShouldBe(1);

        string paramSignature = TypePageTools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe("T2");

        string paramDoc = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe("Type of the item to add.");
    }

    [Fact]
    public void Section_WithMultipleTypeParameter_Matches()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.IMyDictionary`2.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(2);

        string paramSignature1 = TypePageTools.GetTypeParameterName(parameters[0]);
        paramSignature1.ShouldBe("TKey");

        string paramDoc1 = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc1.ShouldBe("Type of the key.");

        string paramSignature2 = TypePageTools.GetTypeParameterName(parameters[1]);
        paramSignature2.ShouldBe("TValue");

        string paramDoc2 = TypePageTools.GetTypeParameterDoc(parameters[1]);
        paramDoc2.ShouldBe("Type of the value.");


    }
}
