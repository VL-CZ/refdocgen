namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record FieldTemplateModel(string Name, string Type, string DocComment, IEnumerable<string> Modifiers);
