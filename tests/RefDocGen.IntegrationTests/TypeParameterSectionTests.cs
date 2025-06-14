using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the 'Type parameters' section - i.e. the individual parameter declarations and doc comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeParameterSectionTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "T", "The type of the items in the collection.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.MyPredicate-1", "T", "The type of the object.")]
    public void TypeParameterData_Match_ForSingleTypeParameter(string pageName, string parameterDeclaration, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(1);

        string paramDeclaration = TypePageTools.GetTypeParameterDeclaration(parameters[0]);
        paramDeclaration.ShouldBe(parameterDeclaration);

        string paramDoc = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void TypeParameterData_Match_ForMethodTypeParameter()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetMemberElement("AddGeneric--1(--0)"));

        parameters.Length.ShouldBe(1);

        string paramDeclaration = TypePageTools.GetTypeParameterDeclaration(parameters[0]);
        paramDeclaration.ShouldBe("T2");

        string paramDoc = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe("Type of the item to add.");
    }

    [Fact]
    public void TypeParameterData_Match_ForMultipleTypeParameters()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.Collections.IMyDictionary-2.html");

        var parameters = TypePageTools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(2);

        string paramDeclaration1 = TypePageTools.GetTypeParameterDeclaration(parameters[0]);
        paramDeclaration1.ShouldBe("TKey");

        string paramDoc1 = TypePageTools.GetTypeParameterDoc(parameters[0]);
        paramDoc1.ShouldBe("Type of the key.");

        string paramDeclaration2 = TypePageTools.GetTypeParameterDeclaration(parameters[1]);
        paramDeclaration2.ShouldBe("TValue");

        string paramDoc2 = TypePageTools.GetTypeParameterDoc(parameters[1]);
        paramDoc2.ShouldBe("Type of the value.");


    }
}
