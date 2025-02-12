using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class TypeDataTests
{
    [Fact]
    public void Test_Type_Constraints()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Tools.Collections.MySortedList`1.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where T : IComparable<T>"]);
    }

    [Fact]
    public void Test_Method_Constraints()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Tools.Collections.MyCollection`1.html");
        var method = document.GetMemberElement("AddGeneric``1(``0)");

        string[] constraints = TypePageTools.GetTypeParamConstraints(method);

        constraints.ShouldBe(["where T2 : class"]);
    }

    [Fact]
    public void Test_Complex_Constraints()
    {
        using var document = TypePageTools.GetDocumentationPage("MyLibrary.Tools.Collections.MyDictionary`2.html");
        string[] constraints = TypePageTools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "object")]
    [InlineData("MyLibrary.Dog", "Animal")]
    [InlineData("MyLibrary.Tools.Collections.MySortedList`1", "MyCollection<T>")]
    public void Test_BaseType(string pageName, string expectedBaseType)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string baseType = TypePageTools.GetBaseTypeName(document.GetTypeDataSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }

    [Theory]
    [InlineData("MyLibrary.Hierarchy.Child", "IChild")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Test_Interfaces(string pageName, string expectedInterfacesImplemented)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string interfaces = TypePageTools.GetInterfacesString(document.GetTypeDataSection());

        interfaces.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }

    [Theory]
    [InlineData("MyLibrary.User", "MyLibrary")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "MyLibrary.Tools.Collections")]
    public void Test_Namespace(string pageName, string expectedNamespace)
    {
        using var document = TypePageTools.GetDocumentationPage($"{pageName}.html");

        string ns = TypePageTools.GetNamespaceString(document.GetTypeDataSection());

        ns.ShouldBe($"namespace {expectedNamespace}");
    }
}
