using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Default.TemplateModels;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class used for generating RazorLight templates using the <see cref="TypeTemplateModel"/> as a type template model and <see cref="NamespaceTemplateModel"/>.
/// </summary>
internal class DefaultTemplateGenerator : RazorLightTemplateGenerator<TypeTemplateModel, NamespaceTemplateModel>
{
    /// <summary>
    /// Create a new instance of <see cref="DefaultTemplateGenerator"/> class
    /// </summary>
    /// <param name="projectPath">Path to the project root directory.</param>
    /// <param name="templatePath">Path to the Razor template file.</param>
    /// <param name="outputDir">RazorLight engine used for generating the templates.</param>
    public DefaultTemplateGenerator(string projectPath, string templatePath, string outputDir) : base(projectPath, templatePath, outputDir)
    {
    }

    /// <inheritdoc/>
    protected override IEnumerable<NamespaceTemplateModel> GetNamespaceTemplateModels(IReadOnlyList<ITypeData> types)
    {
        return NamespaceListTemplateModelCreator.GetFrom(types);
    }

    /// <inheritdoc/>
    protected override IEnumerable<TypeTemplateModel> GetTypeTemplateModels(IReadOnlyList<ITypeData> types)
    {
        return types.Select(TypeTemplateModelCreator.GetFrom);
    }
}
