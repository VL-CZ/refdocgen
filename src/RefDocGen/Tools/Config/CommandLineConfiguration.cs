using CommandLine;
using CommandLine.Text;
using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;

namespace RefDocGen.Tools.Config;

/// <summary>
/// Represents the command-line configuration of the program.
/// </summary>
internal class CommandLineConfiguration : IConfiguration
{
    [Usage(ApplicationAlias = "refdocgen INPUT [OPTIONS]")]
    public static IEnumerable<Example> Examples => [
        new("Generate reference documentation", new object())
    ];

    [Value(0, Required = true, MetaName = "INPUT", HelpText = "The assembly, project, or solution to document.")]
    public required string Input { get; set; }

    [Option('o', "output-dir", HelpText = "The output directory for the generated documentation.", Default = DefaultConfigValues.OutputDir, MetaValue = "DIR")]
    public required string OutputDir { get; set; }

    [Option('t', "template", HelpText = "The template to use for the documentation.", Default = DefaultConfigValues.Template, MetaValue = "TEMPLATE")]
    public DocumentationTemplate Template { get; set; }

    [Option('v', "verbose", HelpText = "Enable verbose output.", Default = false)]
    public bool Verbose { get; set; }

    [Option('f', "force-create", HelpText = "Forces the creation of the documentation. If the output directory already exists, it will be deleted first.", Default = false)]
    public bool ForceCreate { get; set; }

    [Option('s', "save-config", HelpText = "Save the current configuration into a YAML file.", Default = false)]
    public bool SaveConfig { get; set; }

    [Option("static-pages-dir", HelpText = "Directory containing additional static pages to include in the documentation.", Default = null, MetaValue = "DIR")]
    public string? StaticPagesDir { get; set; }

    [Option("doc-version", HelpText = "Generate a specific version of the documentation.", Default = null, MetaValue = "VERSION")]
    public string? DocVersion { get; set; }

    [Option("min-visibility", HelpText = "Minimum visibility level of types and members to include in the documentation.",
        Default = DefaultConfigValues.MinVisibility, MetaValue = "VISIBILITY")]
    public AccessModifier MinVisibility { get; set; }

    [Option("inherit-members", Default = DefaultConfigValues.InheritMembers, MetaValue = "MODE",
        HelpText = "Specify which inherited members to include in the documentation.")]
    public MemberInheritanceMode InheritMembers { get; set; }

    [Option("exclude-projects", HelpText = "Projects to exclude from the documentation.", MetaValue = "PROJECT [PROJECT...]")]
    public required IEnumerable<string> ExcludeProjects { get; set; }

    [Option("exclude-namespaces", HelpText = "Namespaces to exclude from the documentation.", MetaValue = "NAMESPACE [NAMESPACE...]")]
    public required IEnumerable<string> ExcludeNamespaces { get; set; }
}
