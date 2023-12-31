namespace RefDocGen;

public static class Program
{
    public static void Main()
    {
        string? rootPath = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.Parent.FullName;
        string dllPath = Path.Join(rootPath, "demo-lib", "MyLibrary.dll");
        string docPath = Path.Join(rootPath, "demo-lib", "MyLibrary.xml");

        var docGenerator = new DocGenerator(dllPath, docPath);
        docGenerator.GenerateDoc();
    }
}
