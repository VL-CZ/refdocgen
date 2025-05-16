namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

public record SearchPageTM(string Id, string Name, string DocComment) : ITemplateModelWithId
{
    public string Url => $"{Id}.html";
};
