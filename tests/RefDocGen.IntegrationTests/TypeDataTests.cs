using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of any type-related data, excluding the docuemntation.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeDataTests
{
    [Fact]
    public void SingleTypeConstraint_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Tools.Collections.MySortedList-1.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where T : IComparable<T>"]);
    }

    [Fact]
    public void ComplexTypeConstraints_Match()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Tools.Collections.MyDictionary-2.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Animal", "object")]
    [InlineData("RefDocGen.ExampleLibrary.Dog", "Animal")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MySortedList-1", "MyCollection<T>")]
    public void BaseType_Matches(string pageName, string expectedBaseType)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string baseType = TypePageTools.GetBaseTypeName(document.GetTypeDataSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Hierarchy.Child", "IChild")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection-1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Interfaces_Match(string pageName, string expectedInterfacesImplemented)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string interfaces = TypePageTools.GetInterfacesString(document.GetTypeDataSection());

        interfaces.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.User", "RefDocGen.ExampleLibrary")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection-1", "RefDocGen.ExampleLibrary.Tools.Collections")]
    public void Namespace_Matches(string pageName, string expectedNamespace)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string ns = TypePageTools.GetNamespaceString(document.GetTypeDataSection());

        ns.ShouldBe($"namespace {expectedNamespace}");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.User", "RefDocGen.ExampleLibrary")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection-1", "RefDocGen.ExampleLibrary")]
    public void Assembly_Matches(string pageName, string expectedAssembly)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string ns = TypePageTools.GetAssemblyString(document.GetTypeDataSection());

        ns.ShouldBe($"assembly {expectedAssembly}");
    }

    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1.MyCollectionEnumerator", "MyCollection<T>")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1.GenericEnumerator-1", "MyCollection<T>")]
    public void DeclaringType_Matches(string pageName, string expectedDeclaringType)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string declaringType = TypePageTools.GetDeclaringTypeName(document.GetTypeDataSection());

        declaringType.ShouldBe($"Declaring type: {expectedDeclaringType}");
    }

    [Fact]
    public void Attributes_Match()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.User.html");

        string[] attributes = TypePageTools.GetAttributes(document.GetTypeDataSection());

        string[] expectedAttributes = [
            "[Serializable]",
            "[JsonSerializable(typeof(RefDocGen.ExampleLibrary.User), GenerationMode = 2)]"];

        attributes.ShouldBe(expectedAttributes);
    }

    [Fact]
    public void NestedTypes_Match()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.ExampleLibrary.Tools.Collections.MyCollection-1.html");

        var nestedTypeElements = TypePageTools.GetNestedTypes(document.GetTypeDataSection());

        (string typeName, string summaryDoc)[] expectedNestedTypes = [
            ("class MyCollection<T>.GenericEnumerator<T2>", "Generic collection enumerator."),
            ("class MyCollection<T>.MyCollectionEnumerator", "Custom collection enumerator."),
            ("enum MyCollection<T>.Status", "Status of the collection.")];

        int index = 0;
        foreach (var typeElement in nestedTypeElements)
        {
            string typeName = TypePageTools.GetNestedTypeName(typeElement);
            string summaryDoc = TypePageTools.GetSummaryDoc(typeElement);

            typeName.ShouldBe(expectedNestedTypes[index].typeName);
            summaryDoc.ShouldBe(expectedNestedTypes[index].summaryDoc);

            index++;
        }
    }
}
