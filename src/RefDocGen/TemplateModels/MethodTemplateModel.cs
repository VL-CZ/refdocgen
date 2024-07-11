namespace RefDocGen.TemplateModels;

public record MethodTemplateModel(string Name, MethodParameterModel[] Parameters, string ReturnType, string DocComment, string[] Modifiers)
{
}

public record MethodParameterModel(string Name, string Type, IReadOnlyCollection<string> Modifiers);
