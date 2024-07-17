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
        string? rootPath = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName;
        string dllPath = Path.Join(rootPath, "demo-lib", "MyLibrary.dll");
        string docPath = Path.Join(rootPath, "demo-lib", "MyLibrary.xml");

        string projectPath = @"C:\Users\vojta\UK\mgr-thesis\refdocgen\src\RefDocGen";
        string templatePath = "TemplateGenerators/Default/Templates/Default/Template.cshtml";
        string outputDir = Path.Combine(projectPath, "out");

        var templateGenerator = new DefaultTemplateGenerator(projectPath, templatePath, outputDir);

        var docGenerator = new DocGenerator(dllPath, docPath, templateGenerator);
        docGenerator.GenerateDoc();
    }
}
