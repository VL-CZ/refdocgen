using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for the 'Parameters' section - i.e. the individual parameter signatures and doc comments.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class ParameterSectionTests
{
    [Theory]
    [InlineData("MyLibrary.Animal", "GetAverageLifespan(System.String)", "string species", "The species of the animal.")]
    [InlineData("MyLibrary.Tools.Point", "op_UnaryNegation(MyLibrary.Tools.Point)", "Point point", "The provided point.")]
    [InlineData("MyLibrary.Tools.WeatherStation", ".ctor(MyLibrary.Tools.Point)", "Point location", "Location of the weather station.")]
    [InlineData("MyLibrary.Tools.Collections.MyCollection-1", "Item(System.Index)", "Index index", "An Index struct.")]
    [InlineData(
        "MyLibrary.User",
        "AddAnimalsByType(System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.List{MyLibrary.Animal}})",
        "Dictionary<string, List<Animal>> animals",
        "Animals to add. Key: animal type, Value: list of animals of the given type.")]
    public void Section_WithSingleParameter_Matches(string pageName, string memberId, string parameterSignature, string expectedDoc)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        var parameters = TypePageTools.GetMemberParameters(document.GetMemberElement(memberId));

        parameters.Length.ShouldBe(1);

        string paramSignature = TypePageTools.GetParameterName(parameters[0]);
        paramSignature.ShouldBe(parameterSignature);

        string paramDoc = TypePageTools.GetParameterDoc(parameters[0]);
        paramDoc.ShouldBe(expectedDoc);
    }

    [Fact]
    public void Section_WithMultipleParameter_Matches()
    {
        using var document = DocumentationTools.GetApiPage("MyLibrary.User.html");
        var memberElement = document.GetMemberElement("ProcessValues(System.Int32@,System.Int32@,System.String,System.Int32@,System.Double)");

        var parameters = TypePageTools.GetMemberParameters(memberElement);

        parameters.Length.ShouldBe(5);

        (string signature, string doc)[] expectedValues = [
            ("in int inValue", "An input value."),
            ("ref int refValue", "A reference value."),
            ("string s1", ""),
            ("out int outValue", "An output value."),
            ("double d2", "")
        ];

        for (int i = 0; i < expectedValues.Length; i++)
        {
            string paramName = TypePageTools.GetParameterName(parameters[i]);
            paramName.ShouldBe(expectedValues[i].signature);

            string paramDoc = TypePageTools.GetParameterDoc(parameters[i]);
            paramDoc.ShouldBe(expectedValues[i].doc);
        }
    }
}
