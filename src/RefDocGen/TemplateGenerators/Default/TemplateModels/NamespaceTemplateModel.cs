namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

public record NamespaceTemplateModel(string Namespace, IEnumerable<TypeRow> Types);

public record TypeRow(string Id, string Kind, string Name, string DocComment);
