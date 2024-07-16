using RazorLight;
using RefDocGen.TemplateModels;

namespace RefDocGen;


public class RazorTemplateGenerator
{
    private readonly string projectPath = @"C:\Users\vojta\UK\mgr-thesis\refdocgen\src\RefDocGen"; // TODO: use relative path

    // Path to your Razor template file
    private readonly string templatePath = "Templates/Template.cshtml";

    private readonly string outputDir;

    private readonly RazorLightEngine razorLightEngine;

    public RazorTemplateGenerator()
    {
        outputDir = Path.Combine(projectPath, "out"); // TODO: use relative path
        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public void GenerateTemplates(ClassTemplateModel[] templateModels)
    {
        foreach (var model in templateModels)
        {
            string outputFileName = Path.Join(outputDir, $"{model.Name}.html");

            var task = razorLightEngine.CompileRenderAsync(templatePath, model);
            task.Wait(); // TODO: consider using Async
            string result = task.Result;

            File.WriteAllText(outputFileName, result);
        }
    }
}
