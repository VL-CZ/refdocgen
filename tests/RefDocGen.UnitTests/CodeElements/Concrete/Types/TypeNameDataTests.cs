using RefDocGen.CodeElements.Concrete.Types.TypeName;
using Shouldly;

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

        typeData.ShortName.ShouldBe("Person");
    }

    [Fact]
    public void ShortName_ReturnsCorrectData_ForGenericType()
    {
        var paramMock = MockNonGenericType("System", "Int32");
        var typeMock = MockType("System.Collections.Generic", "List`1", [paramMock]);

        var typeData = new TypeNameData(typeMock);

        typeData.ShortName.ShouldBe("List");
    }

    [Fact]
    public void FullName_ReturnsCorrectData_ForNonGenericType()
    {
        var mock = MockNonGenericType("MyApp.Entities", "Person");

        var typeData = new TypeNameData(mock);

        typeData.FullName.ShouldBe("MyApp.Entities.Person");
    }

    [Fact]
    public void FullName_ReturnsCorrectData_ForGenericType()
    {
        var param1Mock = MockNonGenericType("System", "Int32");
        var param2Mock = MockNonGenericType("System", "String");

        var typeMock = MockType("System.Collections.Generic", "Dictionary`2", [param1Mock, param2Mock]);

        var typeData = new TypeNameData(typeMock);

        typeData.FullName.ShouldBe("System.Collections.Generic.Dictionary");
    }
}
