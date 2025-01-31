using NSubstitute;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Tools;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using Shouldly;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;

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
        tValueMock.Name.Returns("TMethodType");

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
        var typeMock = MockNonGenericType(typeNamespace, typeShortName);

        var type = new TypeNameData(typeMock);

        type.Id.ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForTypeWithTypeParameters()
    {
        var innerInnerTypeMock = MockNonGenericType("MyApp.Entities", "Person");

        var innerType2Mock = MockType("System.Collections.Generic", "List`1", [innerInnerTypeMock]);
        var innerType1Mock = MockNonGenericType("System", "String");

        var typeMock = MockType("System.Collection.Generic", "Dictionary`2", [innerType1Mock, innerType2Mock]);

        var typeData = new TypeNameData(typeMock);
        string expectedId = "System.Collection.Generic.Dictionary{System.String,System.Collections.Generic.List{MyApp.Entities.Person}}";

        typeData.Id.ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForTypeWithGenericTypeParameter()
    {
        var genericParamMock = Substitute.For<Type>();
        genericParamMock.Name.Returns("TKey");
        genericParamMock.IsGenericParameter.Returns(true);

        var genericParam = new GenericTypeParameterNameData(genericParamMock, availableTypeParameters);
        var type = MockType("System.Collection.Generic", "List`1", [genericParam]);

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

    /// <summary>
    /// Mock <see cref="ITypeNameData"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="fullName">Fully qualified name of the type.</param>
    /// <param name="genericParameters">Generic parameters of the type.</param>
    /// <returns>Mocked <see cref="ITypeNameData"/> instance.</returns>
    private ITypeNameData MockTypeNameData(string fullName, ITypeNameData[] genericParameters)
    {
        var type = Substitute.For<ITypeNameData>();

        type.FullName.Returns(fullName);
        type.TypeParameters.Returns(genericParameters);
        type.HasTypeParameters.Returns(genericParameters.Length > 0);

        return type;
    }

    /// <summary>
    /// Mock <see cref="ITypeNameData"/> instance with no type parameters and initialize it with the provided data.
    /// </summary>
    /// <param name="fullName">Fully qualified name of the type.</param>
    /// <returns>Mocked <see cref="ITypeNameData"/> instance.</returns>
    private ITypeNameData MockTypeNameData(string fullName)
    {
        return MockTypeNameData(fullName, []);
    }
}
