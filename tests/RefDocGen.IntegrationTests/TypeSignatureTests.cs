using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests of type signatures (including classes, generic classes, interfaces, enums, etc.).
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class TypeSignatureTests
{
    [Theory]
    [InlineData("RefDocGen.TestingLibrary.Dog", "public class Dog")]
    [InlineData("RefDocGen.TestingLibrary.Animal", "public abstract class Animal")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.MyDictionary-2", "internal class MyDictionary<TKey, TValue>")]
    [InlineData("RefDocGen.TestingLibrary.Tools.StringExtensions", "public static class StringExtensions")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Point", "internal struct Point")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Collections.IMyCollection-1", "internal interface IMyCollection<T>")]
    [InlineData("RefDocGen.TestingLibrary.Tools.ICovariant-1", "internal interface ICovariant<out T>")]
    [InlineData("RefDocGen.TestingLibrary.Tools.IContravariant-1", "internal interface IContravariant<in T>")]
    [InlineData("RefDocGen.TestingLibrary.Tools.Season", "internal enum Season")]
    [InlineData("RefDocGen.TestingLibrary.Tools.ObjectPredicate", "internal delegate ObjectPredicate")]
    [InlineData("RefDocGen.TestingLibrary.Tools.MyPredicate-1", "internal delegate MyPredicate<T>")]
    public void TypeSignature_Matches(string pageName, string expectedSignature)
    {
        using var document = DocumentationTools.GetApiPage($"{pageName}.html");

        string signature = TypePageTools.GetTypeSignature(document);

        signature.ShouldBe(expectedSignature);
    }
}
