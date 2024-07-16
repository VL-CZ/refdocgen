namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record ClassTemplateModel(string Name, string DocComment, IEnumerable<string> Modifiers, FieldTemplateModel[] Fields,
    PropertyTemplateModel[] Properties, MethodTemplateModel[] Methods);
