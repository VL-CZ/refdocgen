using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;

namespace RefDocGen.Config;

/// <summary>
/// A common interface for both command-line and YAML configuration of <c>RefDocGen</c>.
/// </summary>
internal interface IProgramConfiguration
{
    /// <summary>
    /// The assembly, project, or solution to document, or a YAML configuration file.
    /// </summary>
    string Input { get; }

    /// <summary>
    /// Force create the documentation (i.e. delete the output directory if it already exists).
    /// </summary>
    bool ForceCreate { get; }

    /// <summary>
    /// Specifies which inherited members to include in the documentation.
    /// </summary>
    MemberInheritanceMode InheritMembers { get; }

    /// <summary>
    /// Minimum visibility level of types and members to include in the documentation.
    /// </summary>
    AccessModifier MinVisibility { get; }

    /// <summary>
    /// Namespaces to exclude from the documentation.
    /// </summary>
    IEnumerable<string> ExcludeNamespaces { get; }

    /// <summary>
    /// The output directory for the generated documentation.
    /// </summary>
    string OutputDir { get; }

    /// <summary>
    /// Projects to exclude from the documentation.
    /// </summary>
    IEnumerable<string> ExcludeProjects { get; }

    /// <summary>
    /// Directory containing additional static pages to include in the documentation.
    /// </summary>
    string? StaticPagesDir { get; }

    /// <summary>
    /// The template to use for the documentation.
    /// </summary>
    DocumentationTemplate Template { get; }

    /// <summary>
    /// Enable verbose output.
    /// </summary>
    bool Verbose { get; }

    /// <summary>
    /// Generate a specific version of the documentation.
    /// </summary>
    string? DocVersion { get; }

    /// <summary>
    /// Indicates that the current configuration should be saved to a YAML file.
    /// </summary>
    bool SaveConfig { get; }
}

