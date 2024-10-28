using RefDocGen.MemberData.Abstract;
using RefDocGen.TemplateGenerators.Default.TemplateModels;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// A class used for generating RazorLight templates using the <see cref="ClassTemplateModel"/> as a template model
/// </summary>
internal class DefaultTemplateGenerator : RazorLightTemplateGenerator<ClassTemplateModel>
{
    /// <summary>
    /// The instance used for building template models.
    /// </summary>
    private readonly DefaultTemplateModelBuilder templateModelBuilder = new();

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
    protected override IEnumerable<ClassTemplateModel> GetTemplateModels(IReadOnlyList<IClassData> classes)
    {
        return classes.Select(templateModelBuilder.CreateClassTemplateModel);
    }
}
