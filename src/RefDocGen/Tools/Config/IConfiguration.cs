using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.Tools.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RefDocGen.Tools.Config;

internal static class DefaultConfigValues
{
    internal const MemberInheritanceMode InheritMembers = MemberInheritanceMode.NonObject;
    internal const AccessModifier MinVisibility = AccessModifier.Family;
    internal const string OutputDir = "reference-docs";
    public const DocumentationTemplate Template = DocumentationTemplate.Default;
}

internal interface IConfiguration
{
    string Input { get; }
    bool ForceCreate { get; }
    MemberInheritanceMode InheritMembers { get; }
    AccessModifier MinVisibility { get; }
    IEnumerable<string> ExcludeNamespaces { get; }
    string OutputDir { get; }
    IEnumerable<string> ExcludeProjects { get; }
    string? StaticPagesDir { get; }
    DocumentationTemplate Template { get; }
    bool Verbose { get; }
    string? DocVersion { get; }
    bool SaveConfig { get; }

}

internal class YamlFileConfiguration : IConfiguration
{
    public string Input { get; set; } = string.Empty;
    public bool ForceCreate { get; set; }
    public MemberInheritanceMode InheritMembers { get; set; } = DefaultConfigValues.InheritMembers;
    public AccessModifier MinVisibility { get; set; } = DefaultConfigValues.MinVisibility;
    public IEnumerable<string> ExcludeNamespaces { get; set; } = [];
    public string OutputDir { get; set; } = DefaultConfigValues.OutputDir;
    public IEnumerable<string> ExcludeProjects { get; set; } = [];
    public string? StaticPagesDir { get; set; }
    public DocumentationTemplate Template { get; set; } = DefaultConfigValues.Template;
    public bool Verbose { get; set; }
    public string? DocVersion { get; set; }

    [YamlIgnore]
    public bool SaveConfig => false;

    public YamlFileConfiguration()
    {
    }

    internal static YamlFileConfiguration From(IConfiguration configuration)
    {
        return new YamlFileConfiguration
        {
            Input = configuration.Input,
            ForceCreate = configuration.ForceCreate,
            InheritMembers = configuration.InheritMembers,
            MinVisibility = configuration.MinVisibility,
            ExcludeNamespaces = configuration.ExcludeNamespaces,
            OutputDir = configuration.OutputDir,
            ExcludeProjects = configuration.ExcludeProjects,
            StaticPagesDir = configuration.StaticPagesDir,
            Template = configuration.Template,
            Verbose = configuration.Verbose,
            DocVersion = configuration.DocVersion,
        };
    }
}

class YamlConfiguration
{
    internal const string fileName = "refdocgen.config.yaml";

    internal static IConfiguration FromFile(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            throw new YamlConfigurationNotFoundException(filePath); // YAML configuration not found
        }

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

        var text = File.ReadAllText(filePath);
        YamlFileConfiguration? config;

        try
        {
            config = deserializer.Deserialize<YamlFileConfiguration>(text);
        }
        catch (YamlException e)
        {
            throw new InvalidYamlConfigurationException(filePath, e);
        }

        if (config.Input == string.Empty)
        {
            throw new InvalidYamlConfigurationException(filePath, new ArgumentException("The required property 'input' is missing."));
        }

        return config;
    }

    internal static void Save(IConfiguration configuration, string filePath)
    {
        var yamlConfig = YamlFileConfiguration.From(configuration);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections)
            .Build();

        string yaml = serializer.Serialize(yamlConfig);

        File.WriteAllText(filePath, yaml);
    }
}
