using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types.TypeName;

namespace RefDocGen.TemplateGenerators.Tools.TypeName;

internal class TypeLink
{
    private ITypeRegistry typeRegistry;

    private Dictionary<string, Uri> links = new()
    {
        ["System"] = new Uri("https://learn.microsoft.com/en-us/dotnet/api/")
    };

    public TypeLink(ITypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    public string? GetLink(ITypeNameData typeNameData)
    {
        string parentId = typeNameData.HasTypeParameters
                ? $"{typeNameData.FullName}`{typeNameData.TypeParameters.Count}"
                : typeNameData.Id;

        // the parent type is contained in the type registry
        if (typeRegistry.GetDeclaredType(parentId) is not null)
        {
            return parentId + ".html";
        }
        else if (GetUrl(typeNameData) is string url)
        {
            return url;
        }

        return null;
    }

    private string? GetUrl(ITypeNameData type)
    {
        var parentNamespace = type.Namespace.Split('.').First();

        if (links.TryGetValue(parentNamespace, out var url))
        {
            return url + type.FullName.Replace('`', '-'); // TODO: update
        }

        return null;
    }
}
