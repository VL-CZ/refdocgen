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
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1",
        "System#Collections#IEnumerable#GetEnumerator",
        "IEnumerator IEnumerable.GetEnumerator()")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1",
        "AddGeneric``1(``0)",
        "public void AddGeneric<T2>(T2 item)")]
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
    [InlineData("Spring", "Spring = 0")]
    [InlineData("Winter", "Winter = 3")]
    public void Test_Enum_Member_Signature(string methodId, string expectedMethodSignature)
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Season.html");

        var method = document.GetMember(methodId);

        string methodName = Tools.GetMemberNameContent(method);
        methodName.ShouldBe(expectedMethodSignature);
    }

}

[Collection(DocumentationTestCollection.Name)]
public class TypeSignatureTests
{
    [Theory]
    [InlineData("MyLibrary.Dog", "public class Dog")]
    [InlineData("MyLibrary.Animal", "public abstract class Animal")]
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
public class TypeDataTests
{
    [Fact]
    public void Test_Type_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MySortedList`1.html");
        var constraints = Tools.GetTypeParamConstraints(document.GetTypeDataSection());

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
        var constraints = Tools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "object")]
    [InlineData("MyLibrary.Dog", "Animal")]
    [InlineData("MyLibrary.Tools.Collections.MySortedList`1", "MyCollection<T>")]
    public void Test_BaseType(string pageName, string expectedBaseType)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetBaseTypeName(document.GetTypeDataSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }

    [Theory]
    [InlineData("MyLibrary.Hierarchy.Child", "IChild")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Test_Interfaces(string pageName, string expectedInterfacesImplemented)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetInterfacesString(document.GetTypeDataSection());

        baseType.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }

    [Theory]
    [InlineData("MyLibrary.User", "MyLibrary")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "MyLibrary.Tools.Collections")]
    public void Test_Namespace(string pageName, string expectedNamespace)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetNamespaceString(document.GetTypeDataSection());

        baseType.ShouldBe($"namespace {expectedNamespace}");
    }
}

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
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetSummaryDocContent(document.GetTypeDataSection());

        baseType.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.Animal.html");

        var baseType = Tools.GetRemarksDocContent(document.GetTypeDataSection());

        baseType.ShouldBe("This class is abstract, use inheritance.");
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyStringCollection.html");

        var baseType = Tools.GetSeeAlsoDocs(document.GetTypeDataSection());

        string[] expectedDocs = ["My collection class", "ICollection<T>"];

        baseType.ShouldBe(expectedDocs);
    }

    [Fact]
    public void Test_Attributes()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        var baseType = Tools.GetAttributes(document.GetTypeDataSection());

        string[] expectedAttributes = [
            "[Serializable]",
            "[JsonSerializable(typeof(MyLibrary.User), GenerationMode = 2)]"];

        baseType.ShouldBe(expectedAttributes);
    }
}

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
    public void Test_Summary(string pageName, string memberId, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetSummaryDocContent(document.GetMember(memberId));

        baseType.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.Animal.html");

        var baseType = Tools.GetRemarksDocContent(document.GetMember("weight"));

        baseType.ShouldBe("The weight is in kilograms (kg).");
    }

    [Fact]
    public void Test_ValueDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        var baseType = Tools.GetValueDocContent(document.GetMember("Age"));

        baseType.ShouldBe("The age of the user.");
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "int", "The average lifespan.")]
    [InlineData("MyLibrary.User", "GetAnimalsByType", "Dictionary<string, List<Animal>>", "Dictionary of animals, indexed by their type.")]
    public void Test_ReturnsDoc(string pageName, string memberId, string returnType, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var baseType = Tools.GetReturnTypeName(document.GetMember(memberId));
        var doc = Tools.GetReturnsDoc(document.GetMember(memberId));

        baseType.ShouldBe(returnType);
        doc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        var baseType = Tools.GetSeeAlsoDocs(document.GetMember("username"));

        string[] expectedSeeAlsoDocs = [
            "http://www.google.com",
            "max age constant",
            "System.Reflection.FieldInfo.IsLiteral",
            "Point",
            "!:notFound"
            ];

        baseType.ShouldBe(expectedSeeAlsoDocs);
    }

    [Fact]
    public void Test_Attributes()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        var baseType = Tools.GetAttributes(document.GetMember("PrintProfile(System.String)"));

        string[] expectedAttributes = ["[Obsolete]"];

        baseType.ShouldBe(expectedAttributes);
    }
}
