using AngleSharp.Dom;

namespace RefDocGen.IntegrationTests.Tools;

/// <summary>
/// A class that contains additional tool methods related to all <c>type</c> pages.
/// </summary>
internal class TypePageTools
{

    /// <summary>
    /// Gets signature of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns></returns>
    internal static string GetMemberSignature(IElement memberElement)
    {
        var memberNameElement = memberElement.GetByDataId(DataId.MemberName);
        string content = memberNameElement.GetParsedContent();

        if (content.EndsWith(" #", StringComparison.InvariantCulture)) // remove the anchor tag
        {
            content = content[..^2];
        }

        return content;
    }

    /// <summary>
    /// Gets the <c>summary</c> doc comment of the type/member.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns><c>summary</c> doc comment of the type/member.</returns>
    internal static string GetSummaryDoc(IElement element)
    {
        return element.GetParsedContent(DataId.SummaryDoc);
    }

    /// <summary>
    /// Gets the <c>remarks</c> doc comment of the type/member.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns><c>remarks</c> doc comment of the type/member.</returns>
    internal static string GetRemarksDoc(IElement element)
    {
        return element.GetParsedContent(DataId.RemarksDoc);
    }

    /// <summary>
    /// Gets the <c>value</c> doc comment of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns><c>value</c> doc comment of the member.</returns>
    internal static string GetValueDoc(IElement memberElement)
    {
        return memberElement.GetParsedContent(DataId.ValueDoc);
    }

    /// <summary>
    /// Gets the return type name of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns>The return type name.</returns>
    internal static string GetReturnTypeName(IElement memberElement)
    {
        return memberElement.GetParsedContent(DataId.ReturnTypeName);
    }

    /// <summary>
    /// Gets the <c>returns</c> doc comment of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns><c>returns</c> doc comment of the member.</returns>
    internal static string GetReturnsDoc(IElement memberElement)
    {
        return memberElement.GetParsedContent(DataId.ReturnsDoc);
    }

    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    /// <param name="paramElement">The HTML element representing the parameter.</param>
    /// <returns>The parameter name.</returns>
    internal static string GetParameterName(IElement paramElement)
    {
        return paramElement.GetParsedContent(DataId.ParameterName);
    }

    /// <summary>
    /// Gets the <c>param</c> doc comment of the parameter.
    /// </summary>
    /// <param name="paramElement">The HTML element representing the parameter.</param>
    /// <returns><c>param</c> doc comment of the parameter.</returns>
    internal static string GetParameterDoc(IElement paramElement)
    {
        return paramElement.GetParsedContent(DataId.ParameterDoc);
    }

    /// <summary>
    /// Gets the name of the type parameter.
    /// </summary>
    /// <param name="typeParamElement">The HTML element representing the type parameter.</param>
    /// <returns>The type parameter name.</returns>
    internal static string GetTypeParameterName(IElement typeParamElement)
    {
        return typeParamElement.GetParsedContent(DataId.TypeParameterName);
    }

    /// <summary>
    /// Gets the <c>typeparam</c> doc comment of the type parameter.
    /// </summary>
    /// <param name="typeParamElement">The HTML element representing the type parameter.</param>
    /// <returns><c>typeparam</c> doc comment of the type parameter.</returns>
    internal static string GetTypeParameterDoc(IElement typeParamElement)
    {
        return typeParamElement.GetParsedContent(DataId.TypeParameterDoc);
    }

    /// <summary>
    /// Gets the type of the exception.
    /// </summary>
    /// <param name="paramElement">The HTML element representing the exception.</param>
    /// <returns>The exception type.</returns>
    internal static string GetExceptionType(IElement paramElement)
    {
        return paramElement.GetParsedContent(DataId.ExceptionType);
    }

    /// <summary>
    /// Gets the <c>exception</c> doc comment of the exception.
    /// </summary>
    /// <param name="paramElement">The HTML element representing the exception.</param>
    /// <returns><c>exception</c> doc comment of the exception.</returns>
    internal static string GetExceptionDoc(IElement paramElement)
    {
        return paramElement.GetParsedContent(DataId.ExceptionDoc);
    }

    /// <summary>
    /// Gets the name of the type's base type.
    /// </summary>
    /// <param name="element">The HTML element representing the type.</param>
    /// <returns>The base type name.</returns>
    internal static string GetBaseTypeName(IElement element)
    {
        return element.GetParsedContent(DataId.BaseType);
    }

    /// <summary>
    /// Gets the collection of implemented interfaces by the type as a string.
    /// </summary>
    /// <param name="element">The HTML element representing the type.</param>
    /// <returns>The implemented interfaces as a string.</returns>
    internal static string GetInterfacesString(IElement element)
    {
        return element.GetParsedContent(DataId.ImplementedInterfaces);
    }

    /// <summary>
    /// Gets the namespace of the type as a string.
    /// </summary>
    /// <param name="element">The HTML element representing the type.</param>
    /// <returns>The namespace as a string.</returns>
    internal static string GetNamespaceString(IElement element)
    {
        return element.GetParsedContent(DataId.TypeNamespace);
    }

    /// <summary>
    /// Gets the parameters of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns>An array of parameter HTML elements.</returns>
    internal static IElement[] GetMemberParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(DataId.ParameterElement)];
    }

    /// <summary>
    /// Gets the type parameters of the member.
    /// </summary>
    /// <param name="memberElement">The HTML element representing the member.</param>
    /// <returns>An array of type parameter HTML elements.</returns>
    internal static IElement[] GetTypeParameters(IElement memberElement)
    {
        return [.. memberElement.GetByDataIds(DataId.TypeParameterElement)];
    }

    /// <summary>
    /// Gets the attributes applied to the type/member.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns>An array of attribute names.</returns>
    internal static string[] GetAttributes(IElement element)
    {
        return element.GetByDataIds(DataId.AttributeData).Select(e => e.GetParsedContent()).ToArray();
    }

    /// <summary>
    /// Gets the <c>seealso</c> doc comments associated with the type/member.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns>An array of <c>seealso</c> doc comments.</returns>
    internal static string[] GetSeeAlsoDocs(IElement element)
    {
        return element.GetByDataIds(DataId.SeeAlsoDocs).Select(e => e.GetParsedContent()).ToArray();
    }

    /// <summary>
    /// Gets the exceptions associated with the type/member.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns>An array of exception elements.</returns>
    internal static IElement[] GetExceptions(IElement element)
    {
        return [.. element.GetByDataIds(DataId.ExceptionData)];
    }

    /// <summary>
    /// Gets the type parameter constraints.
    /// </summary>
    /// <param name="element">The HTML element representing the type/member.</param>
    /// <returns>An array of type parameter constraint strings.</returns>
    internal static string[] GetTypeParamConstraints(IElement element)
    {
        return element.GetByDataIds(DataId.TypeParamConstraints).Select(e => e.GetParsedContent()).ToArray();
    }

    /// <summary>
    /// Gets the declared type signature.
    /// </summary>
    /// <param name="document">The document representing the type.</param>
    /// <returns>The declared type signature.</returns>
    internal static string GetTypeSignature(IDocument document)
    {
        return document.DocumentElement.GetByDataId(DataId.DeclaredTypeSignature).GetParsedContent();
    }
}
