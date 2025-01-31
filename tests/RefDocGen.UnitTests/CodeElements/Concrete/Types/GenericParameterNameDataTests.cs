using NSubstitute;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using Shouldly;

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
    /// Collection of type parameters available; the keys represent type parameter names.
    /// <para>
    /// These data should be passed to <see cref="GenericTypeParameterNameData"/> constructor.
    /// </para>
    /// </summary>
    private readonly IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters;

    public GenericTypeParameterNameDataTests()
    {
        var tKeyMock = Substitute.For<Type>();
        tKeyMock.Name.Returns("TKey");

        var tValueMock = Substitute.For<Type>();
        tValueMock.Name.Returns("TValue");

        var tMethodTypeMock = Substitute.For<Type>();
        tValueMock.Name.Returns("TMethodType");

        availableTypeParameters = new Dictionary<string, TypeParameterData>()
        {
            ["TKey"] = new TypeParameterData(tKeyMock, 0, CodeElementKind.Type),
            ["TValue"] = new TypeParameterData(tValueMock, 1, CodeElementKind.Type),
            ["TMethodType"] = new TypeParameterData(tMethodTypeMock, 0, CodeElementKind.Member) // generic type declared in a member
        };
    }

    [Fact]
    public void Id_ReturnsCorrectData_WhenNoElementType()
    {
        var typeMock = Substitute.For<Type>();
        typeMock.Name.Returns("TValue");

        var gp = new GenericTypeParameterNameData(typeMock, availableTypeParameters);

        string expectedId = "`1";

        gp.Id.ShouldBe(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForArrayType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakeArrayType(); // get 'TKey[]' type.

        var gp = new GenericTypeParameterNameData(type, availableTypeParameters);

        string expectedId = "`0[]";

        gp.Id.ShouldBe(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForPointerType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakePointerType(); // get 'TKey*' type.

        var gp = new GenericTypeParameterNameData(type, availableTypeParameters);

        string expectedId = "`0*";

        gp.Id.ShouldBe(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForTypeDeclaredInMember()
    {
        var typeMock = Substitute.For<Type>();
        typeMock.Name.Returns("TMethodType");

        var gp = new GenericTypeParameterNameData(typeMock, availableTypeParameters);

        string expectedId = "``0";

        gp.Id.ShouldBe(expectedId);
    }
}
