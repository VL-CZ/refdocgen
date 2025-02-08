using AngleSharp;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace RefDocGen.IntegrationTests;

internal enum XmlDocElement
{
    MemberName,
    AttributeData,
    ReturnTypeName,
    ParameterElement,
    ParameterName,
    ParameterDoc,
    TypeParameterElement,
    TypeParameterName,
    TypeParameterDoc,
    SummaryDoc,
    RemarksDoc,
    ReturnsDoc,
    ValueDoc,
    SeeAlsoDocs,
    TypeParamConstraints,
    BaseType,
    ImplementedInterfaces,
    TypeNamespace,
    ExceptionData,
    ExceptionType,
    ExceptionDoc,
    DelegateMethod,
    NamespaceClasses,
    NamespaceInterfaces,
    NamespaceDelegates,
    NamespaceEnums,
    NamespaceStructs,
    TypeRowElement,
    TypeRowName,
    TypeRowDoc,
    NamespaceName
}

internal static class XmlDocElementExtensions
{
    internal static string ToXmlString(this XmlDocElement element)
    {
        return element switch
        {
            XmlDocElement.MemberName => "member-name",
            XmlDocElement.AttributeData => "attribute-data",
            XmlDocElement.ReturnTypeName => "return-type-name",
            XmlDocElement.ParameterElement => "parameter-data",
            XmlDocElement.ParameterName => "parameter-name",
            XmlDocElement.ParameterDoc => "parameter-doc",
            XmlDocElement.TypeParameterElement => "type-parameter-data",
            XmlDocElement.TypeParameterName => "type-parameter-name",
            XmlDocElement.TypeParameterDoc => "type-parameter-doc",
            XmlDocElement.SummaryDoc => "summary-doc",
            XmlDocElement.RemarksDoc => "remarks-doc",
            XmlDocElement.ReturnsDoc => "returns-doc",
            XmlDocElement.ValueDoc => "value-doc",
            XmlDocElement.SeeAlsoDocs => "seealso-item",
            XmlDocElement.TypeParamConstraints => "type-param-constraints",
            XmlDocElement.BaseType => "base-type",
            XmlDocElement.ImplementedInterfaces => "implemented-interfaces",
            XmlDocElement.TypeNamespace => "type-namespace",
            XmlDocElement.ExceptionData => "exception-data",
            XmlDocElement.ExceptionType => "exception-type",
            XmlDocElement.ExceptionDoc => "exception-doc",
            XmlDocElement.DelegateMethod => "delegate-method",
            XmlDocElement.NamespaceClasses => "namespace-classes",
            XmlDocElement.NamespaceInterfaces => "namespace-interfaces",
            XmlDocElement.NamespaceDelegates => "namespace-delegates",
            XmlDocElement.NamespaceEnums => "namespace-enums",
            XmlDocElement.NamespaceStructs => "namespace-structs",
            XmlDocElement.TypeRowElement => "type-row-element",
            XmlDocElement.TypeRowName => "type-row-name",
            XmlDocElement.TypeRowDoc => "type-row-doc",
            XmlDocElement.NamespaceName => "namespace-name",
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };
    }
}


