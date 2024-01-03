namespace RefDocGen.TemplateModels;

public record ClassTemplateModel(string Name, string DocComment, FieldTemplateModel[] Fields, MethodTemplateModel[] Methods)
{
    public string DocComment { get; set; } = DocComment;
}
