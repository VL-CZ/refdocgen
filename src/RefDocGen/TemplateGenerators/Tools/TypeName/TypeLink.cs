using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Tools.TypeName;

internal class TypeLink
{
    private ITypeRegistry typeRegistry;

    public TypeLink(ITypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    public string? GetLink(ITypeNameData typeNameData)
    {
        string? link = null;

        string parentId = typeNameData.HasTypeParameters
                ? $"{typeNameData.FullName}`{typeNameData.TypeParameters.Count}"
                : typeNameData.Id;

        // the parent type is contained in the type registry
        if (typeRegistry.GetDeclaredType(parentId) is not null)
        {
            link = parentId + ".html";
        }
        else if (typeNameData.Namespace.StartsWith("System"))
        {
            var url = new Uri("https://learn.microsoft.com/en-us/dotnet/api/") + typeNameData.FullName.Replace('`', '-');
            link = url;
        }

        return link;
    }
}
