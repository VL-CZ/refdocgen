using RefDocGen.CodeElements.Types.Concrete.TypeName;
using RefDocGen.UnitTests.Shared;
using Shouldly;

namespace RefDocGen.UnitTests.CodeElements.Types.Concrete.TypeName;

/// <summary>
/// Class containing tests for <see cref="TypeNameData"/> class.
/// </summary>
public class TypeNameDataTests
{
    [Fact]
    public void ShortName_ReturnsTypeName_ForNonGenericType()
    {
        var mock = MockHelper.MockNonGenericType("MyApp.Entities", "Person");

        var typeData = new TypeNameData(mock);

        typeData.ShortName.ShouldBe("Person");
    }

    [Fact]
    public void ShortName_ReturnsTypeNameWithoutGenericParameters_ForGenericType()
    {
        var paramMock = MockHelper.MockNonGenericType("System", "Int32");
        var typeMock = MockHelper.MockType("System.Collections.Generic", "List`1", [paramMock]);

        var typeData = new TypeNameData(typeMock);

        typeData.ShortName.ShouldBe("List");
    }

    [Fact]
    public void FullName_ReturnsExpectedData_ForNonGenericType()
    {
        var mock = MockHelper.MockNonGenericType("MyApp.Entities", "Person");

        var typeData = new TypeNameData(mock);

        typeData.FullName.ShouldBe("MyApp.Entities.Person");
    }

    [Fact]
    public void FullName_ReturnsFullyQualifiedNameWithoutGenericParameters_ForGenericType()
    {
        var param1Mock = MockHelper.MockNonGenericType("System", "Int32");
        var param2Mock = MockHelper.MockNonGenericType("System", "String");

        var typeMock = MockHelper.MockType("System.Collections.Generic", "Dictionary`2", [param1Mock, param2Mock]);

        var typeData = new TypeNameData(typeMock);

        typeData.FullName.ShouldBe("System.Collections.Generic.Dictionary");
    }
}
