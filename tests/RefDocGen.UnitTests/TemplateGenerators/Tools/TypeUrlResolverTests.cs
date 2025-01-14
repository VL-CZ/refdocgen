using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Tools;

namespace RefDocGen.UnitTests.TemplateGenerators.Tools;

/// <summary>
/// Class responsible for resolving URL of the type's documentation page.
/// </summary>
/// <remarks>
/// Creates links to both same assembly and standard library types.
/// </remarks>
public class TypeUrlResolverTests
{
    private readonly TypeUrlResolver typeUrlResolver;
    private static readonly string[] typeId = new[] { "MyApp.Person", "MyApp.Dictionary`2" };

    public TypeUrlResolverTests()
    {
        var typeRegistry = Substitute.For<ITypeRegistry>();

        var person = Substitute.For<ITypeDeclaration>();
        var animal = Substitute.For<ITypeDeclaration>();

        typeRegistry.GetDeclaredType("MyApp.Person")
            .Returns(person);

        typeRegistry.GetDeclaredType("MyApp.Dictionary`2")
            .Returns(animal);

        typeRegistry.GetDeclaredType(Arg.Is<string>(a => !typeId.Contains(a)))
            .ReturnsNull();

        typeUrlResolver = new TypeUrlResolver(typeRegistry);
    }


    [Theory]
    [InlineData("MyApp.Person", "./MyApp.Person.html")]
    [InlineData("MyApp.Dictionary`2", "./MyApp.Dictionary`2.html")]
    public void GetUrlOf_ReturnsCorrectData_ForDeclaredType(string typeId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId);

        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("MyApp.Person", "Name", "./MyApp.Person.html#Name")]
    [InlineData("MyApp.Dictionary`2", "Add(System.String,`0)", "./MyApp.Dictionary`2.html#Add(System.String,`0)")]
    public void GetUrlOf_ReturnsCorrectData_ForDeclaredTypeAndMember(string typeId, string memberId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId, memberId);

        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("System.String", "https://learn.microsoft.com/dotnet/api/system.string")]
    [InlineData("System.Collections.Generic.Dictionary`2", "https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2")]
    public void GetUrlOf_ReturnsCorrectData_ForSystemType(string typeId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId);

        result.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("System.String", "ToLower", "https://learn.microsoft.com/dotnet/api/system.string.tolower")]
    [InlineData("System.Collections.Generic.Dictionary`2", "Add(`0,`1)", "https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.add")]
    public void GetUrlOf_ReturnsCorrectData_ForSystemTypeAndMember(string typeId, string memberId, string expectedUrl)
    {
        string? result = typeUrlResolver.GetUrlOf(typeId, memberId);

        result.Should().Be(expectedUrl);
    }
}
