namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record MethodTemplateModel(string Name, MethodParameterTemplateModel[] Parameters, string ReturnType, string DocComment, string[] Modifiers);
