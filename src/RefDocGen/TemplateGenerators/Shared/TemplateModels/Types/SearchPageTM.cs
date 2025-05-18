namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

public record SearchPageTM(string Name, string DocComment, string Id, string? Fragment = null) : ITemplateModelWithId
{
    public string Url =>
        Fragment is not null
        ? $"api/{Id}.html#{Fragment}"
        : $"api/{Id}.html";
};
