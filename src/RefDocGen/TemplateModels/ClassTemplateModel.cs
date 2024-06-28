namespace RefDocGen.TemplateModels;

public record ClassTemplateModel(string Name, string DocComment, string[] Modifiers, FieldTemplateModel[] Fields, PropertyTemplateModel[] Properties, MethodTemplateModel[] Methods)
{
}
