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
/// <param name="AssemblyName">Name of the assembly that contains the namespace.</param>
public record NamespaceTM(
    string Name,
    TypeNameTM[] Classes,
    TypeNameTM[] ValueTypes,
    TypeNameTM[] Interfaces,
    TypeNameTM[] Enums,
    TypeNameTM[] Delegates,
    string AssemblyName
    ) : ITemplateModelWithId
{

#pragma warning disable IDE0305 // Simplify collection initialization

    /// <summary>
    /// An enumerable of all types contained in the namespace, ordered alphabetically by their name.
    /// </summary>
    public TypeNameTM[] AllTypes => Classes
        .Concat(ValueTypes)
        .Concat(Interfaces)
        .Concat(Enums)
        .Concat(Delegates)
        .OrderBy(t => t.Name.CSharpData)
        .ToArray();
#pragma warning restore IDE0305

    /// <inheritdoc/>
    public string Id => Name;
}
