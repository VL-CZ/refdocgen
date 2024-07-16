using System.Xml.Linq;

namespace RefDocGen.DocExtraction;

public class DocCommentTools
{
    public static XElement Empty => new("summary");
}
