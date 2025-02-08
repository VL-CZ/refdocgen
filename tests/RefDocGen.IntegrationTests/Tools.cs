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
    internal static string ToXmlString(this XmlDocElement element) => element switch
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


internal class Tools
{
    internal const string MemberName = "member-name";
    internal const string AttributeData = "attribute-data";
    internal const string ReturnTypeName = "return-type-name";
    internal const string ParameterElement = "parameter-data";
    internal const string ParameterName = "parameter-name";
    internal const string ParameterDoc = "parameter-doc";
    internal const string TypeParameterElement = "type-parameter-data";
    internal const string TypeParameterName = "type-parameter-name";
    internal const string TypeParameterDoc = "type-parameter-doc";
    internal const string SummaryDoc = "summary-doc";
    internal const string RemarksDoc = "remarks-doc";
    internal const string ReturnsDoc = "returns-doc";
    internal const string ValueDoc = "value-doc";
    internal const string SeeAlsoDocs = "seealso-item";
    internal const string TypeParamConstraints = "type-param-constraints";
    internal const string BaseType = "base-type";
    internal const string ImplementedInterfaces = "implemented-interfaces";
    internal const string TypeNamespace = "type-namespace";
    internal const string ExceptionData = "exception-data";
    internal const string ExceptionType = "exception-type";
    internal const string ExceptionDoc = "exception-doc";
    internal const string DelegateMethod = "delegate-method";

    internal const string NamespaceClasses = "namespace-classes";
    internal const string NamespaceInterfaces = "namespace-interfaces";
    internal const string NamespaceDelegates = "namespace-delegates";
    internal const string NamespaceEnums = "namespace-enums";
    internal const string NamespaceStructs = "namespace-structs";

    internal const string TypeRowElement = "type-row-element";
    internal const string TypeRowName = "type-row-name";
    internal const string TypeRowDoc = "type-row-doc";

    internal const string NamespaceName = "namespace-name";

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
        var memberNameElement = memberElement.GetByDataId(MemberName);

        var content = ParseStringContent(memberNameElement);

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }

    internal static string GetSummaryDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(SummaryDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetRemarksDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(RemarksDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetValueDocContent(IElement memberElement)
    {
        var summaryDocElement = memberElement.GetByDataId(ValueDoc);
        return ParseStringContent(summaryDocElement);
    }

    internal static string GetReturnTypeName(IElement memberElement)
    {
        var returnTypeElement = memberElement.GetByDataId(ReturnTypeName);
        return ParseStringContent(returnTypeElement);
    }

    internal static string GetReturnsDoc(IElement memberElement)
    {
        var returnsDocElement = memberElement.GetByDataId(ReturnsDoc);
        return ParseStringContent(returnsDocElement);
    }

    internal static IElement[] GetMemberParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(ParameterElement)];
    }

    internal static string GetParameterName(IElement paramElement)
    {
        var paramNameElement = paramElement.GetByDataId(ParameterName);
        return ParseStringContent(paramNameElement);
    }

    internal static string GetParameterDoc(IElement paramElement)
    {
        var paramDocElement = paramElement.GetByDataId(ParameterDoc);
        return ParseStringContent(paramDocElement);
    }

    internal static IElement[] GetTypeParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(TypeParameterElement)];
    }

    internal static string GetTypeParameterName(IElement typeParamElement)
    {
        var typeParamNameElement = typeParamElement.GetByDataId(TypeParameterName);
        return ParseStringContent(typeParamNameElement);
    }

    internal static string GetTypeParameterDoc(IElement typeParamElement)
    {
        var typeParamDocElement = typeParamElement.GetByDataId(TypeParameterDoc);
        return ParseStringContent(typeParamDocElement);
    }


    internal static string[] GetAttributes(IElement element)
    {
        var attributes = element.GetByDataIds(AttributeData);
        return attributes.Select(ParseStringContent).ToArray();
    }

    internal static string[] GetSeeAlsoDocs(IElement element)
    {
        var attributes = element.GetByDataIds(SeeAlsoDocs);
        return attributes.Select(ParseStringContent).ToArray();
    }

    internal static IElement[] GetExceptions(IElement element)
    {
        return [.. element.GetByDataIds(ExceptionData)];
    }

    internal static string GetExceptionType(IElement paramElement)
    {
        var paramNameElement = paramElement.GetByDataId(ExceptionType);
        return ParseStringContent(paramNameElement);
    }

    internal static string GetExceptionDoc(IElement paramElement)
    {
        var paramDocElement = paramElement.GetByDataId(ExceptionDoc);
        return ParseStringContent(paramDocElement);
    }

    internal static string[] GetTypeParamConstraints(IElement element)
    {
        var attributes = element.GetByDataIds(TypeParamConstraints);
        return attributes.Select(ParseStringContent).ToArray();
    }

    internal static string GetTypeName(IDocument document)
    {
        return ParseStringContent(document.GetTypeName());
    }

    internal static string GetBaseTypeName(IElement element)
    {
        var baseTypeElement = element.GetByDataId(BaseType);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetInterfacesString(IElement element)
    {
        var baseTypeElement = element.GetByDataId(ImplementedInterfaces);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetNamespaceString(IElement element)
    {
        var baseTypeElement = element.GetByDataId(TypeNamespace);
        return ParseStringContent(baseTypeElement);
    }

    internal static IElement[] GetNamespaceClasses(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(NamespaceClasses).GetByDataIds(TypeRowElement)];
    }

    internal static IElement[] GetNamespaceInterfaces(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(NamespaceInterfaces).GetByDataIds(TypeRowElement)];
    }

    internal static IElement[] GetNamespaceDelegates(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(NamespaceDelegates).GetByDataIds(TypeRowElement)];
    }

    internal static IElement[] GetNamespaceEnums(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(NamespaceEnums).GetByDataIds(TypeRowElement)];
    }

    internal static IElement[] GetNamespaceStructs(IDocument document)
    {
        return [.. document.DocumentElement.GetByDataId(NamespaceStructs).GetByDataIds(TypeRowElement)];
    }

    internal static string[] GetNamespaceNames(IDocument document)
    {
        var namespaceNames = document.DocumentElement.GetByDataIds(NamespaceName);
        return namespaceNames.Select(ParseStringContent).ToArray();
    }

    internal static string[] GetNamespaceTypeNames(IElement element)
    {
        return element.GetByDataIds(TypeRowElement).Select(GetTypeRowName).ToArray();
    }

    internal static string GetTypeRowName(IElement element)
    {
        var baseTypeElement = element.GetByDataId(TypeRowName);
        return ParseStringContent(baseTypeElement);
    }

    internal static string GetTypeRowDoc(IElement element)
    {
        var baseTypeElement = element.GetByDataId(TypeRowDoc);
        return ParseStringContent(baseTypeElement);
    }
}
