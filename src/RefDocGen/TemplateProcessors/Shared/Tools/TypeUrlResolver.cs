using System.Globalization;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.TemplateProcessors.Shared.Tools;

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
    /// Gets the URL of the documentation page of the provided <paramref name="type"/> and possibly member.
    /// </summary>
    /// <param name="type">The type for which the documentation page URL is returned.</param>
    /// <param name="memberId">
    /// ID of the member for which the documentation page URL is returned.
    /// Pass <c>null</c> if the type documentation should be returned.
    /// </param>
    /// <returns>
    /// URL of the documentation page of the provided <paramref name="type"/>, or its member. <c>null</c> if the documentation page is not found.
    /// </returns>
    internal string? GetUrlOf(ITypeNameData type, string? memberId = null)
    {
        if (type.IsArray || type.IsPointer) // array or pointer -> select base element type (i.e. `int` instead of `int[]`)
        {
            type = type.TypeObject.GetBaseElementType().GetTypeNameData();
        }

        string typeId = type.TypeDeclarationId;

        return GetUrlOf(typeId, memberId);
    }

    /// <inheritdoc cref="GetUrlOf(ITypeNameData, string)" />
    /// <param name="typeId">ID of the type for which the documentation page URL is returned.</param>
    /// <param name="memberId">
    /// ID of the member for which the documentation page URL is returned.
    /// Pass <c>null</c> if the type documentation should be returned.
    /// </param>
    internal string? GetUrlOf(string typeId, string? memberId = null)
    {
        if (typeRegistry.GetDeclaredType(typeId) is not null) // the type is found in the type registry
        {
            string uriEncodedType = TemplateId.Escape(typeId);

            if (memberId is not null)
            {
                string uriEncodedMember = TemplateId.Escape(memberId);
                return $"./{uriEncodedType}.html#{uriEncodedMember}"; // append member string
            }
            else
            {
                return $"./{uriEncodedType}.html";
            }
        }
        else if (GetSystemTypeUrl(typeId) is string url) // the type is found in the .NET API docs
        {
            if (memberId is not null) // append member string
            {
                int lastHashIndex = memberId.LastIndexOf('#');
                if (lastHashIndex > 0 && lastHashIndex < memberId.Length - 1) // remove explicit interface type (if present) from the ID string
                {
                    memberId = memberId[(lastHashIndex + 1)..];
                }

                if (memberId.TryGetIndex('(', out int parenthesisIndex)) // remove parameters string (if present)
                {
                    memberId = memberId[..parenthesisIndex];
                }

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
            string typeUrl = typeId.ToLower(CultureInfo.InvariantCulture).Replace('`', '-'); // The .NET API docs use the following convention, see https://learn.microsoft.com/en-us/contribute/content/dotnet/dotnet-style-guide
            return new Uri(dotnetApiDocs, typeUrl).ToString();
        }

        return null;
    }
}
