using NSubstitute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;
using Shouldly;

namespace RefDocGen.UnitTests.TemplateGenerators.Shared.Tools.Names;

/// <summary>
/// Class containing tests for <see cref="CSharpTypeName"/> class.
/// </summary>
public class CSharpTypeNameTests
{
    [Theory]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(string), "string")]
    public void GetBuiltInName_ReturnsBuiltInName_WhenPresent(Type type, string expectedName)
    {
        var typeData = Substitute.For<ITypeNameData>();
        typeData.TypeObject.Returns(type);

        string? name = CSharpTypeName.GetBuiltInName(typeData);

        name.ShouldBe(expectedName);
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

        name.ShouldBeNull();
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

        name.ShouldBe(expectedName);
    }

    [Theory]
    [InlineData(typeof(Directory), "Directory", "Directory", false)]
    [InlineData(typeof(double), "Double", "double", false)]
    [InlineData(typeof(int[]), "Int32[]", "int[]", true)]
    [InlineData(typeof(byte[][]), "Byte[][]", "byte[][]", true)]
    public void Of_ReturnsCorrectName_ForNonGenericType(Type type, string shortName, string expectedName, bool isArray)
    {
        var typeData = MockTypeData(type, shortName, [], isArray);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.ShouldBe(expectedName);
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForSimpleGenericType()
    {
        var param = MockTypeData(typeof(int), "Int32", []);
        var typeData = MockTypeData(typeof(List<int>), "List", [param]);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.ShouldBe("List<int>");
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForComplexGenericType()
    {
        var innerInnerType = MockTypeData(typeof(FileInfo), "FileInfo", []);
        var innerType1 = MockTypeData(typeof(string), "String", []);
        var innerType2 = MockTypeData(typeof(List<FileInfo>), "List", [innerInnerType]);
        var typeData = MockTypeData(typeof(Dictionary<string, List<FileInfo>>), "Dictionary", [innerType1, innerType2]);

        string? typeName = CSharpTypeName.Of(typeData);

        typeName.ShouldBe("Dictionary<string, List<FileInfo>>");
    }

    /// <summary>
    /// Mock <see cref="ITypeNameData"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the type to create.</param>
    /// <param name="shortName">Short name of the type (see <see cref="ITypeNameData.ShortName"/>).</param>
    /// <param name="genericParams">Generic parameters of the type.</param>
    /// <param name="isArray">Is the type an array?</param>
    /// <returns>Mocked <see cref="ITypeNameData"/> instance.</returns>
    private ITypeNameData MockTypeData(Type type, string shortName, IReadOnlyList<ITypeNameData> genericParams, bool isArray = false)
    {
        var typeData = Substitute.For<ITypeNameData>();

        typeData.TypeObject.Returns(type);
        typeData.ShortName.Returns(shortName);
        typeData.HasTypeParameters.Returns(genericParams.Any());
        typeData.TypeParameters.Returns(genericParams);
        typeData.IsArray.Returns(isArray);
        typeData.IsPointer.Returns(false);

        return typeData;
    }
}
