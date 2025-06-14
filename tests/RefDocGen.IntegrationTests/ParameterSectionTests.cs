using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the 'Parameters' section - i.e. the individual parameter declarations and doc comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class ParameterSectionTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Animal", "GetAverageLifespan(System.String)", "string species", "The species of the animal.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "op_UnaryNegation(RefDocGen.TestingLibrary.Tools.Point)", "Point point", "The provided point.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.WeatherStation", ".ctor(RefDocGen.TestingLibrary.Tools.Point)", "Point location", "Location of the weather station.")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyCollection-1", "Item(System.Index)", "Index index", "An Index struct.")]
    [InlineData(
        "RefDocGen.TestingLibrary.User",
        "AddAnimalsByType(System.Collections.Generic.Dictionary(System.String,System.Collections.Generic.List(RefDocGen.TestingLibrary.Animal)))",
        "Dictionary<string, List<Animal>> animals",
        "Animals to add. Key: animal type, Value: list of animals of the given type.")]
    public void Section_WithSingleParameter_Matches(string pageName, string memberId, string parameterDeclaration, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var parameters = TypePageTools.GetMemberParameters(document.GetMemberElement(memberId));

        parameters.Length.ShouldBe(1);

        string paramDeclaration = TypePageTools.GetParameterName(parameters[0]);
        paramDeclaration.ShouldBe(parameterDeclaration);

        string paramDoc = TypePageTools.GetParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Section_WithMultipleParameter_Matches()
    {
        using var document = DocumentationTools.GetApiPage("RefDocGen.TestingLibrary.User.html");
        var memberElement = document.GetMemberElement("ProcessValues(System.Int32-,System.Int32-,System.String,System.Int32-,System.Double)");

        var parameters = TypePageTools.GetMemberParameters(memberElement);

        parameters.Length.ShouldBe(5);

        (string declaration, string doc)[] expectedValues = [
            ("in int inValue", "An input value."),
            ("ref int refValue", "A reference value."),
            ("string s1", ""),
            ("out int outValue", "An output value."),
            ("double d2", "")
        ];

        for (int i = 0; i < expectedValues.Length; i++)
        {
            string paramName = TypePageTools.GetParameterName(parameters[i]);
            paramName.ShouldBe(expectedValues[i].declaration);

            string paramDoc = TypePageTools.GetParameterDoc(parameters[i]);
            paramDoc.ShouldBe(expectedValues[i].doc);
        }
    }
}
