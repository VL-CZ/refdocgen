using NSubstitute;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Tools;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using Shouldly;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.UnitTests.Shared;

namespace RefDocGen.UnitTests.CodeElements.Tools;

/// <summary>
/// Class containing tests for <see cref="TypeId"/> class.
/// </summary>
public class TypeIdTests
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

    public TypeIdTests()
    {
        var tKeyMock = Substitute.For<Type>();
        tKeyMock.Name.Returns("TKey");

        var tValueMock = Substitute.For<Type>();
        tValueMock.Name.Returns("TValue");

        var tMethodTypeMock = Substitute.For<Type>();
        tMethodTypeMock.Name.Returns("TMethodType");

        availableTypeParameters = new Dictionary<string, TypeParameterData>()
        {
            ["TKey"] = new TypeParameterData(tKeyMock, 0, CodeElementKind.Type),
            ["TValue"] = new TypeParameterData(tValueMock, 1, CodeElementKind.Type),
            ["TMethodType"] = new TypeParameterData(tMethodTypeMock, 0, CodeElementKind.Member) // generic type declared in a member
        };
    }

    [Theory]
    [InlineData("MyApp.Entities", "Person", "MyApp.Entities.Person")]
    [InlineData("MyApp.Entities", "Person[]", "MyApp.Entities.Person[]")]
    public void Of_ReturnsCorrectData_ForNonGenericType(string typeNamespace, string typeShortName, string expectedId)
    {
        var typeMock = MockHelper.MockNonGenericType(typeNamespace, typeShortName);

        var type = new TypeNameData(typeMock);

        TypeId.Of(type).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForTypeWithTypeParameters()
    {
        var innerInnerTypeMock = MockHelper.MockNonGenericType("MyApp.Entities", "Person");

        var innerType2Mock = MockHelper.MockType("System.Collections.Generic", "List`1", [innerInnerTypeMock]);
        var innerType1Mock = MockHelper.MockNonGenericType("System", "String");

        var typeMock = MockHelper.MockType("System.Collection.Generic", "Dictionary`2", [innerType1Mock, innerType2Mock]);

        var type = new TypeNameData(typeMock);
        string expectedId = "System.Collection.Generic.Dictionary{System.String,System.Collections.Generic.List{MyApp.Entities.Person}}";

        TypeId.Of(type).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForTypeWithGenericTypeParameter()
    {
        var genericParamMock = Substitute.For<Type>();
        genericParamMock.Name.Returns("TKey");
        genericParamMock.IsGenericParameter.Returns(true);

        var typeMock = MockHelper.MockType("System.Collection.Generic", "List`1", [genericParamMock]);

        var type = new TypeNameData(typeMock, availableTypeParameters);
        string expectedId = "System.Collection.Generic.List{`0}";

        TypeId.Of(type).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForGenericParameterType()
    {
        var typeMock = Substitute.For<Type>();
        typeMock.Name.Returns("TValue");

        var gp = new GenericTypeParameterNameData(typeMock, availableTypeParameters);

        string expectedId = "`1";

        TypeId.Of(gp).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForGenericParameterOfArrayType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakeArrayType(); // get 'TKey[]' type.

        var gp = new GenericTypeParameterNameData(type, availableTypeParameters);

        string expectedId = "`0[]";

        TypeId.Of(gp).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForGenericParameterOfPointerType()
    {
        var type = typeof(Helper<,>).GetGenericArguments()[0].MakePointerType(); // get 'TKey*' type.

        var gp = new GenericTypeParameterNameData(type, availableTypeParameters);

        string expectedId = "`0*";

        TypeId.Of(gp).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForGenericParameterDeclaredInAMember()
    {
        var typeMock = Substitute.For<Type>();
        typeMock.Name.Returns("TMethodType");

        var gp = new GenericTypeParameterNameData(typeMock, availableTypeParameters);

        string expectedId = "``0";

        TypeId.Of(gp).ShouldBe(expectedId);
    }
}
