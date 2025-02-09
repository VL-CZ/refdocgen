using AngleSharp.Dom;
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
    public void Test_Method_Signature(string pageName, string methodId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var method = document.GetMember(methodId);

        string signature = Tools.GetMemberSignature(method);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.User", "username", "protected readonly string username")]
    [InlineData("MyLibrary.User", "MaxAge", "private const int MaxAge = 150")]
    [InlineData("MyLibrary.Animal", "weight", "private int weight")]
    [InlineData("MyLibrary.Tools.WeatherStation", "location", "private readonly Point location")]
    public void Test_Field_Signature(string pageName, string fieldId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var field = document.GetMember(fieldId);

        string signature = Tools.GetMemberSignature(field);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "Owner", "protected User Owner { get; set; }")]
    [InlineData("MyLibrary.User", "FirstName", "public string FirstName { get; internal set; }")]
    [InlineData("MyLibrary.User", "LastName", "public string LastName { get; init; }")]
    [InlineData(
        "MyLibrary.Tools.Collections.NonGenericCollection",
        "System#Collections#ICollection#IsSynchronized",
        "bool ICollection.IsSynchronized { get; }")]
    public void Test_Property_Signature(string pageName, string propetyId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var property = document.GetMember(propetyId);

        string signature = Tools.GetMemberSignature(property);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "Item(System.Int32)", "public T this[int index] { get; private set; }")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "Item(System.Index)", "public T this[Index index] { get; private set; }")]
    public void Test_Indexer_Signature(string pageName, string indexerId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var indexer = document.GetMember(indexerId);

        string signature = Tools.GetMemberSignature(indexer);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.User", "#ctor", "public User()")]
    [InlineData("MyLibrary.User", "#ctor(System.String,System.Int32)", "public User(string username, int age)")]
    [InlineData("MyLibrary.Tools.Point", "#ctor(System.Double,System.Double)", "internal Point(double x, double y)")]
    public void Test_Constructor_Signature(string pageName, string constructorId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var constructor = document.GetMember(constructorId);

        string signature = Tools.GetMemberSignature(constructor);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.Point", "op_Equality(MyLibrary.Tools.Point,MyLibrary.Tools.Point)", "public static bool operator ==(Point point, Point other)")]
    [InlineData("MyLibrary.Tools.Point", "op_UnaryNegation(MyLibrary.Tools.Point)", "public static Point operator -(Point point)")]
    [InlineData("MyLibrary.Tools.Point", "op_Explicit(MyLibrary.Tools.Point)~System.Numerics.Vector2", "public static explicit operator Vector2(Point point)")]
    [InlineData("MyLibrary.Tools.Point", "op_Implicit(System.Numerics.Vector2)~MyLibrary.Tools.Point", "public static implicit operator Point(Vector2 vector)")]
    public void Test_Operator_Signature(string pageName, string operatorId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var operatorMember = document.GetMember(operatorId);

        string signature = Tools.GetMemberSignature(operatorMember);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("MyLibrary.Tools.WeatherStation", "OnTemperatureChange", "public event Action OnTemperatureChange")]
    public void Test_Event_Signature(string pageName, string eventId, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var eventMember = document.GetMember(eventId);

        string signature = Tools.GetMemberSignature(eventMember);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("Spring", "Spring = 0")]
    [InlineData("Winter", "Winter = 3")]
    public void Test_Enum_Member_Signature(string enumMemberId, string expectedSignature)
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Season.html");

        var enumMember = document.GetMember(enumMemberId);

        string signature = Tools.GetMemberSignature(enumMember);
        signature.ShouldBe(expectedSignature);
    }

    [Fact]
    public void Test_Delegate_Method_Signature()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.MyPredicate`1.html");

        var delegateMethod = document.DocumentElement.GetByDataId(DataId.DelegateMethod);

        string signature = Tools.GetMemberSignature(delegateMethod);
        signature.ShouldBe("internal delegate bool MyPredicate<T>(T obj)");
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
    public void Test_Type_Signature(string pageName, string expectedSignature)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string signature = Tools.GetTypeSignature(document);

        signature.ShouldBe(expectedSignature);
    }
}

[Collection(DocumentationTestCollection.Name)]
public class TypeDataTests
{
    [Fact]
    public void Test_Type_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MySortedList`1.html");
        string[] constraints = Tools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where T : IComparable<T>"]);
    }

    [Fact]
    public void Test_Method_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyCollection`1.html");
        var method = document.GetMember("AddGeneric``1(``0)");

        string[] constraints = Tools.GetTypeParamConstraints(method);

        constraints.ShouldBe(["where T2 : class"]);
    }

    [Fact]
    public void Test_Complex_Constraints()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyDictionary`2.html");
        string[] constraints = Tools.GetTypeParamConstraints(document.GetTypeDataSection());

        constraints.ShouldBe(["where TKey : class, new(), IComparable, IEquatable<TKey>", "where TValue : struct, IDisposable"]);
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "object")]
    [InlineData("MyLibrary.Dog", "Animal")]
    [InlineData("MyLibrary.Tools.Collections.MySortedList`1", "MyCollection<T>")]
    public void Test_BaseType(string pageName, string expectedBaseType)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string baseType = Tools.GetBaseTypeName(document.GetTypeDataSection());

        baseType.ShouldBe($"Base type: {expectedBaseType}");
    }

    [Theory]
    [InlineData("MyLibrary.Hierarchy.Child", "IChild")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "ICollection<T>, IEnumerable<T>, IEnumerable")]
    public void Test_Interfaces(string pageName, string expectedInterfacesImplemented)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string interfaces = Tools.GetInterfacesString(document.GetTypeDataSection());

        interfaces.ShouldBe($"Implements: {expectedInterfacesImplemented}");
    }

    [Theory]
    [InlineData("MyLibrary.User", "MyLibrary")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "MyLibrary.Tools.Collections")]
    public void Test_Namespace(string pageName, string expectedNamespace)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string ns = Tools.GetNamespaceString(document.GetTypeDataSection());

        ns.ShouldBe($"namespace {expectedNamespace}");
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

        string summaryDoc = Tools.GetSummaryDocContent(document.GetTypeDataSection());

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.Animal.html");

        string remarksDoc = Tools.GetRemarksDocContent(document.GetTypeDataSection());

        remarksDoc.ShouldBe("This class is abstract, use inheritance.");
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyStringCollection.html");

        string[] seeAlsoDocs = Tools.GetSeeAlsoDocs(document.GetTypeDataSection());

        string[] expectedDocs = ["My collection class", "System.Collections.Generic.ICollection`1"]; // TODO: update to ICollection<T>

        seeAlsoDocs.ShouldBe(expectedDocs);
    }

    [Fact]
    public void Test_Attributes()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        string[] attributes = Tools.GetAttributes(document.GetTypeDataSection());

        string[] expectedAttributes = [
            "[Serializable]",
            "[JsonSerializable(typeof(MyLibrary.User), GenerationMode = 2)]"];

        attributes.ShouldBe(expectedAttributes);
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
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1",
        "System#Collections#IEnumerable#GetEnumerator",
        "Returns an enumerator that iterates through the collection.")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "delegate-method", "Predicate about a generic type T.")]
    public void Test_Summary(string pageName, string memberId, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string summaryDoc = Tools.GetSummaryDocContent(document.GetMember(memberId));

        summaryDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_RemarksDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.Animal.html");

        string remarksDoc = Tools.GetRemarksDocContent(document.GetMember("weight"));

        remarksDoc.ShouldBe("The weight is in kilograms (kg).");
    }

    [Fact]
    public void Test_ValueDoc()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        string valueDoc = Tools.GetValueDocContent(document.GetMember("Age"));

        valueDoc.ShouldBe("The age of the user.");
    }

    [Theory]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "int", "The average lifespan.")]
    [InlineData("MyLibrary.User", "GetAnimalsByType", "Dictionary<string, List<Animal>>", "Dictionary of animals, indexed by their type.")]
    [InlineData("MyLibrary.Tools.Point", "op_Equality(MyLibrary.Tools.Point,MyLibrary.Tools.Point)", "bool", "Are the 2 points equal?")]
    public void Test_ReturnsDoc(string pageName, string memberId, string expectedReturnType, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        string returnType = Tools.GetReturnTypeName(document.GetMember(memberId));
        string returnsDoc = Tools.GetReturnsDoc(document.GetMember(memberId));

        returnType.ShouldBe(expectedReturnType);
        returnsDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_SeeAlsoDocs()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        string[] seeAlsoDocs = Tools.GetSeeAlsoDocs(document.GetMember("username"));

        string[] expectedDocs = [
            "http://www.google.com",
            "max age constant",
            "System.Reflection.FieldInfo.IsLiteral",
            "Point",
            "!:notFound"
            ];

        seeAlsoDocs.ShouldBe(expectedDocs);
    }
}

