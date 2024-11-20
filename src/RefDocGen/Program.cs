using RefDocGen.TemplateGenerators.Default;

namespace RefDocGen;

/// <summary>
/// Program class, containing main method
/// </summary>
public static class Program
{
    /// <summary>
    /// Main method, entry point of the RefDocGen app
    /// </summary>
    public static void Main()
    {
        string? rootPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
        string dllPath = Path.Join(rootPath, "demo-lib", "MyLibrary.dll");
        string docPath = Path.Join(rootPath, "demo-lib", "MyLibrary.xml");

        string projectPath = Path.Join(rootPath, "src", "RefDocGen");
        string templatePath = "TemplateGenerators/Default/Templates/Default";
        string outputDir = Path.Combine(projectPath, "out");

        var templateGenerator = new DefaultTemplateGenerator(projectPath, templatePath, outputDir);

        var docGenerator = new DocGenerator(dllPath, docPath, templateGenerator);
        docGenerator.GenerateDoc();

        Console.WriteLine("Done...");
    }
}
