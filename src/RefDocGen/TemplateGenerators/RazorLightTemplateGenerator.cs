using RazorLight;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// An abstract class used for generating any templates using the RazorLight library (for further info, see <see href="https://github.com/toddams/RazorLight"/>).
/// <para>
///     Template models are of a user-defined type, and constructed from <see cref="IClassData"/> objects
/// </para>
/// </summary>
/// <typeparam name="T">Type of the template model class</typeparam>
internal abstract class RazorLightTemplateGenerator<T> : ITemplateGenerator where T : INamedTemplateModel
{
    /// <summary>
    /// Path to the Razor template file.
    /// </summary>
    private readonly string templatePath;

    /// <summary>
    /// Directory, where the generated output will be stored.
    /// </summary>
    private readonly string outputDir;

    /// <summary>
    /// RazorLight engine used for generating the templates.
    /// </summary>
    private readonly RazorLightEngine razorLightEngine;

    /// <summary>
    /// Initialize a new instance of <see cref="RazorLightTemplateGenerator{T}"/> class.
    /// </summary>
    /// <param name="projectPath">Path to the project root directory.</param>
    /// <param name="templatePath">Path to the Razor template file.</param>
    /// <param name="outputDir">RazorLight engine used for generating the templates.</param>
    protected RazorLightTemplateGenerator(string projectPath, string templatePath, string outputDir)
    {
        this.templatePath = templatePath;
        this.outputDir = outputDir;

        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    /// <inheritdoc/>
    public void GenerateTemplates(IReadOnlyList<IClassData> classes)
    {
        var templateModels = GetTemplateModels(classes);

        foreach (var model in templateModels)
        {
            string outputFileName = Path.Join(outputDir, $"{model.Name}.html");

            var task = razorLightEngine.CompileRenderAsync(templatePath, model);
            //task.Wait(); // TODO: consider using Async
            string result = task.Result;

            File.WriteAllText(outputFileName, result);
        }
    }

    /// <summary>
    /// Convert the provided <paramref name="classes"/> data to a collection of template models that will be passed to the Razor templatess.
    /// </summary>
    /// <param name="classes"><see cref="IClassData"/> objects to be converted into template models.</param>
    /// <returns>A collection of template models that will be passed to the Razor templates.</returns>
    protected abstract IEnumerable<T> GetTemplateModels(IReadOnlyList<IClassData> classes);
}
