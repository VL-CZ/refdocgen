using FluentAssertions;
using NSubstitute;
using RefDocGen.MemberData.Abstract;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.UnitTests.TemplateGenerators.Tools.TypeName;

public class CSharpTypeNameTests
{
    [Theory]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(string), "string")]
    public void GetBuiltInName_ReturnsBuiltInName_WhenPresent(Type type, string builtInName)
    {
        var typeData = Substitute.For<ITypeNameData>();
        typeData.TypeObject.Returns(type);

        string? name = CSharpTypeName.GetBuiltInName(typeData);

        name.Should().Be(builtInName);
    }

    [Theory]
    [InlineData(typeof(Type))]
    [InlineData(typeof(ITypeNameData))]
    [InlineData(typeof(Type[]))]
    [InlineData(typeof(Dictionary<string, int>))]
    public void GetBuiltInName_ReturnsNull_WhenThereIsNoBuiltInName(Type type)
    {
        var typeData = Substitute.For<ITypeNameData>();
        typeData.TypeObject.Returns(type);

        string? name = CSharpTypeName.GetBuiltInName(typeData);

        name.Should().BeNull();
    }

    [Theory]
    [InlineData(typeof(int[]), "Int32[]", "int[]")]
    [InlineData(typeof(double[][]), "Double[][]", "double[][]")]
    [InlineData(typeof(byte[,,]), "Byte[,,]", "byte[,,]")]
    public void GetBuiltInName_ReturnsBuiltInName_ForArray(Type type, string shortName, string expectedName)
    {
        var typeData = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(type);
        typeData.IsArray.Returns(true);
        typeData.ShortName.Returns(shortName);

        string? name = CSharpTypeName.GetBuiltInName(typeData);

        name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData(typeof(Directory), "Directory", "Directory", false)]
    [InlineData(typeof(double), "Double", "double", false)]
    [InlineData(typeof(int[]), "Int32[]", "int[]", true)]
    [InlineData(typeof(byte[][]), "Byte[][]", "byte[][]", true)]
    public void Of_ReturnsCorrectName_ForNonGenericType(Type type, string shortName, string expectedName, bool isArray)
    {
        var typeData = InitTypeData(type, shortName, [], isArray);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be(expectedName);
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForSimpleGenericType()
    {
        var param = InitTypeData(typeof(int), "Int32", []);
        var typeData = InitTypeData(typeof(List<int>), "List", [param]);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be("List<int>");
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForComplexGenericType()
    {
        var innerInner = InitTypeData(typeof(FileInfo), "FileInfo", []);
        var inner1 = InitTypeData(typeof(string), "String", []);
        var inner2 = InitTypeData(typeof(List<FileInfo>), "List", [innerInner]);
        var typeData = InitTypeData(typeof(Dictionary<string, List<FileInfo>>), "Dictionary", [inner1, inner2]);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be("Dictionary<string, List<FileInfo>>");
    }

    private ITypeNameData InitTypeData(Type type, string shortName, IReadOnlyList<ITypeNameData> genericParams, bool isArray = false)
    {
        var typeData = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(type);
        typeData.ShortName.Returns(shortName);
        typeData.HasGenericParameters.Returns(genericParams.Any());
        typeData.GenericParameters.Returns(genericParams);
        typeData.IsArray.Returns(isArray);
        typeData.IsPointer.Returns(false);

        return typeData;
    }
}