[Collection(DocumentationTestCollection.Name)]
public class MemberDataTests
{
    [Fact]
    public void Test_Attributes()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");

        string[] attributes = Tools.GetAttributes(document.GetMember("PrintProfile(System.String)"));

        string[] expectedAttributes = ["[Obsolete]"];

        attributes.ShouldBe(expectedAttributes);
    }

    [Fact]
    public void Test_Exceptions()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyCollection`1.html");

        var exceptions = Tools.GetExceptions(document.GetMember("Add(`0)"));

        exceptions.Length.ShouldBe(2);

        string e1type = Tools.GetExceptionType(exceptions[0]);
        string e1doc = Tools.GetExceptionDoc(exceptions[0]);

        string e2type = Tools.GetExceptionType(exceptions[1]);
        string e2doc = Tools.GetExceptionDoc(exceptions[1]);

        e1type.ShouldBe("System.NotImplementedException");
        e1doc.ShouldBeEmpty();

        e2type.ShouldBe("System.ArgumentNullException");
        e2doc.ShouldBe("If the argument is null.");
    }
}

[Collection(DocumentationTestCollection.Name)]
public class ParameterDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "string species", "The species of the animal.")]
    [InlineData("MyLibrary.Tools.Point", "op_UnaryNegation(MyLibrary.Tools.Point)", "Point point", "The provided point.")]
    [InlineData("MyLibrary.Tools.WeatherStation", "#ctor(MyLibrary.Tools.Point)", "Point location", "Location of the weather station.")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "Item(System.Index)", "Index index", "An Index struct.")]
    [InlineData(
        "MyLibrary.User",
        "AddAnimalsByType(System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.List{MyLibrary.Animal}})",
        "Dictionary<string, List<Animal>> animals",
        "Animals to add. Key: animal type, Value: list of animals of the given type.")]
    public void Test_SingleParameter(string pageName, string memberId, string parameterSignature, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var parameters = Tools.GetMemberParameters(document.GetMember(memberId));

        parameters.Length.ShouldBe(1);

        string paramSignature = Tools.GetParameterName(parameters[0]);
        paramSignature.ShouldBe(parameterSignature);

        string paramDoc = Tools.GetParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_MethodWithManyParameters()
    {
        using var document = Tools.GetDocument("MyLibrary.User.html");
        var memberElement = document.GetMember("ProcessValues(System.Int32@,System.Int32@,System.String,System.Int32@,System.Double)");

        var parameters = Tools.GetMemberParameters(memberElement);

        parameters.Length.ShouldBe(5);

        List<(string signature, string? doc)> expectedValues = [
            ("in int inValue", "An input value."),
            ("ref int refValue", "A reference value."),
            ("string s1", ""),
            ("out int outValue", "An output value."),
            ("double d2", "")
        ];

        for (int i = 0; i < expectedValues.Count; i++)
        {
            string paramName = Tools.GetParameterName(parameters[i]);
            paramName.ShouldBe(expectedValues[i].signature);

            string paramDoc = Tools.GetParameterDoc(parameters[i]);
            paramDoc.ShouldBe(expectedValues[i].doc);
        }
    }
}

