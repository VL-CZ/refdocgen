using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools;
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
    internal TypeUrlResolver(ITypeRegistry typeRegistry)
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
    internal string? GetUrlOf(ITypeNameData type)
    {
        if (type.IsArray || type.IsPointer) // array or pointer -> select base element type (i.e. `int` instead of `int[]`)
        {
            type = type.TypeObject.GetBaseElementType().GetTypeNameData();
        }

        string typeId = type.HasTypeParameters // TODO: fix
                ? $"{type.FullName}`{type.TypeParameters.Count}"
                : type.Id;

        return GetUrlOf(typeId);
    }

    /// <inheritdoc cref="GetUrlOf(ITypeNameData)" path="/summary"/>
    /// <inheritdoc cref="GetUrlOf(ITypeNameData)" path="/returns"/>
    /// <param name="typeId">ID of the type for which the documentation page URL is returned.</param>
    /// <param name="memberId">
    /// ID of the member for which the documentation page URL is returned.
    /// Pass <c>null</c> if the type documentation should be returned.
    /// </param>
    internal string? GetUrlOf(string typeId, string? memberId = null)
    {
        if (typeRegistry.GetDeclaredType(typeId) is not null) // the type is found in the type registry
        {
            string uriEncodedString = Uri.EscapeDataString(typeId);

            if (memberId is not null)
            {
                return $"./{uriEncodedString}.html#{memberId}";
            }
            else
            {
                return $"./{uriEncodedString}.html";
            }
        }
        else if (GetSystemTypeUrl(typeId) is string url) // the type is found in the .NET API docs
        {
            if (memberId is not null)
            {
                return $"{url}.{memberId.ToLower(CultureInfo.InvariantCulture)}";
            }
            else
            {
                return url;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the URL of the .NET API docs page of the provided type.
    /// </summary>
    /// <param name="typeId">ID of the type for which the documentation page URL is returned.</param>
    /// <returns>
    /// URL of the documentation page of the desired type.
    /// <c>null</c> if the type isn't contained in .NET API docs.
    /// </returns>
    /// <seealso cref="dotnetApiDocs"/>
    private string? GetSystemTypeUrl(string typeId)
    {
        const string systemNs = "System";
        string rootTypeNamespace = typeId.Split('.').First();

        if (rootTypeNamespace == systemNs)
        {
            string typeUrl = typeId.ToLower(CultureInfo.InvariantCulture).Replace('`', '-'); // The .NET API docs use the following convention (TODO: fix)
            return new Uri(dotnetApiDocs, typeUrl).ToString();
        }

        return null;
    }
}
