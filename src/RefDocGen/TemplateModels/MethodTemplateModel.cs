namespace RefDocGen.TemplateModels;

public record MethodTemplateModel(string Name, string ReturnType, string DocComment)
{
    public string DocComment { get; set; } = DocComment;
}
