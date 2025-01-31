using RefDocGen.Tools;
using Shouldly;

namespace RefDocGen.UnitTests.Tools;

/// <summary>
/// Class containing tests for <see cref="StringExtensions"/> class.
/// </summary>
public class StringExtensionsTests
{
    [Theory]
    [InlineData("12345", '1', 0)]
    [InlineData("John Doe", 'o', 1)]
    public void TryGetIndex_ReturnsCorrectIndex_IfTheValueIsFound(string s, char value, int expectedIndex)
    {
        bool found = StringExtensions.TryGetIndex(s, value, out int index);

        (found, index).ShouldBe((true, expectedIndex));
    }

    [Theory]
    [InlineData("John Doe", 'x')]
    [InlineData("abcde", 'C')]
    public void TryGetIndex_ReturnsMinusOne_IfTheValueIsNotFound(string s, char value)
    {
        bool found = StringExtensions.TryGetIndex(s, value, out int index);

        (found, index).ShouldBe((false, -1));
    }
}
