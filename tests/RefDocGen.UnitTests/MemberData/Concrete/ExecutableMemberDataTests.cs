using FluentAssertions;
using NSubstitute;
using RefDocGen.MemberData.Concrete;
using System.Reflection;

namespace RefDocGen.UnitTests.MemberData.Concrete;

/// <summary>
/// Class containing tests for <see cref="ExecutableMemberData"/> class.
/// </summary>
public class ExecutableMemberDataTests
{
    [Fact]
    public void Id_IsSameAsMemberName_WhenNoParameters()
    {
        var methodInfo = MockMethodInfo("Execute", typeof(void), []);
        ExecutableMemberData memberData = new MethodData(methodInfo, []);

        memberData.Id.Should().Be("Execute");
    }

    [Fact]
    public void Id_ReturnsCorrectData_WithSingleParameter()
    {
        var param = MockParameterInfo(typeof(string));
        var methodInfo = MockMethodInfo("Execute", typeof(void), [param]);

        ExecutableMemberData memberData = new MethodData(methodInfo, []);

        memberData.Id.Should().Be("Execute(System.String)");
    }

    [Fact]
    public void Id_ReturnsCorrectData_WithMultipleParameters()
    {
        var param1 = MockParameterInfo(typeof(string));
        var param2 = MockParameterInfo(typeof(Type));
        var param3 = MockParameterInfo(typeof(int).MakeByRefType());
        var param4 = MockParameterInfo(typeof(double).MakeArrayType());
        var methodInfo = MockMethodInfo("Execute", typeof(void), [param1, param2, param3, param4]);

        ExecutableMemberData memberData = new MethodData(methodInfo, []);

        string expectedId = "Execute(System.String,System.Type,System.Int32@,System.Double[])";

        memberData.Id.Should().Be(expectedId);
    }

    /// <summary>
    /// Mock <see cref="MethodInfo"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="name">Name of the method.</param>
    /// <param name="returnType">Return type of the method.</param>
    /// <param name="parameters">Array of method parameters.</param>
    /// <returns>Mocked <see cref="MethodInfo"/> instance.</returns>
    private MethodInfo MockMethodInfo(string name, Type returnType, ParameterInfo[] parameters)
    {
        var methodInfo = Substitute.For<MethodInfo>();
        methodInfo.Name.Returns(name);
        methodInfo.ReturnType.Returns(returnType);
        methodInfo.GetParameters().Returns(parameters);

        return methodInfo;
    }

    /// <summary>
    /// Mock <see cref="ParameterInfo"/> and initialize it with the provided data.
    /// </summary>
    /// <param name="type">Type of the parameter</param>
    /// <returns>Mocked <see cref="ParameterInfo"/> instance.</returns>
    private ParameterInfo MockParameterInfo(Type type)
    {
        var param = Substitute.For<ParameterInfo>();
        param.ParameterType.Returns(type);

        return param;
    }
}
