using System.Xml.Linq;

namespace RefDocGen.TemplateProcessors.Shared.DocComments.Html;

/// <summary>
/// Defines the HTML representations of the inner XML documentation elements.
/// </summary>
internal interface IDocCommentHtmlConfiguration
{
    /// <summary>
    /// The HTML representation of the <c>&lt;para&gt;</c> element.
    /// </summary>
    XElement ParagraphElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;list type="bullet"&gt;</c> element.
    /// </summary>
    XElement BulletListElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;list type="number"&gt;</c> element.
    /// </summary>
    XElement NumberListElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;list type="table"&gt;</c> element.
    /// </summary>
    XElement TableListElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;item&gt;</c> element contained in a non-table list.
    /// </summary>
    XElement ListItemElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;item&gt;</c> element contained in a table list.
    /// </summary>
    XElement TableItemElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;term&gt;</c> element contained in a non-table list.
    /// </summary>
    XElement ListTermElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;description&gt;</c> element contained in a non-table list.
    /// </summary>
    XElement ListDescriptionElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;term&gt;</c> element contained in a table list.
    /// </summary>
    XElement TableTermElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;description&gt;</c> element contained in a table list.
    /// </summary>
    XElement TableDescriptionElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;listheader&gt;</c> element.
    /// </summary>
    XElement ListHeaderElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;c&gt;</c> element.
    /// </summary>
    XElement InlineCodeElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;code&gt;</c> element.
    /// </summary>
    XElement CodeBlockElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;example&gt;</c> element.
    /// </summary>
    XElement ExampleElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;paramref&gt;</c> element.
    /// </summary>
    XElement ParamRefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;typeparamref&gt;</c> element.
    /// </summary>
    XElement TypeParamRefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;see cref="..."&gt;</c> element.
    /// </summary>
    XElement SeeCrefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;see href="..."&gt;</c> element.
    /// </summary>
    XElement SeeHrefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;see langword="..."&gt;</c> element.
    /// </summary>
    XElement SeeLangwordElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;see cref="..."&gt;</c> element, whose reference isn't found.
    /// </summary>
    XElement SeeCrefNotFoundElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;seealso cref="..."&gt;</c> element.
    /// </summary>
    XElement SeeAlsoCrefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;seealso href="..."&gt;</c> element.
    /// </summary>
    XElement SeeAlsoHrefElement { get; }

    /// <summary>
    /// The HTML representation of the <c>&lt;seealso cref="..."&gt;</c> element, whose reference isn't found.
    /// </summary>
    XElement SeeAlsoCrefNotFoundElement { get; }
}
