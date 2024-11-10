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
    public void Id_IsCorrect_WithParameters()
    {
        var param1 = Substitute.For<ParameterData>();
        param1.Type.ShortName.Returns("String");
        param1.Name.Returns()

        var methodInfo = Substitute.For<MethodInfo>();
        methodInfo.Name.Returns("Execute");
        methodInfo.ReturnType.Returns(typeof(void));
        methodInfo.GetParameters().Returns([]);

        ExecutableMemberData memberData = new MethodData(methodInfo);

        memberData.Id.Should().Be("Execute");
    }
}
