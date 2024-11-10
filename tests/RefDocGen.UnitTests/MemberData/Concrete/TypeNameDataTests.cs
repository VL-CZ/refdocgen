using FluentAssertions;
using NSubstitute;
using RefDocGen.MemberData.Concrete;

namespace RefDocGen.UnitTests.MemberData.Concrete;

/// <summary>
/// Class containing tests for <see cref="TypeNameData"/> class.
/// </summary>
public class TypeNameDataTests
{
    [Fact]
    public void ShortName_IsCorrect_ForNonGenericType()
    {
        var mock = Substitute.For<Type>();
        mock.Name.Returns("Person");

        var type = new TypeNameData(mock);

        type.ShortName.Should().Be("Person");
    }

    [Fact]
    public void ShortName_IsCorrect_ForGenericType()
    {
        var mock = Substitute.For<Type>();
        mock.Name.Returns("List`2");

        var type = new TypeNameData(mock);

        type.ShortName.Should().Be("List");
    }

    [Fact]
    public void FullName_IsCorrect_ForNonGenericType()
    {
        var mock = Substitute.For<Type>();
        mock.Name.Returns("Person");
        mock.Namespace.Returns("MyApp.Entities");

        var type = new TypeNameData(mock);

        type.FullName.Should().Be("MyApp.Entities.Person");
    }

    [Fact]
    public void FullName_IsCorrect_ForGenericType()
    {
        var mock = Substitute.For<Type>();
        mock.Name.Returns("Dictionary`2");
        mock.Namespace.Returns("System.Collections.Generic");

        var type = new TypeNameData(mock);

        type.FullName.Should().Be("System.Collections.Generic.Dictionary");
    }

    [Theory]
    [InlineData("MyApp.Entities", "Person", "MyApp.Entities.Person")]
    [InlineData("MyApp.Entities", "Person[]", "MyApp.Entities.Person[]")]
    public void Id_IsCorrect_ForNonGenericType(string typeNamespace, string typeShortName, string expectedId)
    {
        var mock = Substitute.For<Type>();
        mock.Name.Returns(typeShortName);
        mock.Namespace.Returns(typeNamespace);
        mock.IsGenericType.Returns(false);

        var type = new TypeNameData(mock);

        type.Id.Should().Be(expectedId);
    }

    [Fact]
    public void Id_IsCorrect_ForGenericType()
    {
        string expectedId = "System.Collection.Generic.Dictionary{System.String,System.Collections.Generic.List{MyApp.Entities.Person}}";

        var type3 = Substitute.For<Type>();
        type3.Namespace.Returns("MyApp.Entities");
        type3.Name.Returns("Person");

        var type2 = Substitute.For<Type>();
        type2.Namespace.Returns("System.Collections.Generic");
        type2.Name.Returns("List`1");
        type2.IsGenericType.Returns(true);
        type2.GetGenericArguments().Returns([type3]);

        var type1 = Substitute.For<Type>();
        type1.Namespace.Returns("System");
        type1.Name.Returns("String");

        var mock = Substitute.For<Type>();
        mock.Name.Returns("Dictionary`2");
        mock.Namespace.Returns("System.Collection.Generic");
        mock.IsGenericType.Returns(true);
        mock.GetGenericArguments().Returns([type1, type2]);

        var type = new TypeNameData(mock);

        type.Id.Should().Be(expectedId);
    }
}
