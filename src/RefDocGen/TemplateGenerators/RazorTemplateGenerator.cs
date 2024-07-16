using RazorLight;
using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators;

public abstract class RazorTemplateGenerator<T> : ITemplateGenerator where T : INamedTemplateModel
{
    // Path to your Razor template file
    private readonly string templatePath;

    private readonly string outputDir;

    private readonly RazorLightEngine razorLightEngine;

    public RazorTemplateGenerator(string projectPath, string templatePath, string outputDir)
    {
        this.templatePath = templatePath;
        this.outputDir = outputDir;

        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

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

    protected abstract IEnumerable<T> GetTemplateModels(ClassData[] classes);
}
