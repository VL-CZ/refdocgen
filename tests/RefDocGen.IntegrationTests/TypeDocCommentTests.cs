﻿using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class TypeDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.User", "Class representing an user of our app.")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "My collection interface.")]
    [InlineData("MyLibrary.Tools.Point", "Struct representing a point.")]
    [InlineData("MyLibrary.Tools.Season", "Represents season of a year.")]
    [InlineData("MyLibrary.Tools.ObjectPredicate", "Predicate about an object.")]
    public void Test_Summary(string pageName, string expectedDoc)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string summaryDoc = TypePageTools.GetSummaryDoc(document.GetTypeDataSection());

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Animal.html");

        string remarksDoc = TypePageTools.GetRemarksDoc(document.GetTypeDataSection());

        remarksDoc.ShouldBe("This class is abstract, use inheritance.");
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Tools.Collections.MyStringCollection.html");

        string[] seeAlsoDocs = TypePageTools.GetSeeAlsoDocs(document.GetTypeDataSection());

        string[] expectedDocs = ["My collection class", "System.Collections.Generic.ICollection`1"]; // TODO: update to ICollection<T>

        seeAlsoDocs.ShouldBe(expectedDocs);
    }

    [Fact]
    public void Test_Attributes()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.User.html");

        string[] attributes = TypePageTools.GetAttributes(document.GetTypeDataSection());

        string[] expectedAttributes = [
            "[Serializable]",
            "[JsonSerializable(typeof(MyLibrary.User), GenerationMode = 2)]"];

        attributes.ShouldBe(expectedAttributes);
    }
}
