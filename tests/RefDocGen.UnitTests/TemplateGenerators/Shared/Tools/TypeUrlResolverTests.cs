using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.TemplateGenerators.Shared.Tools;
using Shouldly;

namespace RefDocGen.UnitTests.TemplateGenerators.Shared.Tools;

/// <summary>
/// Class containing tests for <see cref="TypeUrlResolver"/> class.
/// </summary>
public class TypeUrlResolverTests
{
    /// <summary>
    /// Resolver of the type pages URLs.
    /// </summary>
    private readonly TypeUrlResolver typeUrlResolver;

    /// <summary>
    /// IDs of the types contained in the type registry.
    /// </summary>
    private static readonly string[] declaredTypeIds = ["MyApp.Person", "MyApp.Dictionary`2"];

    public TypeUrlResolverTests()
    {
        var typeRegistry = Substitute.For<ITypeRegistry>();

        var person = Substitute.For<ITypeDeclaration>();
        var dictionary = Substitute.For<ITypeDeclaration>();

        typeRegistry.GetDeclaredType("MyApp.Person")
            .Returns(person);

        typeRegistry.GetDeclaredType("MyApp.Dictionary`2")
            .Returns(dictionary);

        typeRegistry.GetDeclaredType(Arg.Is<string>(t => !declaredTypeIds.Contains(t)))
            .ReturnsNull();

        typeUrlResolver = new TypeUrlResolver(typeRegistry);
    }


    [Theory]
    [InlineData("MyApp.Person", "./MyApp.Person.html")]
    [InlineData("MyApp.Dictionary`2", "./MyApp.Dictionary%602.html")]
    public void GetUrlOf_ReturnsCorrectData_ForDeclaredType(string typeId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId);

        result.ShouldBe(expectedUrl);
    }

    [Theory]
    [InlineData("MyApp.Person", "Name", "./MyApp.Person.html#Name")]
    [InlineData("MyApp.Dictionary`2", "Add(System.String,`0)", "./MyApp.Dictionary%602.html#Add%28System.String%2C%600%29")]
    public void GetUrlOf_ReturnsCorrectData_ForDeclaredTypeAndMember(string typeId, string memberId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId, memberId);

        result.ShouldBe(expectedUrl);
    }

    [Theory]
    [InlineData("System.String", "https://learn.microsoft.com/dotnet/api/system.string")]
    [InlineData("System.Collections.Generic.Dictionary`2", "https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2")]
    public void GetUrlOf_ReturnsCorrectData_ForSystemType(string typeId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId);

        result.ShouldBe(expectedUrl);
    }

    [Theory]
    [InlineData("System.String", "ToLower", "https://learn.microsoft.com/dotnet/api/system.string.tolower")]
    [InlineData("System.Collections.Generic.Dictionary`2", "Add(`0,`1)", "https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.add")]
    public void GetUrlOf_ReturnsCorrectData_ForSystemTypeAndMember(string typeId, string memberId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId, memberId);

        result.ShouldBe(expectedUrl);
    }
}
