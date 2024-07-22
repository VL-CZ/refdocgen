using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

/// <summary>
/// Contains helper methods for anything related to XML documentation comments.
/// </summary>
internal class DocCommentTools
{
    /// <summary>
    /// Get new empty 'summary' <see cref="XElement"/>.
    /// </summary>
    internal static XElement EmptySummaryNode => new("summary");

    internal static XElement EmptyParamNode => new("param");
}
