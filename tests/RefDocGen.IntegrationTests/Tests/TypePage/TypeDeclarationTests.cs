using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests.Tests.TypePage;

/// <summary>
/// This class contains tests of type declarations (including classes, generic classes, interfaces, enums, etc.).
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeDeclarationTests
{
    [Theory]
    [InlineData("RefDocGen.ExampleLibrary.Dog", "public class Dog")]
    [InlineData("RefDocGen.ExampleLibrary.Animal", "public abstract class Animal")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.MyDictionary-2", "internal class MyDictionary<TKey, TValue>")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.StringExtensions", "public static class StringExtensions")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Point", "internal struct Point")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Collections.IMyCollection-1", "internal interface IMyCollection<T>")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.ICovariant-1", "internal interface ICovariant<out T>")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.IContravariant-1", "internal interface IContravariant<in T>")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.Season", "internal enum Season")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.ObjectPredicate", "internal delegate ObjectPredicate")]
    [InlineData("RefDocGen.ExampleLibrary.Tools.MyPredicate-1", "internal delegate MyPredicate<T>")]
    [InlineData("RefDocGen.ExampleFSharpLibrary.FSharpMathTools", "public static class FSharpMathTools")]
    [InlineData("RefDocGen.ExampleVbLibrary.VbMathTools", "public class VbMathTools")]
    public void TypeDeclaration_Matches(string pageName, string expectedDeclarationString)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string typeDeclString = TypePageTools.GetTypeDeclaration(document);

        typeDeclString.ShouldBe(expectedDeclarationString);
    }
}
