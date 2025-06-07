using RefDocGen.TemplateProcessors.Shared.TemplateModels.Namespaces;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModels.Assemblies;

/// <summary>
/// Represents template model for an assembly.
/// </summary>
/// <param name="Name">Name of the assembly.</param>
/// <param name="Namespaces">Namespaces contained within the assembly.</param>
public record AssemblyTM(string Name, IEnumerable<NamespaceTM> Namespaces) : ITemplateModelWithId
{
    /// <inheritdoc/>
    public string Id => $"{Name}-DLL";
}
