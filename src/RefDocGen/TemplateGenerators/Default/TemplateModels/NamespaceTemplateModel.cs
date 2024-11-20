namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model for a namespace.
/// </summary>
/// <param name="Name">The name of the namespace.</param>
/// <param name="Types">Types contained in the namespace, ordered by their name.</param>
public record NamespaceTemplateModel(string Name, IEnumerable<TypeNameTemplateModel> Types);

/// <summary>
/// Represents the template model for a type, including its name and other data.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="Kind">Kind of the type.</param>
/// <param name="Name">Name of the type.</param>
/// <param name="DocComment">Documentation comment for the type.</param>
public record TypeNameTemplateModel(string Id, string Kind, string Name, string DocComment);
