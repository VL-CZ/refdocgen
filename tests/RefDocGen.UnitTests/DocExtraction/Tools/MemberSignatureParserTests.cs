using FluentAssertions;
using RefDocGen.DocExtraction.Tools;

namespace RefDocGen.UnitTests.DocExtraction.Tools;

/// <summary>
/// Class containing tests for <see cref="MemberSignatureParser"/> class.
/// </summary>
public class MemberSignatureParserTests
{
    [Fact]
    public void Parse_ReturnsCorrectData_ForMemberWithNoParameters()
    {
        string memberString = "MyLibrary.Animal.weight";

        var result = MemberSignatureParser.Parse(memberString);

        result.Should().BeEquivalentTo(("MyLibrary.Animal", "weight", ""));
    }

    [Fact]
    public void Parse_ReturnsCorrectData_ForMethodWithParameters()
    {
        string memberString = "MyLibrary.Animal.GenerateAnimalProfile(System.String,System.String,System.DateTime)";

        var result = MemberSignatureParser.Parse(memberString);

        result.Should().BeEquivalentTo(("MyLibrary.Animal", "GenerateAnimalProfile", "(System.String,System.String,System.DateTime)"));
    }
}
