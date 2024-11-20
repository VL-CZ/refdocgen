using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default;

internal class NamespaceListTemplateModelCreator
{
    public static IEnumerable<NamespaceTemplateModel> TransformToNamespaceModels(IReadOnlyList<ITypeData> typeData)
    {
        var grouped = typeData.GroupBy(typeData => typeData.Namespace);

        var models = new List<NamespaceTemplateModel>();

        foreach (var group in grouped)
        {
            if (group.Key is not null)
            {
                var types = group.Select(t => new TypeRow(
                    t.Id,
                    t.Kind.GetName(),
                    CSharpTypeName.Of(t),
                    t.DocComment.Value));

                models.Add(new NamespaceTemplateModel(group.Key, types));
            }
        }

        return models;
    }
}
