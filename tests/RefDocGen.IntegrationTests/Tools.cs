using AngleSharp;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace RefDocGen.IntegrationTests;

internal class Tools
{
    internal const string MemberName = "member-name";
    internal const string SummaryDoc = "summary-doc";
    internal const string ReturnTypeName = "return-type-name";
    internal const string ReturnsDoc = "returns-doc";
    internal const string ParameterElement = "parameter-data";
    internal const string ParameterName = "parameter-name";
    internal const string ParameterDoc = "parameter-doc";

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

    internal static string ParseStringContent(IElement element)
    {
        return Regex.Replace(element.TextContent, @"\s+", " ").Trim();
    }

    internal static string GetMemberNameContent(IElement memberElement)
    {
        var memberNameElement = memberElement.GetDataId(MemberName);

        var content = ParseStringContent(memberNameElement);

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }

    internal static string GetSummaryDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetDataId(SummaryDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetReturnTypeName(IElement memberElement)
    {
        var returnTypeElement = memberElement.GetDataId(ReturnTypeName);
        return ParseStringContent(returnTypeElement);
    }

    internal static string GetReturnsDoc(IElement memberElement)
    {
        var returnsDocElement = memberElement.GetDataId(ReturnsDoc);
        return ParseStringContent(returnsDocElement);
    }

    internal static IElement[] GetMemberParameters(IElement memberElement)
    {
        return [.. memberElement.GetDataIds(ParameterElement)];
    }

    internal static string GetParameterName(IElement paramElement)
    {
        var paramNameElement = paramElement.GetDataId(ParameterName);
        return ParseStringContent(paramNameElement);
    }

    internal static string GetParameterDoc(IElement paramElement)
    {
        var paramDocElement = paramElement.GetDataId(ParameterDoc);
        return ParseStringContent(paramDocElement);
    }
}


