using FluentAssertions;
using RefDocGen.Tools;

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

    public void Merge_ReturnsCorrectData_WhenNoDuplicateKeys()
    {

        var expectedResult = new Dictionary<int, string>()
        {
            [0] = "a",
            [1] = "b",
            [2] = "c",
            [3] = "d",
        };

        var result = DictionaryExtensions.Merge(dictionary1, dictionary2);

        result.Should().BeEquivalentTo(expectedResult);
    }

    public void Merge_ReturnsCorrectData_ForDuplicateKeys()
    {
        dictionary1[3] = "newValue"; // add the value

        var expectedResult = new Dictionary<int, string>()
        {
            [0] = "a",
            [1] = "b",
            [2] = "c",
            [3] = "newValue",
        };

        var result = DictionaryExtensions.Merge(dictionary1, dictionary2);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