[Collection(DocumentationTestCollection.Name)]
public class TypeParameterDocCommentTests
{
    [Theory]
    [InlineData("MyLibrary.Tools.Collections.MyCollection`1", "T", "The type of the items in the collection.")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "T", "The type of the object.")]
    public void Test_SingleTypeParameter(string pageName, string parameterSignature, string expectedDoc)
    {
        using var document = Tools.GetDocument($"{pageName}.html");

        var parameters = Tools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(1);

        string paramSignature = Tools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe(parameterSignature);

        string paramDoc = Tools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Test_MethodWithTypeParameter()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.MyCollection`1.html");

        var parameters = Tools.GetTypeParameters(document.GetMember("AddGeneric``1(``0)"));

        parameters.Length.ShouldBe(1);

        string paramSignature = Tools.GetTypeParameterName(parameters[0]);
        paramSignature.ShouldBe("T2");

        string paramDoc = Tools.GetTypeParameterDoc(parameters[0]);
        paramDoc.ShouldBe("Type of the item to add.");
    }

    [Fact]
    public void Test_MultipleTypeParameters()
    {
        using var document = Tools.GetDocument("MyLibrary.Tools.Collections.IMyDictionary`2.html");

        var parameters = Tools.GetTypeParameters(document.GetTypeDataSection());

        parameters.Length.ShouldBe(2);

        string paramSignature1 = Tools.GetTypeParameterName(parameters[0]);
        paramSignature1.ShouldBe("TKey");

        string paramDoc1 = Tools.GetTypeParameterDoc(parameters[0]);
        paramDoc1.ShouldBe("Type of the key.");

        string paramSignature2 = Tools.GetTypeParameterName(parameters[1]);
        paramSignature2.ShouldBe("TValue");

        string paramDoc2 = Tools.GetTypeParameterDoc(parameters[1]);
        paramDoc2.ShouldBe("Type of the value.");


    }
}

