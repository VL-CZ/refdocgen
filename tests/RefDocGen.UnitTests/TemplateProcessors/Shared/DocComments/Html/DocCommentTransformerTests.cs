using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using Shouldly;
using System.Xml.Linq;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;

namespace RefDocGen.UnitTests.TemplateProcessors.Shared.DocComments.Html;

/// <summary>
/// Class responsible for transforming the XML doc comments into HTML.
/// </summary>
public class DocCommentTransformerTests
{
    /// <summary>
    /// Transformer of the doc comments into HTML.
    /// </summary>
    private readonly DocCommentTransformer docCommentTransformer;

    /// <inheritdoc cref="IDocCommentHtmlConfiguration"/>
    private readonly IDocCommentHtmlConfiguration configuration;

    public DocCommentTransformerTests()
    {
        configuration = new DocCommentHtmlConfiguration();

        var typeRegistry = GetMockedTypeRegistry();
        docCommentTransformer = new(configuration, typeRegistry);
    }

    [Fact]
    public void ToHtmlString_ReturnsTransformedData_ForBasicElements()
    {
        var docComment = XElement.Parse(
            """
            <summary>
                documentation, parameter <paramref name="input"/> and type parameter <typeparamref name="T"/>
                <para>
                    Paragraph
                </para>
                <list type="bullet">
                  <item>item1</item>
                </list>
                <list type="number">
                  <item>item2</item>
                </list>
                <code>
                int result = 1;
                </code>
            </summary>
            """);

        string? result = docCommentTransformer.ToHtmlString(docComment);

        string expectedResult = """
            <div>
                documentation, parameter <code class="refdocgen-paramref">input</code> and type parameter <code class="refdocgen-typeparamref">T</code>
                <div class="refdocgen-paragraph">
                    Paragraph
                </div>
                <ul class="refdocgen-bullet-list">
                  <li class="refdocgen-list-item">item1</li>
                </ul>
                <ol class="refdocgen-number-list">
                  <li class="refdocgen-list-item">item2</li>
                </ol>
                <pre>
                <code class="refdocgen-code-block">
                int result = 1;
                </code>
                </pre>
            </div>
            """;

        ShouldBeEqivalent(expectedResult, result);
    }

    [Fact]
    public void ToHtmlString_ReturnsHtmlTable_ForTableListElement()
    {
        var docComment = XElement.Parse(
            """
            <summary>
                <list type="table">
                  <listheader>
                    <term>Header 1</term>
                    <description>Header 2</description>
                  </listheader>
                  <item>
                    <term>Row 1, Col 1</term>
                    <description>Row 1, Col 2</description>
                  </item>
                  <item>
                    <term>Row 2, Col 1</term>
                    <description>Row 2, Col 2</description>
                  </item>
                </list>
            </summary>
            """);

        string? result = docCommentTransformer.ToHtmlString(docComment);

        string expectedResult = """
            <div>
                <table class="refdocgen-table">
                      <thead class="refdocgen-table-header">
                          <td class="refdocgen-table-term">Header 1</td>
                          <td class="refdocgen-table-element">Header 2</td>
                      </thead>
                      <tr class="refdocgen-table-item">
                          <td class="refdocgen-table-term">Row 1, Col 1</td>
                          <td class="refdocgen-table-element">Row 1, Col 2</td>
                      </tr>
                      <tr class="refdocgen-table-item">
                          <td class="refdocgen-table-term">Row 2, Col 1</td>
                          <td class="refdocgen-table-element">Row 2, Col 2</td>
                      </tr>
                </table>
            </div>
            """;

        ShouldBeEqivalent(expectedResult, result);
    }

    [Theory]
    [InlineData("T:type1", "./type1.html", "type1")]
    [InlineData("F:type1.field1", "./type1.html#field1", "type1.field1")]
    public void ToHtmlString_ReturnsLink_ForCrefElementWhenLinkFound(string crefValue, string expectedHref, string expectedText)
    {
        var docComment = XElement.Parse(
            $"""
            <summary>
                <see cref="{crefValue}"/>
            </summary>
            """);

        string? result = docCommentTransformer.ToHtmlString(docComment);

        string expectedResult = $"""
            <div>
                <a class="refdocgen-see-cref" href="{expectedHref}">{expectedText}</a>
            </div>
            """;

        ShouldBeEqivalent(expectedResult, result);
    }

    [Fact]
    public void ToHtmlString_ReturnsHighlightedText_ForCrefElementWhenLinkNotFound()
    {
        var docComment = XElement.Parse(
            $"""
            <summary>
                <see cref="T:notFoundType"/>
            </summary>
            """);

        string? result = docCommentTransformer.ToHtmlString(docComment);

        string expectedResult = $"""
            <div>
                <code class="refdocgen-see-cref-not-found">notFoundType</code>
            </div>
            """;

        ShouldBeEqivalent(expectedResult, result);
    }

    /// <summary>
    /// Checks whether the <paramref name="actualHtmlDoc"/> documentation comments in HTML format matches the <paramref name="expectedHtmlDoc"/>, regardless to whitespace.
    /// </summary>
    /// <param name="expectedHtmlDoc">The expected documentation comment as HTML string.</param>
    /// <param name="actualHtmlDoc">The actual documentation comment as HTML string.</param>
    private void ShouldBeEqivalent(string? expectedHtmlDoc, string? actualHtmlDoc)
    {
        if (expectedHtmlDoc is not null && actualHtmlDoc is not null)
        {
            var expectedXml = XElement.Parse(expectedHtmlDoc);
            var actualXml = XElement.Parse(actualHtmlDoc);

            // compare as XElements, because of possible whitespace
            XNode.DeepEquals(expectedXml, actualXml)
                .ShouldBeTrue($"Expected:\n{expectedXml}\n\nActual:\n{actualXml}");
        }
        else
        {
            actualHtmlDoc.ShouldBe(expectedHtmlDoc);
        }
    }

    /// <summary>
    /// Gets mocked type registry with two types: <c>type1</c> and <c>type2</c>.
    /// </summary>
    /// <returns>Mock of the type registry.</returns>
    private ITypeRegistry GetMockedTypeRegistry()
    {
        var typeRegistry = Substitute.For<ITypeRegistry>();

        var type1 = Substitute.For<ITypeDeclaration>();
        type1.ShortName.Returns("type1");
        type1.HasTypeParameters.Returns(false);
        type1.DeclaringType.ReturnsNull();

        var type2 = Substitute.For<ITypeDeclaration>();
        type2.ShortName.Returns("type2");
        type2.HasTypeParameters.Returns(false);
        type2.DeclaringType.ReturnsNull();

        typeRegistry.GetDeclaredType("type1").Returns(type1);
        typeRegistry.GetDeclaredType("type2").Returns(type2);
        typeRegistry.GetDeclaredType("notFoundType").ReturnsNull();

        return typeRegistry;
    }
}
