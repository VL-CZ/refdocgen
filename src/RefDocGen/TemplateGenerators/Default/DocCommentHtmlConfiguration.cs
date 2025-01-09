using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using System.Xml.Linq;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Default configuration for transforming the XML documentation elements into their HTML representations.
/// </summary>
internal class DocCommentHtmlConfiguration : IDocCommentHtmlConfiguration
{
    /// <inheritdoc />
    public virtual XElement ParagraphElement => new("div");

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
    public virtual XElement SeeCrefElement => new("a");

    /// <inheritdoc />
    public virtual XElement SeeHrefElement => new("a");

    /// <inheritdoc />
    public virtual XElement SeeLangwordElement => new("code");

    /// <inheritdoc />
    public virtual XElement SeeCrefNotFoundElement => new("code");

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefElement => new("a");

    /// <inheritdoc />
    public virtual XElement SeeAlsoHrefElement => new("a");

    /// <inheritdoc />
    public virtual XElement SeeAlsoCrefNotFoundElement => new("code");
}
