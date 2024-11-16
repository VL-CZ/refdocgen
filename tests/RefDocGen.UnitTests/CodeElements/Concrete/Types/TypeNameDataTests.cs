using FluentAssertions;
using NSubstitute;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.UnitTests.CodeElements.Concrete.Types;

/// <summary>
/// Class containing tests for <see cref="TypeNameData"/> class.
/// </summary>
public class TypeNameDataTests
{
    [Fact]
    public void ShortName_ReturnsCorrectData_ForNonGenericType()
    {
        var mock = MockNonGenericType("MyApp.Entities", "Person");

        var typeData = new TypeNameData(mock);

        typeData.ShortName.Should().Be("Person");
    }

    [Fact]
    public void ShortName_ReturnsCorrectData_ForGenericType()
    {
        var paramMock = MockNonGenericType("System", "Int32");
        var typeMock = MockType("System.Collections.Generic", "List`1", [paramMock]);

        var typeData = new TypeNameData(typeMock);

        typeData.ShortName.Should().Be("List");
    }

    [Fact]
    public void FullName_ReturnsCorrectData_ForNonGenericType()
    {
        var mock = MockNonGenericType("MyApp.Entities", "Person");

        var typeData = new TypeNameData(mock);

        typeData.FullName.Should().Be("MyApp.Entities.Person");
    }

    [Fact]
    public void FullName_ReturnsCorrectData_ForGenericType()
    {
        var param1Mock = MockNonGenericType("System", "Int32");
        var param2Mock = MockNonGenericType("System", "String");

        var typeMock = MockType("System.Collections.Generic", "Dictionary`2", [param1Mock, param2Mock]);

        var typeData = new TypeNameData(typeMock);

        typeData.FullName.Should().Be("System.Collections.Generic.Dictionary");
    }

    [Theory]
    [InlineData("MyApp.Entities", "Person", "MyApp.Entities.Person")]
    [InlineData("MyApp.Entities", "Person[]", "MyApp.Entities.Person[]")]
    public void Id_ReturnsCorrectData_ForNonGenericType(string typeNamespace, string typeShortName, string expectedId)
    {
        var typeMock = MockNonGenericType(typeNamespace, typeShortName);

        var type = new TypeNameData(typeMock);

        type.Id.Should().Be(expectedId);
    }

    [Fact]
    public void Id_ReturnsCorrectData_ForGenericType()
    {
        var innerInnerTypeMock = MockNonGenericType("MyApp.Entities", "Person");

        var innerType2Mock = MockType("System.Collections.Generic", "List`1", [innerInnerTypeMock]);
        var innerType1Mock = MockNonGenericType("System", "String");

        var typeMock = MockType("System.Collection.Generic", "Dictionary`2", [innerType1Mock, innerType2Mock]);

        var typeData = new TypeNameData(typeMock);
        string expectedId = "System.Collection.Generic.Dictionary{System.String,System.Collections.Generic.List{MyApp.Entities.Person}}";

        typeData.Id.Should().Be(expectedId);
    }

    /// <summary>
    /// Mock <see cref="Type"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="typeNamespace">Namespace containing the type.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="genericParameters">Generic parameters of the type.</param>
    /// <returns>Mocked <see cref="Type"/> instance.</returns>
    private Type MockType(string typeNamespace, string typeName, Type[] genericParameters)
    {
        var type = Substitute.For<Type>();

        type.Name.Returns(typeName);
        type.Namespace.Returns(typeNamespace);
        type.GetGenericArguments().Returns(genericParameters);
        type.IsGenericType.Returns(genericParameters.Length > 0);

        return type;
    }

    /// <summary>
    /// Mock non-generic <see cref="Type"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="typeNamespace">Namespace of the type</param>
    /// <param name="typeName">Name of the type.</param>
    /// <returns>Mocked <see cref="Type"/> instance.</returns>
    private Type MockNonGenericType(string typeNamespace, string typeName)
    {
        return MockType(typeNamespace, typeName, []);
    }
}