internal class Tools
{
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
        var memberNameElement = memberElement.GetByDataId(XmlDocElement.MemberName);
        var content = ParseStringContent(memberNameElement);

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }

    internal static string GetSummaryDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(XmlDocElement.SummaryDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetRemarksDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(XmlDocElement.RemarksDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetValueDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(XmlDocElement.ValueDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetReturnTypeName(IElement memberElement)
    {
        var returnTypeElement = memberElement.GetByDataId(XmlDocElement.ReturnTypeName);
        return ParseStringContent(returnTypeElement);
    }

    internal static string GetReturnsDoc(IElement memberElement)
    {
        var returnsDocElement = memberElement.GetByDataId(XmlDocElement.ReturnsDoc);
        return ParseStringContent(returnsDocElement);
    }

    internal static IElement[] GetMemberParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(XmlDocElement.ParameterElement)];
    }

    internal static string GetParameterName(IElement paramElement)
    {
        var paramNameElement = paramElement.GetByDataId(XmlDocElement.ParameterName);
        return ParseStringContent(paramNameElement);
    }

    internal static string GetParameterDoc(IElement paramElement)
    {
        var paramDocElement = paramElement.GetByDataId(XmlDocElement.ParameterDoc);
        return ParseStringContent(paramDocElement);
    }

    internal static IElement[] GetTypeParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(XmlDocElement.TypeParameterElement)];
    }

    internal static string GetTypeParameterName(IElement typeParamElement)
    {
        var typeParamNameElement = typeParamElement.GetByDataId(XmlDocElement.TypeParameterName);
        return ParseStringContent(typeParamNameElement);
    }

    internal static string GetTypeParameterDoc(IElement typeParamElement)
    {
        var typeParamDocElement = typeParamElement.GetByDataId(XmlDocElement.TypeParameterDoc);
        return ParseStringContent(typeParamDocElement);
    }

    internal static string[] GetAttributes(IElement element)
    {
        return element.GetByDataIds(XmlDocElement.AttributeData).Select(ParseStringContent).ToArray();
    }

    internal static string[] GetSeeAlsoDocs(IElement element)
    {
        return element.GetByDataIds(XmlDocElement.SeeAlsoDocs).Select(ParseStringContent).ToArray();
    }

    internal static IElement[] GetExceptions(IElement element)
    {
        return [.. element.GetByDataIds(XmlDocElement.ExceptionData)];
    }

    internal static string GetExceptionType(IElement paramElement)
    {
        var paramNameElement = paramElement.GetByDataId(XmlDocElement.ExceptionType);
        return ParseStringContent(paramNameElement);
    }

    internal static string GetExceptionDoc(IElement paramElement)
    {
        var paramDocElement = paramElement.GetByDataId(XmlDocElement.ExceptionDoc);
        return ParseStringContent(paramDocElement);
    }

    internal static string[] GetTypeParamConstraints(IElement element)
    {
        return element.GetByDataIds(XmlDocElement.TypeParamConstraints).Select(ParseStringContent).ToArray();
    }

    internal static string GetTypeName(IDocument document)
    {
        return ParseStringContent(document.GetTypeName());
    }

    internal static string GetBaseTypeName(IElement element)
    {
        var baseTypeElement = element.GetByDataId(XmlDocElement.BaseType);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetInterfacesString(IElement element)
    {
        var baseTypeElement = element.GetByDataId(XmlDocElement.ImplementedInterfaces);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetNamespaceString(IElement element)
    {
        var baseTypeElement = element.GetByDataId(XmlDocElement.TypeNamespace);
        return ParseStringContent(baseTypeElement);
    }

    internal static IElement[] GetNamespaceClasses(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(XmlDocElement.NamespaceClasses).GetByDataIds(XmlDocElement.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceInterfaces(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(XmlDocElement.NamespaceInterfaces).GetByDataIds(XmlDocElement.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceDelegates(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(XmlDocElement.NamespaceDelegates).GetByDataIds(XmlDocElement.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceEnums(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(XmlDocElement.NamespaceEnums).GetByDataIds(XmlDocElement.TypeRowElement)];
    }

    internal static IElement[] GetNamespaceStructs(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(XmlDocElement.NamespaceStructs).GetByDataIds(XmlDocElement.TypeRowElement)];
    }

    internal static string[] GetNamespaceNames(IDocument document)
    {
        return document.DocumentElement.GetByDataIds(XmlDocElement.NamespaceName).Select(ParseStringContent).ToArray();
    }

    internal static string[] GetNamespaceTypeNames(IElement element)
    {
        return element.GetByDataIds(XmlDocElement.TypeRowElement).Select(GetTypeRowName).ToArray();
    }

    internal static string GetTypeRowName(IElement element)
    {
        var baseTypeElement = element.GetByDataId(XmlDocElement.TypeRowName);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetTypeRowDoc(IElement element)
    {
        var baseTypeElement = element.GetByDataId(XmlDocElement.TypeRowDoc);
        return ParseStringContent(baseTypeElement);
    }
}

