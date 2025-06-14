using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of member declarations (including methods, fields, properties, etc.).
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class MemberDeclarationTests
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
    public void MethodDeclaration_Matches(string pageName, string methodId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var method = document.GetMemberElement(methodId);

        string declaration = TypePageTools.GetMemberDeclaration(method);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.User", "username", "protected readonly string username")]
    [InlineData("RefDocGen.TestingLibrary.User", "MaxAge", "private const int MaxAge = 150")]
    [InlineData("RefDocGen.TestingLibrary.Animal", "weight", "private int weight")]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", "location", "private readonly Point location")]
    public void FieldDeclaration_Matches(string pageName, string fieldId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var field = document.GetMemberElement(fieldId);

        string declaration = TypePageTools.GetMemberDeclaration(field);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Animal", "Owner", "protected User Owner { get; set; }")]
    [InlineData("RefDocGen.TestingLibrary.User", "FirstName", "public string FirstName { get; internal set; }")]
    [InlineData("RefDocGen.TestingLibrary.User", "LastName", "public string LastName { get; init; }")]
    [InlineData(
        "RefDocGen.TestingLibrary.Tools.Collections.NonGenericCollection",
        "System.Collections.ICollection.IsSynchronized",
        "bool ICollection.IsSynchronized { get; }")]
    public void PropertyDeclaration_Matches(string pageName, string propertyId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var property = document.GetMemberElement(propertyId);

        string declaration = TypePageTools.GetMemberDeclaration(property);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "Item(System.Int32)", "public T this[int index] { get; private set; }")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "Item(System.Index)", "public T this[Index index] { get; private set; }")]
    public void IndexerDeclaration_Matches(string pageName, string indexerId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var indexer = document.GetMemberElement(indexerId);

        string declaration = TypePageTools.GetMemberDeclaration(indexer);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.User", ".ctor", "public User()")]
    [InlineData("RefDocGen.TestingLibrary.User", ".ctor(System.String,System.Int32)", "public User(string username, int age)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", ".ctor(System.Double,System.Double)", "internal Point(double x, double y)")]
    public void ConstructorDeclaration_Matches(string pageName, string constructorId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var constructor = document.GetMemberElement(constructorId);

        string declaration = TypePageTools.GetMemberDeclaration(constructor);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Equality(RefDocGen.TestingLibrary.Tools.Point,RefDocGen.TestingLibrary.Tools.Point)", "public static bool operator ==(Point point, Point other)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_UnaryNegation(RefDocGen.TestingLibrary.Tools.Point)", "public static Point operator -(Point point)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Explicit(RefDocGen.TestingLibrary.Tools.Point)~System.Numerics.Vector2", "public static explicit operator Vector2(Point point)")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_Implicit(System.Numerics.Vector2)~RefDocGen.TestingLibrary.Tools.Point", "public static implicit operator Point(Vector2 vector)")]
    public void OperatorDeclaration_Matches(string pageName, string operatorId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var operatorMember = document.GetMemberElement(operatorId);

        string declaration = TypePageTools.GetMemberDeclaration(operatorMember);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", "OnTemperatureChange", "public event Action OnTemperatureChange")]
    public void EventDeclaration_Matches(string pageName, string eventId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var eventMember = document.GetMemberElement(eventId);

        string declaration = TypePageTools.GetMemberDeclaration(eventMember);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Theory]
    [InlineData("Spring", "Spring = 0")]
    [InlineData("Winter", "Winter = 3")]
    public void EnumMemberDeclaration_Matches(string enumMemberId, string expectedDeclaration)
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.Season.html");

        var enumMember = document.GetMemberElement(enumMemberId);

        string declaration = TypePageTools.GetMemberDeclaration(enumMember);
        declaration.ShouldBe(expectedDeclaration);
    }

    [Fact]
    public void DelegateMethodDeclaration_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.Tools.MyPredicate-1.html");

        var delegateMethod = document.DocumentElement.GetByDataId(DataId.DelegateMethod);

        string declaration = TypePageTools.GetMemberDeclaration(delegateMethod);
        declaration.ShouldBe("internal delegate bool MyPredicate<T>(T obj)");
    }
}
