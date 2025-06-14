using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RefDocGen.CodeElements.Members;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateProcessors.Shared.Tools.Names;
using Shouldly;

namespace RefDocGen.UnitTests.TemplateProcessors.Shared.Tools.Names;

/// <summary>
/// Class containing tests for <see cref="CSharpOperatorName"/> class.
/// </summary>
public class CSharpOperatorNameTests
{
    [Theory]
    [InlineData(OperatorKind.Addition, "operator +")]
    [InlineData(OperatorKind.Inequality, "operator !=")]
    [InlineData(OperatorKind.True, "operator true")]
    public void Of_ReturnsExpectedData_ForNonConversionOperator(OperatorKind kind, string expectedName)
    {
        var operatorData = Substitute.For<IOperatorData>();
        operatorData.Kind.Returns(kind);
        operatorData.IsConversionOperator.Returns(false);

        CSharpOperatorName.Of(operatorData).ShouldBe(expectedName);
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForConversionOperator()
    {
        var returnType = Substitute.For<ITypeNameData>();

        returnType.TypeObject.Returns(typeof(DateTime));
        returnType.ShortName.Returns("DateTime");
        returnType.HasTypeParameters.Returns(false);
        returnType.DeclaringType.ReturnsNull();

        var operatorData = Substitute.For<IOperatorData>();

        operatorData.Kind.Returns(OperatorKind.ExplicitConversion);
        operatorData.IsConversionOperator.Returns(true);
        operatorData.ReturnType.Returns(returnType);

        CSharpOperatorName.Of(operatorData).ShouldBe("operator DateTime");
    }
}
