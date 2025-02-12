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
    public void TypeConstraints_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MySortedList`1.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where T : IComparable<T>"]);
    }

    [Fact]
    public void MethodConstraints_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MyCollection`1.html");
        var method = document.GetMemberElement("AddGeneric``1(``0)");

        string[] constraints = TypePageTools.GetTypeParamConstraints(method);

        constraints.ShouldBe(["where T2 : class"]);
    }

    [Fact]
    public void ComplexConstraints_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.Tools.Collections.MyDictionary`2.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "object")]
    [InlineData("MyLibrary.Dog", "Animal")]
    [InlineData("MyLibrary.Tools.Collections.MySortedList`1", "MyCollection<T>")]
    public void BaseType_Matches(string pageName, string expectedBaseType)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        string baseType = TypePageTools.GetBaseTypeName(document.GetTypeDataSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }

    [Theory]
    [InlineData("MyLibrary.Hierarchy.Child", "IChild")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Interfaces_Match(string pageName, string expectedInterfacesImplemented)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        string interfaces = TypePageTools.GetInterfacesString(document.GetTypeDataSection());

        interfaces.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }

    [Theory]
    [InlineData("MyLibrary.User", "MyLibrary")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "MyLibrary.Tools.Collections")]
    public void Namespace_Matches(string pageName, string expectedNamespace)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        string ns = TypePageTools.GetNamespaceString(document.GetTypeDataSection());

        ns.ShouldBe($"namespace {expectedNamespace}");
    }

    [Fact]
    public void Attributes_Match()
    {
        using var document = DocumentationTools.GetPage("MyLibrary.User.html");

        string[] attributes = TypePageTools.GetAttributes(document.GetTypeDataSection());

        string[] expectedAttributes = [
            "[Serializable]",
            "[JsonSerializable(typeof(MyLibrary.User), GenerationMode = 2)]"];

        attributes.ShouldBe(expectedAttributes);
    }
}
