using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Default configuration for transforming the XML documentation elements into their HTML representations.
/// </summary>
internal class DocCommentHtmlConfiguration : IDocCommentHtmlConfiguration
{
    /// <inheritdoc />
    public virtual XElement ParagraphElement => new("div", new XAttribute("class", "mx-2"));

    /// <inheritdoc />
    public virtual XElement BulletListElement => new("ul");

    /// <inheritdoc />
    public virtual XElement NumberListElement => new("ol");

    /// <inheritdoc />
    public virtual XElement ListItemElement => new("li");

    /// <inheritdoc />
    public virtual XElement InlineCodeElement => new("code");

    /// <inheritdoc />
    public virtual XElement CodeBlockElement => new("pre",
                                                    new XElement("code")
                                                );

    /// <inheritdoc />
    public virtual XElement ExampleElement => new("div");

    /// <inheritdoc />
    public virtual XElement ParamRefElement => new("code");

    /// <inheritdoc />
    public virtual XElement TypeParamRefElement => new("code");

    /// <inheritdoc />
    public virtual XElement SeeCrefElement =>
        new("a",
            new XAttribute("class", "link-primary link-offset-2 link-underline-opacity-50 link-underline-opacity-100-hover")
        );

    /// <inheritdoc />
    public virtual XElement SeeHrefElement => SeeCrefElement;

    /// <inheritdoc />
    public virtual XElement SeeLangwordElement => new("code");

    /// <inheritdoc />
    public virtual XElement SeeCrefNotFoundElement => new("code");

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefElement => SeeCrefElement;

    /// <inheritdoc />
    public virtual XElement SeeAlsoHrefElement => SeeCrefElement;

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefNotFoundElement => new("code");

    /// <inheritdoc />
    public XElement TermElement => new("b");

    /// <inheritdoc />
    public XElement ListHeaderElement => new("thead");

    /// <inheritdoc />
    public XElement TableListElement => new("table", new XAttribute("class", "table w-auto"));

    /// <inheritdoc />
    public XElement TableItemElement => new("tr");

    /// <inheritdoc />
    public XElement ListTermElement => new("b");

    /// <inheritdoc />
    public XElement ListDescriptionElement => new("span",
                                                    " ",
                                                    new XElement("span"));

    /// <inheritdoc />
    public XElement TableTermElement => new("td", new XAttribute("class", "text-muted"), new XElement("b"));

    /// <inheritdoc />
    public XElement TableDescriptionElement => new("td", new XAttribute("class", "text-muted"));
}
