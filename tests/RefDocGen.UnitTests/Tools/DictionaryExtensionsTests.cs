using RefDocGen.Tools;
using Shouldly;

namespace RefDocGen.UnitTests.Tools;

/// <summary>
/// Class containing tests for <see cref="DictionaryExtensions"/> class.
/// </summary>
public class DictionaryExtensionsTests
{
    private readonly Dictionary<int, string> dictionary1;
    private readonly Dictionary<int, string> dictionary2;

    public DictionaryExtensionsTests()
    {
        dictionary1 = new()
        {
            [0] = "a",
            [1] = "b",
        };

        dictionary2 = new()
        {
            [2] = "c",
            [3] = "d"
        };
    }

    [Fact]
    public void Merge_ReturnsExpectedData_WhenNoDuplicateKeys()
    {
        var expectedResult = new Dictionary<int, string>()
        {
            [0] = "a",
            [1] = "b",
            [2] = "c",
            [3] = "d",
        };

        var result = DictionaryExtensions.Merge(dictionary1, dictionary2);

        result.ShouldBe(expectedResult);
    }

    [Fact]
    public void Merge_ReturnsExpectedData_ForDuplicateKeys()
    {
        dictionary1[3] = "newValue"; // add the value

        var expectedResult = new Dictionary<int, string>()
        {
            [0] = "a",
            [1] = "b",
            [3] = "newValue",
            [2] = "c",
        };

        var result = DictionaryExtensions.Merge(dictionary1, dictionary2);

        result.ShouldBe(expectedResult);
    }
}
