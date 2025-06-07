using RefDocGen.CodeElements;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Assemblies;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the assemblies of a program.
/// </summary>
internal class AssemblyTMCreator : BaseTMCreator
{
    /// <summary>
    /// A creator responsible for creating <see cref="TemplateModels.Namespaces.NamespaceTM"/> instances.
    /// </summary>
    private readonly NamespaceTMCreator nsTMCreator;

    public AssemblyTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
    {
        nsTMCreator = new(docCommentTransformer, availableLanguages);
    }

    /// <summary>
    /// Creates an enumerable of <see cref="AssemblyTM"/> instances based on the provided <see cref="AssemblyData"/>.
    /// </summary>
    /// <param name="assemblyData">The <see cref="AssemblyData"/> instance representing the assembly.</param>
    /// <returns>An <see cref="AssemblyTM"/> instance based on the provided <paramref name="assemblyData"/>.</returns>
    internal AssemblyTM GetFrom(AssemblyData assemblyData)
    {
        return new AssemblyTM(assemblyData.Name, assemblyData.Namespaces.OrderBy(n => n.Name).Select(nsTMCreator.GetFrom));
    }
}
