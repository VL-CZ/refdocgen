namespace RefDocGen.CodeElements;

/// <summary>
/// Represents a .NET assembly.
/// </summary>
/// <param name="Name">Name of the assembly (without file extension)</param>
/// <param name="Namespaces">Enumerable of namespaces contained in the assembly.</param>
public record AssemblyData(string Name, IEnumerable<NamespaceData> Namespaces);
