namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model for a namespace.
/// </summary>
/// <param name="Name">The name of the namespace.</param>
/// <param name="Types">Classes contained in the namespace, ordered alphabetically by their name.</param>
public record NamespaceTM(string Name, IEnumerable<TypeNameTemplateModel> Types);

/// <summary>
/// Represents the template model for a type, including its name and other data.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="Kind">Kind of the type.</param>
/// <param name="Name">Name of the type.</param>
/// <param name="DocComment">Documentation comment for the type.</param>
public record TypeNameTemplateModel(string Id, string Kind, string Name, string DocComment);

/// <summary>
/// Represents the template model for a namespace.
/// </summary>
/// <param name="Name">The name of the namespace.</param>
/// <param name="Classes">Classes contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="ValueTypes">Value types contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="Interfaces">Interfaces contained in the namespace, ordered alphabetically by their name.</param>
public record NamespaceDetailsTM(string Name, IEnumerable<TypeNameTemplateModel> Classes,
    IEnumerable<TypeNameTemplateModel> ValueTypes, IEnumerable<TypeNameTemplateModel> Interfaces);
