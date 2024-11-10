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
        var methodInfo = Substitute.For<MethodInfo>();
        methodInfo.Name.Returns("Execute");
        methodInfo.ReturnType.Returns(typeof(void));
        methodInfo.GetParameters().Returns([]);

        ExecutableMemberData memberData = new MethodData(methodInfo);

        memberData.Id.Should().Be("Execute");
    }

    [Fact]
    public void Id_IsCorrect_WithSingleParameter()
    {
        var param = Substitute.For<ParameterInfo>();
        param.ParameterType.Returns(typeof(string));

        var methodInfo = Substitute.For<MethodInfo>();
        methodInfo.Name.Returns("Execute");
        methodInfo.ReturnType.Returns(typeof(void));
        methodInfo.GetParameters().Returns([param]);

        ExecutableMemberData memberData = new MethodData(methodInfo);

        memberData.Id.Should().Be("Execute(System.String)");
    }

    [Fact]
    public void Id_IsCorrect_WithMultipleParameters()
    {
        string expectedId = "Execute(System.String,System.Type,System.Int32@)";

        var param1 = Substitute.For<ParameterInfo>();
        param1.ParameterType.Returns(typeof(string));

        var param2 = Substitute.For<ParameterInfo>();
        param2.ParameterType.Returns(typeof(Type));

        var param3 = Substitute.For<ParameterInfo>();
        param3.ParameterType.Returns(typeof(int).MakeByRefType());

        var methodInfo = Substitute.For<MethodInfo>();
        methodInfo.Name.Returns("Execute");
        methodInfo.ReturnType.Returns(typeof(void));
        methodInfo.GetParameters().Returns([param1, param2, param3]);

        ExecutableMemberData memberData = new MethodData(methodInfo);

        memberData.Id.Should().Be(expectedId);
    }
}
