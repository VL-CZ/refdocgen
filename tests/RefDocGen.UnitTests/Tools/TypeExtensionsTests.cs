using FluentAssertions;
using TypeExtensions = RefDocGen.Tools.TypeExtensions;

namespace RefDocGen.UnitTests.Tools;

/// <summary>
/// Class containing tests for <see cref="TypeExtensions"/> class.
/// </summary>
public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(object))]
    [InlineData(typeof(TypeExtensionsTests))]
    public void GetBaseElementType_ReturnsTheSameType_WhenNoElementType(Type type)
    {
        var result = TypeExtensions.GetBaseElementType(type);

        result.Should().Be(type);
    }

    [Theory]
    [InlineData(typeof(string[]), typeof(string))]
    [InlineData(typeof(object[][]), typeof(object))]
    [InlineData(typeof(double[][][]), typeof(double))]
    [InlineData(typeof(int[,,]), typeof(int))]
    public void GetBaseElementType_ReturnsBaseElementType_ForArrayType(Type inputType, Type expectedResultType)
    {
        var result = TypeExtensions.GetBaseElementType(inputType);

        result.Should().Be(expectedResultType);
    }

    [Theory]
    [InlineData(typeof(int*), typeof(int))]
    [InlineData(typeof(double**), typeof(double))]
    public void GetBaseElementType_ReturnsBaseElementType_ForPointerType(Type inputType, Type expectedResultType)
    {
        var result = TypeExtensions.GetBaseElementType(inputType);

        result.Should().Be(expectedResultType);
    }
}
