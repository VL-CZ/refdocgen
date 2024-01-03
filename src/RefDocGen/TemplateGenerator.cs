using RazorLight;
using RefDocGen.TemplateModels;

namespace RefDocGen;

internal class TemplateGenerator
{
    internal void GenerateTemplates(ClassTemplateModel[] templateModels)
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(@"C:\Users\vojta\UK\mgr-thesis\refdocgen\src\RefDocGen") // TODO: use relative path
            .UseMemoryCachingProvider()
            .Build();

        // Path to your Razor template file
        string templatePath = "Templates/Template.cshtml";

        string outputDir = "C:\\Users\\vojta\\UK\\mgr-thesis\\refdocgen\\src\\RefDocGen\\out\\"; // TODO: use relative path

        foreach (var model in templateModels)
        {
            string outputFileName = Path.Join(outputDir, $"{model.Name}.html");

            var task = engine.CompileRenderAsync(templatePath, model);
            task.Wait(); // TODO: consider using Async
            string result = task.Result;

            File.WriteAllText(outputFileName, result);
        }
    }
}
