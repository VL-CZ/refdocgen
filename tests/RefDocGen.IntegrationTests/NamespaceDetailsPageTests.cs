using AngleSharp.Dom;
using RefDocGen.IntegrationTests.Fixtures;
using RefDocGen.IntegrationTests.Tools;
using Shouldly;

namespace RefDocGen.IntegrationTests;

/// <summary>
/// This class contains tests for 'namespace detail' page.
/// </summary>
[Collection(DocumentationTestCollection.Name)]
public class NamespaceDetailsPageTests : IDisposable
{
    /// <summary>
    /// The HTML page representing the <c>MyLibrary.Tools</c> namespace.
    /// </summary>
    private readonly IDocument document;

    public NamespaceDetailsPageTests()
    {
        document = DocumentationTools.GetPage("MyLibrary.Tools.html");
    }

    public void Dispose()
    {
        document.Dispose();
    }

    [Fact]
    public void ClassNames_Match()
    {
        var classes = NamespacePageTools.GetNamespaceClasses(document);

        classes.Length.ShouldBe(2);

        string class1Name = NamespacePageTools.GetTypeRowName(classes[0]);
        string class2Name = NamespacePageTools.GetTypeRowName(classes[1]);

        class1Name.ShouldBe("class StringExtensions");
        class2Name.ShouldBe("class WeatherStation");
    }

    [Fact]
    public void StructNames_Match()
    {
        var structs = NamespacePageTools.GetNamespaceStructs(document);

        structs.Length.ShouldBe(1);

        string structName = NamespacePageTools.GetTypeRowName(structs[0]);

        structName.ShouldBe("struct Point");
    }

    [Fact]
    public void InterfaceNames_Match()
    {
        var interfaces = NamespacePageTools.GetNamespaceInterfaces(document);

        interfaces.Length.ShouldBe(2);

        string interface1Name = NamespacePageTools.GetTypeRowName(interfaces[0]);
        string interface2Name = NamespacePageTools.GetTypeRowName(interfaces[1]);

        interface1Name.ShouldBe("interface IContravariant<T>");
        interface2Name.ShouldBe("interface ICovariant<T>");
    }

    [Fact]
    public void EnumNames_Match()
    {
        var enums = NamespacePageTools.GetNamespaceEnums(document);

        enums.Length.ShouldBe(2);

        string enum1Name = NamespacePageTools.GetTypeRowName(enums[0]);
        string enum2Name = NamespacePageTools.GetTypeRowName(enums[1]);

        enum1Name.ShouldBe("enum HarvestingSeason");
        enum2Name.ShouldBe("enum Season");
    }

    [Fact]
    public void DelegateNames_Match()
    {
        var delegates = NamespacePageTools.GetNamespaceDelegates(document);

        delegates.Length.ShouldBe(2);

        string delegate1Name = NamespacePageTools.GetTypeRowName(delegates[0]);
        string delegate2Name = NamespacePageTools.GetTypeRowName(delegates[1]);

        delegate1Name.ShouldBe("delegate MyPredicate<T>");
        delegate2Name.ShouldBe("delegate ObjectPredicate");
    }

}
