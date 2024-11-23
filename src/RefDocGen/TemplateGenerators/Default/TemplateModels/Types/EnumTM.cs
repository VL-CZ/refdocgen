namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

public record EnumTM(string Id, string Name, string Namespace, string DocComment, IEnumerable<string> Modifiers, IEnumerable<EnumValueTM> Values) : ITemplateModelWithId;

public record EnumValueTM(string Name, string DocComment);
