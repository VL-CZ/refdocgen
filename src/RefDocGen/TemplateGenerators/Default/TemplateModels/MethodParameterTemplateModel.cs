namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record MethodParameterTemplateModel(string Name, string Type, IReadOnlyCollection<string> Modifiers);
