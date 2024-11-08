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
        var typeData = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(type);
        typeData.ShortName.Returns(shortName);
        typeData.HasGenericParameters.Returns(false);
        typeData.IsArray.Returns(isArray);
        typeData.IsPointer.Returns(false);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be(expectedName);
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForSimpleGenericType()
    {
        var typeData = Substitute.For<ITypeNameData>();
        var param = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(typeof(List<int>));
        typeData.ShortName.Returns("List");
        typeData.HasGenericParameters.Returns(true);
        typeData.GenericParameters.Returns([param]);
        typeData.IsPointer.Returns(false);
        typeData.IsArray.Returns(false);

        param.TypeObject.Returns(typeof(int));
        param.ShortName.Returns("Int32");
        param.HasGenericParameters.Returns(false);
        param.IsPointer.Returns(false);
        param.IsArray.Returns(false);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be("List<int>");
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForComplexGenericType()
    {
        var typeData = Substitute.For<ITypeNameData>();
        var param1 = Substitute.For<ITypeNameData>();
        var param2 = Substitute.For<ITypeNameData>();
        var param3 = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(typeof(Dictionary<string, List<FileInfo>>));
        typeData.ShortName.Returns("Dictionary");
        typeData.HasGenericParameters.Returns(true);
        typeData.GenericParameters.Returns([param1, param2]);
        typeData.IsPointer.Returns(false);
        typeData.IsArray.Returns(false);

        param1.TypeObject.Returns(typeof(string));
        param1.ShortName.Returns("String");
        param1.HasGenericParameters.Returns(false);
        param1.IsPointer.Returns(false);
        param1.IsArray.Returns(false);

        param2.TypeObject.Returns(typeof(List<FileInfo>));
        param2.ShortName.Returns("List");
        param2.HasGenericParameters.Returns(true);
        param2.GenericParameters.Returns([param3]);
        param2.IsPointer.Returns(false);
        param2.IsArray.Returns(false);

        param3.TypeObject.Returns(typeof(FileInfo));
        param3.ShortName.Returns("FileInfo");
        param3.HasGenericParameters.Returns(false);
        param3.IsPointer.Returns(false);
        param3.IsArray.Returns(false);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.Should().Be("Dictionary<string, List<FileInfo>>");
    }
}
