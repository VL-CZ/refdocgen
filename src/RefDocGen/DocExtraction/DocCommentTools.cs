using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

internal class DocCommentTools
{
    internal static XElement Empty => new("summary");
}
