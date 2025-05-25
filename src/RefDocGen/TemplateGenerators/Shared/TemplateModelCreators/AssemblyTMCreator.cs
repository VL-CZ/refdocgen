using RefDocGen.CodeElements;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Assemblies;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the assemblies of a program.
/// </summary>
internal class AssemblyTMCreator : BaseTMCreator
{
    private readonly NamespaceTMCreator nsTMCreator;

    public AssemblyTMCreator(IDocCommentTransformer docCommentTransformer, IReadOnlyDictionary<Language, ILanguageSpecificData> languageSpecificData)
        : base(docCommentTransformer, languageSpecificData)
    {
        nsTMCreator = new(docCommentTransformer, languageSpecificData);
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
