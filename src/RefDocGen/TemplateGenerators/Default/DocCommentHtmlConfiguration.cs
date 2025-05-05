using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.DocComments.Html;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Default configuration for transforming the XML documentation elements into their HTML representations.
/// </summary>
internal class DocCommentHtmlConfiguration : IDocCommentHtmlConfiguration
{
    /// <inheritdoc />
    public virtual XElement ParagraphElement => new XElement("div").WithClass("refdocgen-paragraph");
    //, new XAttribute("class", "refdocgen-paragraph"), new XAttribute("class", "mx-2"));

    /// <inheritdoc />
    public virtual XElement BulletListElement => new XElement("ul").WithClass("refdocgen-bullet-list");

    /// <inheritdoc />
    public virtual XElement NumberListElement => new XElement("ol").WithClass("refdocgen-number-list");

    /// <inheritdoc />
    public virtual XElement ListItemElement => new XElement("li").WithClass("refdocgen-list-item");

    /// <inheritdoc />
    public virtual XElement InlineCodeElement => new XElement("code").WithClass("refdocgen-inline-code");

    /// <inheritdoc />
    public virtual XElement CodeBlockElement => new("pre",
                                                    new XElement("code").WithClass("refdocgen-code-block")
                                                );

    /// <inheritdoc />
    public virtual XElement ExampleElement => new XElement("div").WithClass("refdocgen-example");

    /// <inheritdoc />
    public virtual XElement ParamRefElement => new XElement("code").WithClass("refdocgen-paramref");

    /// <inheritdoc />
    public virtual XElement TypeParamRefElement => new XElement("code").WithClass("refdocgen-typeparamref");

    /// <inheritdoc />
    public virtual XElement SeeCrefElement => new XElement("a").WithClass("refdocgen-see-cref");
    //new("a",
    //    new XAttribute("class", "link-offset-2 link-underline-opacity-50 link-underline-opacity-100-hover")
    //);

    /// <inheritdoc />
    public virtual XElement SeeHrefElement => new XElement("a").WithClass("refdocgen-see-href");

    /// <inheritdoc />
    public virtual XElement SeeLangwordElement => new XElement("code").WithClass("refdocgen-see-langword");

    /// <inheritdoc />
    public virtual XElement SeeCrefNotFoundElement => new XElement("code").WithClass("refdocgen-see-cref-not-found");

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefElement => new XElement("a").WithClass("refdocgen-seealso-cref");

    /// <inheritdoc />
    public virtual XElement SeeAlsoHrefElement => new XElement("a").WithClass("refdocgen-seealso-href");

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefNotFoundElement => new XElement("code").WithClass("refdocgen-seealso-cref-not-found");

    /// <inheritdoc />
    public virtual XElement ListHeaderElement => new XElement("thead").WithClass("refdocgen-table-header");

    /// <inheritdoc />
    public virtual XElement TableListElement => new XElement("table").WithClass("refdocgen-table");
    //, new XAttribute("class", "table w-auto"));

    /// <inheritdoc />
    public virtual XElement TableItemElement => new XElement("tr").WithClass("refdocgen-table-item");

    /// <inheritdoc />
    public virtual XElement ListTermElement => new XElement("b").WithClass("refdocgen-list-term");

    /// <inheritdoc />
    public XElement ListDescriptionElement => new("span",
                                                    " ",
                                                    new XElement("span").WithClass("refdocgen-list-description"));

    /// <inheritdoc />
    public XElement TableTermElement => new XElement("td").WithClass("refdocgen-table-term");
    //new XAttribute("class", "text-muted")
    //new XAttribute("class", "text-muted")

    /// <inheritdoc />
    public XElement TableDescriptionElement => new XElement("td").WithClass("refdocgen-table-element");
    //new XAttribute("class", "text-muted")
}
