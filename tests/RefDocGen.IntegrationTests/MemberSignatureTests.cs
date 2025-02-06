using Shouldly;

namespace RefDocGen.IntegrationTests;

[Collection(DocumentationTestCollection.Name)]
public class MemberSignatureTests
{
    [Theory]
    [InlineData("MyLibrary.Animal", "GetSound", "internal abstract string GetSound()")]
    [InlineData("MyLibrary.Dog",
        "GenerateAnimalProfile(System.String,System.String,System.DateTime)",
        "public override sealed string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth)")]
    [InlineData("MyLibrary.Dog", "GetSound", "internal override string GetSound()")]
    [InlineData("MyLibrary.Dog", "BarkAsync", "private async Task BarkAsync()")]
    [InlineData("MyLibrary.User", "GetAnimalsByType", "public Dictionary<string, List<Animal>> GetAnimalsByType()")]
    [InlineData("MyLibrary.User", "AddAnimals(MyLibrary.Animal[])", "public void AddAnimals(params Animal[] animals)")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1",
        "AddRange(System.Collections.Generic.IEnumerable{`0})",
        "public void AddRange(IEnumerable<T> range)")]
    [InlineData("MyLibrary.Tools.StringExtensions",
        "ZipWith(System.String,System.String)",
        "public static string ZipWith(this string s1, string s2)")]
    public void Test_Method_Signature(string pageName, string methodId, string expectedMethodSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(methodId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedMethodSignature);
    }

    [Theory]
    [InlineData("MyLibrary.User", "username", "protected readonly string username")]
    [InlineData("MyLibrary.User", "MaxAge", "private const int MaxAge = 150")]
    [InlineData("MyLibrary.Animal", "weight", "private int weight")]
    [InlineData("MyLibrary.Tools.WeatherStation", "location", "private readonly Point location")]
    public void Test_Field_Signature(string pageName, string fieldId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(fieldId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "Owner", "protected User Owner { get; set; }")]
    [InlineData("MyLibrary.User", "FirstName", "public string FirstName { get; internal set; }")]
    [InlineData("MyLibrary.User", "LastName", "public string LastName { get; init; }")]
    [InlineData(
        "MyLibrary.Tools.Collections.NonGenericCollection",
        "System#Collections#ICollection#IsSynchronized",
        "bool ICollection.IsSynchronized { get; }")]
    public void Test_Property_Signature(string pageName, string fieldId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(fieldId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "Item(System.Int32)", "public T this[int index] { get; private set; }")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "Item(System.Index)", "public T this[Index index] { get; private set; }")]
    public void Test_Indexer_Signature(string pageName, string fieldId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(fieldId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.User", "#ctor", "public User()")]
    [InlineData("MyLibrary.User", "#ctor(System.String,System.Int32)", "public User(string username, int age)")]
    [InlineData("MyLibrary.Tools.Point", "#ctor(System.Double,System.Double)", "internal Point(double x, double y)")]
    public void Test_Constructor_Signature(string pageName, string methodId, string expectedMethodSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(methodId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedMethodSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.Point", "op_Equality(MyLibrary.Tools.Point,MyLibrary.Tools.Point)", "public static bool operator ==(Point point, Point other)")]
    [InlineData("MyLibrary.Tools.Point", "op_UnaryNegation(MyLibrary.Tools.Point)", "public static Point operator -(Point point)")]
    [InlineData("MyLibrary.Tools.Point", "op_Explicit(MyLibrary.Tools.Point)~System.Numerics.Vector2", "public static explicit operator Vector2(Point point)")]
    [InlineData("MyLibrary.Tools.Point", "op_Implicit(System.Numerics.Vector2)~MyLibrary.Tools.Point", "public static implicit operator Point(Vector2 vector)")]

    public void Test_Operator_Signature(string pageName, string methodId, string expectedMethodSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(methodId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedMethodSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.WeatherStation", "OnTemperatureChange", "public event Action OnTemperatureChange")]
    public void Test_Event_Signature(string pageName, string methodId, string expectedMethodSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(methodId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedMethodSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "public abstract class Animal")]
    [InlineData("MyLibrary.Dog", "public class Dog")]
    [InlineData("MyLibrary.Tools.Collections.MyDictionary`2", "internal class MyDictionary<TKey, TValue>")]
    [InlineData("MyLibrary.Tools.StringExtensions", "public static class StringExtensions")]
    [InlineData("MyLibrary.Tools.Point", "internal struct Point")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "internal interface IMyCollection<T>")]
    [InlineData("MyLibrary.Tools.ICovariant`1", "internal interface ICovariant<out T>")]
    [InlineData("MyLibrary.Tools.IContravariant`1", "internal interface IContravariant<in T>")]
    [InlineData("MyLibrary.Tools.Season", "internal enum Season")]
    [InlineData("MyLibrary.Tools.ObjectPredicate", "internal delegate ObjectPredicate")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "internal delegate MyPredicate<T>")]
    public void Test_Type_Signature(string pageName, string expectedTypeSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var typeNameElement = document.GetTypeName();

        string typeName = Tools.ParseStringContent(typeNameElement);
        typeName.ShouldBe(expectedTypeSignature);
    }
}

[Collection(DocumentationTestCollection.Name)]
public class GenericConstraintsSignatureTests
{
    [Fact]
    public void Test_Type_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MySortedList`1.html");
        var constraints = Tools.GetTypeParamConstraints(document.GetTypeDocsSection());

        constraints.ShouldBe(["where T : IComparable<T>"]);
    }

    [Fact]
    public void Test_Method_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyCollection`1.html");
        var method = document.GetMember("AddGeneric``1(``0)");

        var constraints = Tools.GetTypeParamConstraints(method);

        constraints.ShouldBe(["where T2 : class"]);
    }

    [Fact]
    public void Test_Complex_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyDictionary`2.html");
        var constraints = Tools.GetTypeParamConstraints(document.GetTypeDocsSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }
}

[Collection(DocumentationTestCollection.Name)]
public class BaseTypeNameTests
{
    [Theory]
    [InlineData("MyLibrary.Animal", "object")]
    [InlineData("MyLibrary.Dog", "Animal")]
    [InlineData("MyLibrary.Tools.Collections.MySortedList`1", "MyCollection<T>")]
    public void Test_Type_Constraints(string pageName, string expectedBaseType)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetBaseTypeName(document.GetTypeDocsSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }
}


[Collection(DocumentationTestCollection.Name)]
public class ImplementedInterfacesTests
{
    [Theory]
    [InlineData("MyLibrary.Hierarchy.Child", "IChild")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Test_Type_Constraints(string pageName, string expectedInterfacesImplemented)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetInterfacesString(document.GetTypeDocsSection());

        baseType.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }
}

[Collection(DocumentationTestCollection.Name)]
public class TypeNamespaceTests
{
    [Theory]
    [InlineData("MyLibrary.User", "MyLibrary")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "MyLibrary.Tools.Collections")]
    public void Test_Type_Constraints(string pageName, string expectedNamespace)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetNamespaceString(document.GetTypeDocsSection());

        baseType.ShouldBe($"namespace {expectedNamespace}");
    }
}
