using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests.Tests.TypePage;

/// <summary>
/// This class tests that the documentation inheritance works as expected for simple scenarios.
/// </summary>
/// <remarks>
/// For more complex scenarios, check <see cref="HierarchyDocTests"/> class.
/// </remarks>
[Collection(DocumentationTestCollection.Name)]
public class InheritDocTests
{
    [Fact]
    public void MemberSummaryDoc_IsInherited()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Dog.html");

        var memberElement = document.GetMemberElement("GenerateAnimalProfile(System.String,System.String,System.DateTime)");
        string summaryDoc = TypePageTools.GetSummaryDoc(memberElement);

        summaryDoc.ShouldBe("A virtual method to generate an animal profile.");
    }

    [Fact]
    public void MemberSummaryDoc_IsInherited_WhenCrefIsUsed()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Dog.html");

        var memberElement = document.GetMemberElement("GenerateAnimalProfile(System.String,System.String,System.DateTime,System.String())");
        string summaryDoc = TypePageTools.GetSummaryDoc(memberElement);

        summaryDoc.ShouldBe("A virtual method to generate an animal profile.");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1", "AddRange(System.Collections.Generic.IEnumerable(-0))", "Add range of items into the collection.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1", "RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection(T).CanAdd", "Checks if an item can be added into the collection.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MySortedList-1", "AddRange(System.Collections.Generic.IEnumerable(-0))", "Add range of items into the collection.")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyStringCollection", "Add(System.String)", "Adds an item to the collection.")]
    public void MemberSummaryDoc_IsInherited_ForGenericType(string typeId, string memberId, string expectedSummaryDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{typeId}.html");

        var memberElement = document.GetMemberElement(memberId);
        string summaryDoc = TypePageTools.GetSummaryDoc(memberElement);

        summaryDoc.ShouldBe(expectedSummaryDoc);
    }
}
