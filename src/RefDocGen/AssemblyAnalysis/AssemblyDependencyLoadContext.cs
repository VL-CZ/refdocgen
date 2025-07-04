using System.Reflection;
using System.Runtime.Loader;

namespace RefDocGen.AssemblyAnalysis;

/// <summary>
/// Represents a load context of an assembly together with its dependencies.
/// </summary>
internal class AssemblyDependencyLoadContext : AssemblyLoadContext
{
    /// <summary>
    /// Absolute path to the folder containing the main assembly.
    /// </summary>
    private readonly string folderPath;

    /// <summary>
    /// Create a new instance of <see cref="AssemblyDependencyLoadContext"/>.
    /// </summary>
    /// <param name="folderPath">Absolute path to the folder containing the main assembly.</param>
    public AssemblyDependencyLoadContext(string folderPath) : base(isCollectible: false)
    {
        this.folderPath = folderPath;
        Resolving += ResolveFromFolder; // custom assembly resolution
    }

    /// <summary>
    /// Resolves the assembly from a folder.
    /// </summary>
    /// <param name="context">The assembly context.</param>
    /// <param name="assemblyName">Name of the assembly to resolve.</param>
    /// <returns>The loaded assembly, or <c>null</c> if the resolution failed.</returns>
    private Assembly? ResolveFromFolder(AssemblyLoadContext context, AssemblyName assemblyName)
    {
        // Look for dependency DLL in the folder (non-recursive, change if needed)
        string dependencyPath = Path.Combine(folderPath, assemblyName.Name + ".dll");

        if (File.Exists(dependencyPath)) // if the assembly is present in the folder, load it
        {
            return LoadFromAssemblyPath(dependencyPath);
        }

        return null; // cannot resolve -> return null
    }
}
