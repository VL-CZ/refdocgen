using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System;
using System.Globalization;

namespace RefDocGen.TemplateGenerators.Tools;

/// <summary>
/// Class responsible for resolving URL of the type's documentation page.
/// </summary>
/// <remarks>
/// Creates links to both same assembly and standard library types.
/// </remarks>
internal class TypeUrlResolver
{
    /// <summary>
    /// The registry of types declared in the analyzed assemblies.
    /// </summary>
    private readonly ITypeRegistry typeRegistry;

    /// <summary>
    /// Base URI of the .NET API docs.
    /// </summary>
    private readonly Uri dotnetApiDocs = new("https://learn.microsoft.com/dotnet/api/");

    /// <summary>
    /// Creates a new instance of <see cref="TypeUrlResolver"/> class.
    /// </summary>
    /// <param name="typeRegistry">
    /// The registry of types declared in the analyzed assemblies.
    /// </param>
    public TypeUrlResolver(ITypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    /// <summary>
    /// Gets the URL of the documentation page of the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type for which the documentation page URL is returned.</param>
    /// <returns>
    /// URL of the documentation page of the provided <paramref name="type"/>. <c>null</c> if the documentation page is not found.
    /// </returns>
    public string? GetUrlOf(ITypeNameData type)
    {
        string typeId = type.HasTypeParameters
                ? $"{type.FullName}`{type.TypeParameters.Count}"
                : type.Id;

        if (typeRegistry.GetDeclaredType(typeId) is not null) // the type is found in the type registry
        {
            string uriEncodedString = Uri.EscapeDataString(typeId);
            return $"./{uriEncodedString}.html";
        }
        else if (GetSystemTypeUrl(type) is string url) // the type is found in the .NET API docs
        {
            return url;
        }

        return null;
    }

    /// <summary>
    /// Gets the URL of the .NET API docs page of the provided <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type for which the documentation page URL is returned.</param>
    /// <returns>
    /// URL of the documentation page of the provided <paramref name="type"/>.
    /// <c>null</c> if the type isn't contained in .NET API docs.
    /// </returns>
    /// <seealso cref="dotnetApiDocs"/>
    private string? GetSystemTypeUrl(ITypeNameData type)
    {
        const string systemNs = "System";
        string rootTypeNamespace = type.Namespace.Split('.').First();

        if (rootTypeNamespace == systemNs)
        {
            string typeUrl = type.FullName.ToLower(CultureInfo.InvariantCulture).Replace('`', '-'); // The .NET API docs use the following convention
            return new Uri(dotnetApiDocs, typeUrl).ToString();
        }

        return null;
    }
}
