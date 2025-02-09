using AngleSharp;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace RefDocGen.IntegrationTests;

internal class Tools
{
    internal static IDocument GetDocument(string name)
    {
        string userFile = Path.Join("output", name);
        string fileData = File.ReadAllText(userFile);

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

    internal static string GetMemberSignature(IElement memberElement)
    {
        var memberNameElement = memberElement.GetByDataId(DataId.MemberName);
        string content = ParseStringContent(memberNameElement);

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }

    private static string GetParsedContent(IElement element, DataId dataId)
    {
        var targetElement = element.GetByDataId(dataId);
        return ParseStringContent(targetElement);
    }

    internal static string GetSummaryDocContent(IElement memberElement)
    {
        return GetParsedContent(memberElement, DataId.SummaryDoc);
    }

    internal static string GetRemarksDocContent(IElement memberElement)
    {
        return GetParsedContent(memberElement, DataId.RemarksDoc);
    }

    internal static string GetValueDocContent(IElement memberElement)
    {
        return GetParsedContent(memberElement, DataId.ValueDoc);
    }

    internal static string GetReturnTypeName(IElement memberElement)
    {
        return GetParsedContent(memberElement, DataId.ReturnTypeName);
    }

    internal static string GetReturnsDoc(IElement memberElement)
    {
        return GetParsedContent(memberElement, DataId.ReturnsDoc);
    }

    internal static string GetParameterName(IElement paramElement)
    {
        return GetParsedContent(paramElement, DataId.ParameterName);
    }

    internal static string GetParameterDoc(IElement paramElement)
    {
        return GetParsedContent(paramElement, DataId.ParameterDoc);
    }

    internal static string GetTypeParameterName(IElement typeParamElement)
    {
        return GetParsedContent(typeParamElement, DataId.TypeParameterName);
    }

    internal static string GetTypeParameterDoc(IElement typeParamElement)
    {
        return GetParsedContent(typeParamElement, DataId.TypeParameterDoc);
    }

    internal static string GetExceptionType(IElement paramElement)
    {
        return GetParsedContent(paramElement, DataId.ExceptionType);
    }

    internal static string GetExceptionDoc(IElement paramElement)
    {
        return GetParsedContent(paramElement, DataId.ExceptionDoc);
    }

    internal static string GetBaseTypeName(IElement element)
    {
        return GetParsedContent(element, DataId.BaseType);
    }

    internal static string GetInterfacesString(IElement element)
    {
        return GetParsedContent(element, DataId.ImplementedInterfaces);
    }

    internal static string GetNamespaceString(IElement element)
    {
        return GetParsedContent(element, DataId.TypeNamespace);
    }

    internal static string GetTypeRowName(IElement element)
    {
        return GetParsedContent(element, DataId.TypeRowName);
    }

    internal static string GetTypeRowDoc(IElement element)
    {
        return GetParsedContent(element, DataId.TypeRowDoc);
    }

    internal static IElement[] GetMemberParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(DataId.ParameterElement)];
    }

    internal static IElement[] GetTypeParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(DataId.TypeParameterElement)];
    }

    internal static string[] GetAttributes(IElement element)
    {
        return element.GetByDataIds(DataId.AttributeData).Select(ParseStringContent).ToArray();
    }

    internal static string[] GetSeeAlsoDocs(IElement element)
    {
        return element.GetByDataIds(DataId.SeeAlsoDocs).Select(ParseStringContent).ToArray();
    }

    internal static IElement[] GetExceptions(IElement element)
    {
        return [.. element.GetByDataIds(DataId.ExceptionData)];
    }

    internal static string[] GetTypeParamConstraints(IElement element)
    {
        return element.GetByDataIds(DataId.TypeParamConstraints).Select(ParseStringContent).ToArray();
    }

    internal static string GetTypeSignature(IDocument document)
    {
        return ParseStringContent(document.GetTypeSignatureElement());
    }

    internal static IElement[] GetNamespaceClasses(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceClasses).GetByDataIds(DataId.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceInterfaces(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceInterfaces).GetByDataIds(DataId.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceDelegates(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceDelegates).GetByDataIds(DataId.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceEnums(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceEnums).GetByDataIds(DataId.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceStructs(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(DataId.NamespaceStructs).GetByDataIds(DataId.TypeRowElement)];
    }

    internal static string[] GetNamespaceNames(IDocument document)
    {
        return document.DocumentElement.GetByDataIds(DataId.NamespaceName).Select(ParseStringContent).ToArray();
    }

    internal static string[] GetNamespaceTypeNames(IElement element)
    {
        return element.GetByDataIds(DataId.TypeRowElement).Select(GetTypeRowName).ToArray();
    }
}

