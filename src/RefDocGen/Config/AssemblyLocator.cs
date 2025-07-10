using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Exceptions;
using RefDocGen.Tools.Exceptions;

namespace RefDocGen.Config;

/// <summary>
/// Class responsible for locating assemblies.
/// </summary>
internal static class AssemblyLocator
{
    /// <summary>
    /// Extensions of .NET assembly files.
    /// </summary>
    private static readonly string[] assemblyExtensions = [".dll", ".exe"];

    /// <summary>
    /// Extensions of .NET solution files.
    /// </summary>
    private static readonly string[] solutionFileExtensions = [".sln", ".slnx"];

    /// <summary>
    /// Gets assemblies based on the provided input path.
    /// </summary>
    /// <param name="inputPath">Path to the assembly/.NET project/solution file.</param>
    /// <returns>
    /// <list type="bullet">
    /// <item>If the <paramref name="inputPath"/> refers to an assembly, it is simply returned in an array.</item>
    /// <item>For .NET projects, an array containing its assembly path is returned.</item>
    /// <item>For solutions, an array containing all project assemblies is returned.</item>
    /// </list>
    /// </returns>
    public static string[] GetAssemblies(string inputPath)
    {
        if (!Path.IsPathRooted(inputPath))
        {
            inputPath = Path.GetFullPath(inputPath); // use full path, if the provided one is relative
        }

        string extension = Path.GetExtension(inputPath);

        if (assemblyExtensions.Contains(extension)) // assembly
        {
            return [inputPath];
        }
        else if (solutionFileExtensions.Contains(extension)) // solution
        {
            try
            {
                var solution = SolutionFile.Parse(inputPath);
                var projectPaths = solution.ProjectsInOrder.Select(p => p.AbsolutePath);

                return [.. projectPaths.Select(GetProjectAssembly)];
            }
            catch (InvalidProjectFileException e)
            {
                throw new SolutionNotLoadedException(inputPath, e); // cannot load the solution
            }
        }
        else // project
        {
            return [GetProjectAssembly(inputPath)];
        }
    }

    /// <summary>
    /// Gets path to the project assembly.
    /// </summary>
    /// <param name="projectPath">Path to the project.</param>
    /// <returns></returns>
    /// <exception cref="ProjectNotLoadedException">Thrown if the project is not found or cannot be loaded.</exception>
    private static string GetProjectAssembly(string projectPath)
    {
        try
        {
            var globalProperties = new Dictionary<string, string>
            {
                ["Configuration"] = "Debug",
                ["Platform"] = "AnyCPU"
            };

            var project = new Project(projectPath, globalProperties, null);

            string outputPath = project.GetPropertyValue("OutputPath").Replace('\\', Path.DirectorySeparatorChar);
            string assemblyName = project.GetPropertyValue("AssemblyName");
            string outputType = project.GetPropertyValue("OutputType");

            string extension = outputType switch
            {
                "Exe" or "WinExe" => ".exe",
                _ => ".dll"
            };

            string outputFile = Path.Combine(project.DirectoryPath, outputPath, assemblyName + extension);

            if (extension == ".exe")
            {
                string outputDll = Path.ChangeExtension(outputFile, ".dll");
                if (Path.Exists(outputDll)) // check if the DLL with the same name exists, if yes -> return it
                {
                    return outputDll;
                }
            }

            return outputFile;
        }
        catch (InvalidProjectFileException e)
        {
            throw new ProjectNotLoadedException(projectPath, e); // cannot find/load the project
        }
    }
}
