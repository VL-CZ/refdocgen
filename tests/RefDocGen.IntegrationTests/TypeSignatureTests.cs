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
    [InlineData("MyLibrary.Dog", "public class Dog")]
    [InlineData("MyLibrary.Animal", "public abstract class Animal")]
    [InlineData("MyLibrary.Tools.Collections.MyDictionary`2", "internal class MyDictionary<TKey, TValue>")]
    [InlineData("MyLibrary.Tools.StringExtensions", "public static class StringExtensions")]
    [InlineData("MyLibrary.Tools.Point", "internal struct Point")]
    [InlineData("MyLibrary.Tools.Collections.IMyCollection`1", "internal interface IMyCollection<T>")]
    [InlineData("MyLibrary.Tools.ICovariant`1", "internal interface ICovariant<out T>")]
    [InlineData("MyLibrary.Tools.IContravariant`1", "internal interface IContravariant<in T>")]
    [InlineData("MyLibrary.Tools.Season", "internal enum Season")]
    [InlineData("MyLibrary.Tools.ObjectPredicate", "internal delegate ObjectPredicate")]
    [InlineData("MyLibrary.Tools.MyPredicate`1", "internal delegate MyPredicate<T>")]
    public void TypeSignature_Matches(string pageName, string expectedSignature)
    {
        using var document = DocumentationTools.GetPage($"{pageName}.html");

        string signature = TypePageTools.GetTypeSignature(document);

        signature.ShouldBe(expectedSignature);
    }
}
