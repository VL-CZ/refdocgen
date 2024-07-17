using RazorLight;
using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators;

/// <summary>
/// An abstract class used for generating any Razor templates using the <see cref="ClassData"/> objects.
/// </summary>
/// <typeparam name="T">Type of the template model class</typeparam>
public abstract class RazorTemplateGenerator<T> : ITemplateGenerator where T : INamedTemplateModel
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
    /// Initialize a new instance of <see cref="RazorTemplateGenerator{T}"/> class.
    /// </summary>
    /// <param name="projectPath">Path to the project root directory.</param>
    /// <param name="templatePath">Path to the Razor template file.</param>
    /// <param name="outputDir">RazorLight engine used for generating the templates.</param>
    public RazorTemplateGenerator(string projectPath, string templatePath, string outputDir)
    {
        this.templatePath = templatePath;
        this.outputDir = outputDir;

        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    /// <inheritdoc/>
    public void GenerateTemplates(ClassData[] classes)
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
    /// <param name="classes"><see cref="ClassData"/> objects to be converted into template models.</param>
    /// <returns>A collection of template models that will be passed to the Razor templates.</returns>
    protected abstract IEnumerable<T> GetTemplateModels(ClassData[] classes);
}
