using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Namespaces;

/// <summary>
/// Represents the template model for a namespace.
/// </summary>
/// <param name="Name">The name of the namespace.</param>
/// <param name="Classes">Classes contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="ValueTypes">Value types contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="Interfaces">Interfaces contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="Enums">Enums contained in the namespace, ordered alphabetically by their name.</param>
/// <param name="Delegates">Delegates contained in the namespace, ordered alphabetically by their name.</param>
public record NamespaceTM(
    string Name,
    IEnumerable<TypeNameTM> Classes,
    IEnumerable<TypeNameTM> ValueTypes,
    IEnumerable<TypeNameTM> Interfaces,
    IEnumerable<TypeNameTM> Enums,
    IEnumerable<TypeNameTM> Delegates
    ) : ITemplateModelWithId
{
    /// <summary>
    /// An enumerable of all types contained in the namespace, ordered alphabetically by their name.
    /// </summary>
    public IEnumerable<TypeNameTM> AllTypes => Classes
        .Concat(ValueTypes)
        .Concat(Interfaces)
        .Concat(Enums)
        .Concat(Delegates)
        .OrderBy(t => t.Name.CSharpData);

    /// <inheritdoc/>
    public string Id => Name;
}
