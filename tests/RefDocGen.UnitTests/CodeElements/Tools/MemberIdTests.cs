using FluentAssertions;
using NSubstitute;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Concrete.Types.TypeName;
using RefDocGen.CodeElements.Tools;

namespace RefDocGen.UnitTests.CodeElements.Tools;

/// <summary>
/// Class containing tests for <see cref="MemberId"/> class.
/// </summary>
public class MemberIdTests
{
    [Fact]
    public void Of_ReturnsMemberName_WhenNoParameters()
    {
        var method = MockMethodData("Execute", typeof(void), []);

        MemberId.Of(method).Should().Be("Execute");
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForSingleParameter()
    {
        var param = MockParameter(typeof(string));
        var method = MockMethodData("Execute", typeof(void), [param]);

        MemberId.Of(method).Should().Be("Execute(System.String)");
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForSingleRefParameter()
    {
        var param = MockParameter(typeof(string), true);
        var method = MockMethodData("Execute", typeof(void), [param]);

        MemberId.Of(method).Should().Be("Execute(System.String@)");
    }

    [Fact]
    public void Of_ReturnsCorrectData_ForMultipleParameters()
    {
        var param1 = MockParameter(typeof(string));
        var param2 = MockParameter(typeof(Type));
        var param3 = MockParameter(typeof(int).MakeByRefType(), true);
        var param4 = MockParameter(typeof(double).MakeArrayType());
        var method = MockMethodData("Execute", typeof(void), [param1, param2, param3, param4]);

        string expectedId = "Execute(System.String,System.Type,System.Int32@,System.Double[])";

        MemberId.Of(method).Should().Be(expectedId);
    }

    /// <summary>
    /// Mock <see cref="IMethodData"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="name">Name of the method.</param>
    /// <param name="returnType">Return type of the method.</param>
    /// <param name="parameters">Array of method parameters.</param>
    /// <returns>Mocked <see cref="IMethodData"/> instance.</returns>
    private IMethodData MockMethodData(string name, Type returnType, IParameterData[] parameters)
    {
        var methodInfo = Substitute.For<IMethodData>();
        var typeNameData = new TypeNameData(returnType);

        methodInfo.Name.Returns(name);
        methodInfo.ReturnType.Returns(typeNameData);
        methodInfo.Parameters.Returns(parameters);

        return methodInfo;
    }

    /// <summary>
    /// Mock <see cref="IParameterData"/> and initialize it with the provided data.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="isByRef">Is the parameter passed by reference?</param>
    /// <returns>Mocked <see cref="IParameterData"/> instance.</returns>
    private IParameterData MockParameter(Type type, bool isByRef = false)
    {
        var param = Substitute.For<IParameterData>();
        var typeNameData = new TypeNameData(type);

        param.Type.Returns(typeNameData);
        param.IsByRef.Returns(isByRef);

        return param;
    }
}
