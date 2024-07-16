namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record MethodParameterTemplateModel(string Name, string Type, string DocComment, IReadOnlyCollection<string> Modifiers);
