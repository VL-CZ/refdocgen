using NSubstitute;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;
using Shouldly;

namespace RefDocGen.UnitTests.TemplateGenerators.Shared.Tools.Names;

/// <summary>
/// Class containing tests for <see cref="CSharpOperatorName"/> class.
/// </summary>
public class CSharpOperatorNameTests
{
    [Theory]
    [InlineData(OperatorKind.Addition, "operator +")]
    [InlineData(OperatorKind.Inequality, "operator !=")]
    [InlineData(OperatorKind.True, "operator true")]
    public void Of_ReturnsCorrectName_ForNonConversionOperator(OperatorKind kind, string expectedName)
    {
        var operatorData = Substitute.For<IOperatorData>();
        operatorData.Kind.Returns(kind);
        operatorData.IsConversionOperator.Returns(false);

        CSharpOperatorName.Of(operatorData).ShouldBe(expectedName);
    }

    [Fact]
    public void Of_ReturnsCorrectName_ForConversionOperator()
    {
        var returnType = Substitute.For<ITypeNameData>();

        returnType.TypeObject.Returns(typeof(DateTime));
        returnType.ShortName.Returns("DateTime");
        returnType.HasTypeParameters.Returns(false);

        var operatorData = Substitute.For<IOperatorData>();

        operatorData.Kind.Returns(OperatorKind.ExplicitConversion);
        operatorData.IsConversionOperator.Returns(true);
        operatorData.ReturnType.Returns(returnType);

        CSharpOperatorName.Of(operatorData).ShouldBe("operator DateTime");
    }
}
