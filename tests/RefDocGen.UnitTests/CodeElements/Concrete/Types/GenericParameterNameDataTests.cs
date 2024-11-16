using FluentAssertions;
using NSubstitute;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.UnitTests.CodeElements.Concrete.Types;

/// <summary>
/// Class containing tests for <see cref="GenericTypeParameterNameData"/> class.
/// </summary>
public class GenericTypeParameterNameDataTests
{
    /// <summary>
    /// Helper class providing generic type parameters for testing.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    private class Helper<TKey, TValue> { }

    /// <summary>
    /// Collection of type parameters declared in the containing type; the keys represent type parameter names.
    /// <para>
    /// These data should be passed to <see cref="GenericTypeParameterNameData"/> constructor.
    /// </para>
    /// </summary>
    private readonly IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters;

    public GenericTypeParameterNameDataTests()
    {
        declaredTypeParameters = new Dictionary<string, TypeParameterDeclaration>()
        {
            ["TKey"] = new TypeParameterDeclaration("TKey", 0),
            ["TValue"] = new TypeParameterDeclaration("TValue", 1)
        };
    }

    [Fact]
    public void Id_ReturnsCorrectData_WhenNoElementType()
    {
        var typeMock = Substitute.For<Type>();
        typeMock.Name.Returns("TValue");

        var gp = new GenericTypeParameterNameData(typeMock, declaredTypeParameters);

        string expectedId = "`1";

        gp.Id.Should().Be(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForArrayType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakeArrayType(); // get 'TKey[]' type.

        var gp = new GenericTypeParameterNameData(type, declaredTypeParameters);

        string expectedId = "`0[]";

        gp.Id.Should().Be(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForPointerType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakePointerType(); // get 'TKey*' type.

        var gp = new GenericTypeParameterNameData(type, declaredTypeParameters);

        string expectedId = "`0*";

        gp.Id.Should().Be(expectedId);
    }
}
