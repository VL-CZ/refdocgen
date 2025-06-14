using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Tools;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete.TypeName;
using Shouldly;
using System.Reflection;

namespace RefDocGen.UnitTests.CodeElements.Members.Tools;

/// <summary>
/// Class containing tests for <see cref="MemberId"/> class.
/// </summary>
public class MemberIdTests
{
    [Fact]
    public void Of_ReturnsMemberName_ForMethodWithoutParameters()
    {
        var method = MockMethodData("Execute", typeof(void), []);

        MemberId.Of(method).ShouldBe("Execute");
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForMethodWithASingleParameter()
    {
        var param = MockParameterData(typeof(string));
        var method = MockMethodData("Execute", typeof(void), [param]);

        MemberId.Of(method).ShouldBe("Execute(System.String)");
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForMethodWithASingleRefParameter()
    {
        var param = MockParameterData(typeof(string), isByRef: true);
        var method = MockMethodData("Execute", typeof(void), [param]);

        MemberId.Of(method).ShouldBe("Execute(System.String@)");
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForExplicitlyImplementedMethod()
    {
        var explicitInterfaceType = Substitute.For<ITypeNameData>();
        explicitInterfaceType.Id.Returns("MyApp.MyInterface");

        var param = MockParameterData(typeof(MemberInfo));
        var method = MockMethodData("Execute", typeof(void), [param], explicitInterfaceType);

        MemberId.Of(method).ShouldBe("MyApp#MyInterface#Execute(System.Reflection.MemberInfo)");
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForMethodWithMultipleParameters()
    {
        var param1 = MockParameterData(typeof(string));
        var param2 = MockParameterData(typeof(Type));
        var param3 = MockParameterData(typeof(int).MakeByRefType(), true);
        var param4 = MockParameterData(typeof(double).MakeArrayType());
        var method = MockMethodData("Execute", typeof(void), [param1, param2, param3, param4]);

        string expectedId = "Execute(System.String,System.Type,System.Int32@,System.Double[])";

        MemberId.Of(method).ShouldBe(expectedId);
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForConversionOperator()
    {
        var returnType = Substitute.For<ITypeNameData>();
        returnType.Id.Returns("MyApp.MyClass");

        var param = MockParameterData(typeof(MemberInfo));

        var operatorData = Substitute.For<IOperatorData>();

        operatorData.Name.Returns("op_Explicit");
        operatorData.ReturnType.Returns(returnType);
        operatorData.Parameters.Returns([param]);
        operatorData.IsConversionOperator.Returns(true);
        operatorData.IsExplicitImplementation.Returns(false);
        operatorData.ExplicitInterfaceType.ReturnsNull();

        MemberId.Of(operatorData).ShouldBe("op_Explicit(System.Reflection.MemberInfo)~MyApp.MyClass");
    }

    [Fact]
    public void Of_ReturnsExpectedData_ForIndexer()
    {
        var param = MockParameterData(typeof(int));

        var indexerData = Substitute.For<IIndexerData>();

        indexerData.Name.Returns("Item");
        indexerData.Parameters.Returns([param]);
        indexerData.IsExplicitImplementation.Returns(false);
        indexerData.ExplicitInterfaceType.ReturnsNull();

        MemberId.Of(indexerData).ShouldBe("Item(System.Int32)");
    }

    /// <summary>
    /// Mock <see cref="IMethodData"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="name">Name of the method.</param>
    /// <param name="returnType">Return type of the method.</param>
    /// <param name="parameters">Array of method parameters.</param>
    /// <param name="explicitInterfaceType">Type of the interface that explicitly declares the member,
    /// <c>null</c> if the member isn't explicitly implemented.</param>
    /// <returns>Mocked <see cref="IMethodData"/> instance.</returns>
    private IMethodData MockMethodData(string name, Type returnType,
        IParameterData[] parameters, ITypeNameData? explicitInterfaceType = null)
    {
        var methodInfo = Substitute.For<IMethodData>();
        var typeNameData = new TypeNameData(returnType);

        methodInfo.Name.Returns(name);
        methodInfo.ReturnType.Returns(typeNameData);
        methodInfo.Parameters.Returns(parameters);
        methodInfo.ExplicitInterfaceType.Returns(explicitInterfaceType);
        methodInfo.IsExplicitImplementation.Returns(explicitInterfaceType is not null);

        return methodInfo;
    }

    /// <summary>
    /// Mock <see cref="IParameterData"/> and initialize it with the provided data.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="isByRef">Is the parameter passed by reference?</param>
    /// <returns>Mocked <see cref="IParameterData"/> instance.</returns>
    private IParameterData MockParameterData(Type type, bool isByRef = false)
    {
        var param = Substitute.For<IParameterData>();
        var typeNameData = new TypeNameData(type);

        param.Type.Returns(typeNameData);
        param.IsByRef.Returns(isByRef);

        return param;
    }
}