[Collection(DocumentationTestCollection.Name)]
public class NamespaceDetailsPageTests : IDisposable
{
    private readonly IDocument document;

    public NamespaceDetailsPageTests()
    {
        document = Tools.GetDocument("MyLibrary.Tools.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void TestClasses()
    {
        var classes = Tools.GetNamespaceClasses(document);

        classes.Length.ShouldBe(2);

        string class1Name = Tools.GetTypeRowName(classes[0]);
        string class2Name = Tools.GetTypeRowName(classes[1]);

        class1Name.ShouldBe("class StringExtensions");
        class2Name.ShouldBe("class WeatherStation");
    }

    [Fact]
    public void TestStructs()
    {
        var structs = Tools.GetNamespaceStructs(document);

        structs.Length.ShouldBe(1);

        string structName = Tools.GetTypeRowName(structs[0]);

        structName.ShouldBe("struct Point");
    }

    [Fact]
    public void TestInterfaces()
    {
        var interfaces = Tools.GetNamespaceInterfaces(document);

        interfaces.Length.ShouldBe(2);

        string interface1Name = Tools.GetTypeRowName(interfaces[0]);
        string interface2Name = Tools.GetTypeRowName(interfaces[1]);

        interface1Name.ShouldBe("interface IContravariant<T>");
        interface2Name.ShouldBe("interface ICovariant<T>");
    }

    [Fact]
    public void TestEnums()
    {
        var enums = Tools.GetNamespaceEnums(document);

        enums.Length.ShouldBe(2);

        string enum1Name = Tools.GetTypeRowName(enums[0]);
        string enum2Name = Tools.GetTypeRowName(enums[1]);

        enum1Name.ShouldBe("enum HarvestingSeason");
        enum2Name.ShouldBe("enum Season");
    }

    [Fact]
    public void TestDelegates()
    {
        var delegates = Tools.GetNamespaceDelegates(document);

        delegates.Length.ShouldBe(2);

        string delegate1Name = Tools.GetTypeRowName(delegates[0]);
        string delegate2Name = Tools.GetTypeRowName(delegates[1]);

        delegate1Name.ShouldBe("delegate MyPredicate<T>");
        delegate2Name.ShouldBe("delegate ObjectPredicate");
    }

}

[Collection(DocumentationTestCollection.Name)]
public class NamespaceListPageTests : IDisposable
{
    private readonly IDocument document;

    public NamespaceListPageTests()
    {
        document = Tools.GetDocument("index.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void TestNamespaceNames()
    {
        string[] classes = Tools.GetNamespaceNames(document);

        string[] expected = [
            "namespace MyLibrary",
            "namespace MyLibrary.CyclicDoc",
            "namespace MyLibrary.Hierarchy",
            "namespace MyLibrary.Tools",
            "namespace MyLibrary.Tools.Collections"
        ];

        classes.ShouldBe(expected);
    }

    [Fact]
    public void TestNamespaceTypes()
    {
        var ns = document.GetElementById("MyLibrary.Tools");

        string[] nsTypes = Tools.GetNamespaceTypeNames(ns);

        string[] expected = [
            "enum HarvestingSeason",
            "interface IContravariant<T>",
            "interface ICovariant<T>",
            "delegate MyPredicate<T>",
            "delegate ObjectPredicate",
            "struct Point",
            "enum Season",
            "class StringExtensions",
            "class WeatherStation"
        ];

        nsTypes.ShouldBe(expected);
    }

}
