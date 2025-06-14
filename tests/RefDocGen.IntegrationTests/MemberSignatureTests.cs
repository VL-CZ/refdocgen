using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of member signatures (including methods, fields, properties, etc.).
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class MemberSignatureTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Animal", "GetSound", "internal abstract string GetSound()")]
    [InlineData("RefDocGen.TestingLibrary.Dog",
        "GenerateAnimalProfile(System.String,System.String,System.DateTime)",
        "public override sealed string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth)")]
    [InlineData("RefDocGen.TestingLibrary.Dog", "GetSound", "internal override string GetSound()")]
    [InlineData("RefDocGen.TestingLibrary.Dog", "BarkAsync", "private async Task BarkAsync()")]
    [InlineData("RefDocGen.TestingLibrary.User", "GetAnimalsByType", "public Dictionary<string, List<Animal>> GetAnimalsByType()")]
    [InlineData("RefDocGen.TestingLibrary.User", "AddAnimals(RefDocGen.TestingLibrary.Animal())", "public void AddAnimals(params Animal[] animals)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.IMyCollection-1",
        "AddRange(System.Collections.Generic.IEnumerable(-0))",
        "public void AddRange(IEnumerable<T> range)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.StringExtensions",
        "ZipWith(System.String,System.String)",
        "public static string ZipWith(this string s1, string s2)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1",
        "System.Collections.IEnumerable.GetEnumerator",
        "IEnumerator IEnumerable.GetEnumerator()")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1",
        "AddGeneric--1(--0)",
        "public void AddGeneric<T2>(T2 item)")]
    public void MethodSignature_Matches(string pageName, string methodId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var method = document.GetMemberElement(methodId);

        string signature = TypePageTools.GetMemberSignature(method);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.User", "username", "protected readonly string username")]
    [InlineData("RefDocGen.TestingLibrary.User", "MaxAge", "private const int MaxAge = 150")]
    [InlineData("RefDocGen.TestingLibrary.Animal", "weight", "private int weight")]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", "location", "private readonly Point location")]
    public void FieldSignature_Matches(string pageName, string fieldId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var field = document.GetMemberElement(fieldId);

        string signature = TypePageTools.GetMemberSignature(field);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Animal", "Owner", "protected User Owner { get; set; }")]
    [InlineData("RefDocGen.TestingLibrary.User", "FirstName", "public string FirstName { get; internal set; }")]
    [InlineData("RefDocGen.TestingLibrary.User", "LastName", "public string LastName { get; init; }")]
    [InlineData(
        "RefDocGen.TestingLibrary.Tools.Collections.NonGenericCollection",
        "System.Collections.ICollection.IsSynchronized",
        "bool ICollection.IsSynchronized { get; }")]
    public void PropertySignature_Matches(string pageName, string propertyId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var property = document.GetMemberElement(propertyId);

        string signature = TypePageTools.GetMemberSignature(property);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "Item(System.Int32)", "public T this[int index] { get; private set; }")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "Item(System.Index)", "public T this[Index index] { get; private set; }")]
    public void IndexerSignature_Matches(string pageName, string indexerId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var indexer = document.GetMemberElement(indexerId);

        string signature = TypePageTools.GetMemberSignature(indexer);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.User", ".ctor", "public User()")]
    [InlineData("RefDocGen.TestingLibrary.User", ".ctor(System.String,System.Int32)", "public User(string username, int age)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", ".ctor(System.Double,System.Double)", "internal Point(double x, double y)")]
    public void ConstructorSignature_Matches(string pageName, string constructorId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var constructor = document.GetMemberElement(constructorId);

        string signature = TypePageTools.GetMemberSignature(constructor);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Equality(RefDocGen.TestingLibrary.Tools.Point,RefDocGen.TestingLibrary.Tools.Point)", "public static bool operator ==(Point point, Point other)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_UnaryNegation(RefDocGen.TestingLibrary.Tools.Point)", "public static Point operator -(Point point)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Explicit(RefDocGen.TestingLibrary.Tools.Point)~System.Numerics.Vector2", "public static explicit operator Vector2(Point point)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Implicit(System.Numerics.Vector2)~RefDocGen.TestingLibrary.Tools.Point", "public static implicit operator Point(Vector2 vector)")]
    public void OperatorSignature_Matches(string pageName, string operatorId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var operatorMember = document.GetMemberElement(operatorId);

        string signature = TypePageTools.GetMemberSignature(operatorMember);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", "OnTemperatureChange", "public event Action OnTemperatureChange")]
    public void EventSignature_Matches(string pageName, string eventId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var eventMember = document.GetMemberElement(eventId);

        string signature = TypePageTools.GetMemberSignature(eventMember);
        signature.ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("Spring", "Spring = 0")]
    [InlineData("Winter", "Winter = 3")]
    public void EnumMemberSignature_Matches(string enumMemberId, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.Season.html");

        var enumMember = document.GetMemberElement(enumMemberId);

        string signature = TypePageTools.GetMemberSignature(enumMember);
        signature.ShouldBe(expectedSignature);
    }

    [Fact]
    public void DelegateMethodSignature_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.MyPredicate-1.html");

        var delegateMethod = document.DocumentElement.GetByDataId(DataId.DelegateMethod);

        string signature = TypePageTools.GetMemberSignature(delegateMethod);
        signature.ShouldBe("internal delegate bool MyPredicate<T>(T obj)");
    }
}
