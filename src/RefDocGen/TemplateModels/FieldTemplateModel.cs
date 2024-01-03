namespace RefDocGen.TemplateModels;

public record FieldTemplateModel(string Name, string Type, string DocComment)
{
    public string DocComment { get; set; } = DocComment;
}
