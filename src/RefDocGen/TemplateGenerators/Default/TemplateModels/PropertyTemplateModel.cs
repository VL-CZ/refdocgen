namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record PropertyTemplateModel(string Name, string Type, string DocComment, IEnumerable<string> Modifiers, bool HasGetter, bool HasSetter,
    IEnumerable<string> GetterModifiers, IEnumerable<string> SetterModifiers);
