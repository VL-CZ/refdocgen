using RefDocGen.Tools;
using Shouldly;

namespace RefDocGen.UnitTests.Tools;

/// <summary>
/// Class containing tests for <see cref="StringArrayExtensions"/> class.
/// </summary>
public class StringArrayExtensionsTests
{
    [Fact]
    public void Dedent_RemovesCommonLeadingWhitespace_FromAllLines()
    {
        string[] input =
        [
            "    line1",
            "    line2",
            "    line3"
        ];

        string[] expected =
        [
            "line1",
            "line2",
            "line3"
        ];

        input.Dedent().ShouldBe(expected);
    }

    [Fact]
    public void Dedent_KeepsRelativeIndentation_WhenLinesHaveMixedIndent()
    {
        string[] input =
        [
            "    line1",
            "        line2",
            "    line3"
        ];

        string[] expected =
        [
            "line1",
            "    line2",
            "line3"
        ];

        input.Dedent().ShouldBe(expected);
    }

    [Fact]
    public void Dedent_HandlesLinesShorterThanMinimumIndent()
    {
        string[] input =
        [
            "    line1",
            "    ",
            "    line2",
            ""
        ];

        string[] expected =
        [
            "line1",
            "    ",
            "line2",
            ""
        ];

        input.Dedent().ShouldBe(expected);
    }

    [Fact]
    public void Dedent_ReturnsEmptyArray_WhenInputIsEmpty()
    {
        string[] input = [];

        input.Dedent().ShouldBeEmpty();
    }

    [Fact]
    public void Dedent_ReturnsSameArray_WhenNoCommonIndentExists()
    {
        string[] input =
        [
            "line1",
            "  line2",
            "line3"
        ];

        string[] expected =
        [
            "line1",
            "  line2",
            "line3"
        ];

        input.Dedent().ShouldBe(expected);
    }
}
