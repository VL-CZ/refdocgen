using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;

namespace RefDocGen.IntegrationTests;

internal class Shared
{
    internal const string MemberName = "member-name";
    internal const string SummaryDoc = "summary-doc";

    internal static IDocument GetDocument(string name)
    {
        var userFile = Path.Join("output", name);
        var fileData = File.ReadAllText(userFile);

        // Configure and create a browsing context
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);

        // Load the HTML document directly from the file
        var document = context.OpenAsync((req) => req.Content(fileData)).Result; // TODO
        return document;
    }

    internal static INode GetSignature(IElement memberElement)
    {
        return memberElement.FirstChild ?? throw new Exception();
    }

    internal static IHtmlDivElement GetDocComment(IElement memberElement)
    {
        return memberElement.FindChild<IHtmlDivElement>() ?? throw new Exception();
    }

    internal static string ParseStringContent(IElement memberElement)
    {
        return Regex.Replace(memberElement.TextContent, @"\s+", " ").Trim();
    }

    internal static string GetMemberNameContent(IElement memberElement)
    {
        var content = ParseStringContent(memberElement);

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }
}


