using RazorLight;
using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Default;


public class DefaultTemplateGenerator : ITemplateGenerator
{
    private readonly string projectPath = @"C:\Users\vojta\UK\mgr-thesis\refdocgen\src\RefDocGen"; // TODO: use relative path

    // Path to your Razor template file
    private readonly string templatePath = "Templates/Template.cshtml";

    private readonly string outputDir;

    private readonly RazorLightEngine razorLightEngine;

    private readonly DefaultTemplateModelBuilder templateModelBuilder = new();

    public DefaultTemplateGenerator()
    {
        outputDir = Path.Combine(projectPath, "out"); // TODO: use relative path
        razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(projectPath)
            .UseMemoryCachingProvider()
            .Build();
    }

    public void GenerateTemplates(ClassData[] classes)
    {
        var templateModels = classes.Select(templateModelBuilder.CreateClassTemplateModel);

        // convert to template model

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
