namespace RefDocGen.TemplateModels;

public interface ITypeModel
{
    string Name { get; }
}

public record ClassTemplateModel(string Name, string DocComment, string[] Modifiers, FieldTemplateModel[] Fields, PropertyTemplateModel[] Properties, MethodTemplateModel[] Methods) : ITypeModel
{
}
