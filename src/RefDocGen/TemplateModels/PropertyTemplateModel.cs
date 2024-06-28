namespace RefDocGen.TemplateModels;

public record PropertyTemplateModel(string Name, string Type, string DocComment, string[] Modifiers, bool HasGetter, bool HasSetter, string[] GetterModifiers, string[] SetterModifiers)
{
}
