using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class TypeParameterDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "T", "The type of the items in the collection.")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "T", "The type of the object.")]
    public void Test_SingleTypeParameter(string pageName, string parameterSignature, string expectedDoc)
    {
        using var document = TestTools.GetDocumentationPage($"{pageName}.html");

        var parameters = TestTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(1);

        string paramSignature = TestTools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe(parameterSignature);

        string paramDoc = TestTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_MethodWithTypeParameter()
    {
        using var document = TestTools.GetDocumentationPage("MyLibrary.Tools.Collections.MyCollection`1.html");

        var parameters = TestTools.GetTypeParameters(document.GetMemberElement("AddGeneric``1(``0)"));

        parameters.Length.ShouldBe(1);

        string paramSignature = TestTools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe("T2");

        string paramDoc = TestTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe("Type of the item to add.");
    }

    [Fact]
    public void Test_MultipleTypeParameters()
    {
        using var document = TestTools.GetDocumentationPage("MyLibrary.Tools.Collections.IMyDictionary`2.html");

        var parameters = TestTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(2);

        string paramSignature1 = TestTools.GetTypeParameterName(parameters[0]);
        paramSignature1.ShouldBe("TKey");

        string paramDoc1 = TestTools.GetTypeParameterDoc(parameters[0]);
        paramDoc1.ShouldBe("Type of the key.");

        string paramSignature2 = TestTools.GetTypeParameterName(parameters[1]);
        paramSignature2.ShouldBe("TValue");

        string paramDoc2 = TestTools.GetTypeParameterDoc(parameters[1]);
        paramDoc2.ShouldBe("Type of the value.");


    }
}
