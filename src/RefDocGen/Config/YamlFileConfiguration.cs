using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using RefDocGen.Tools.Exceptions;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RefDocGen.Config;

/// <summary>
/// Represents the YAML configuration of <c>RefDocGen</c>.
/// </summary>
internal class YamlFileConfiguration : IProgramConfiguration
{
    /// <summary>
    /// The naming convention of YAML (de)serialization.
    /// </summary>
    private static readonly INamingConvention namingConvention = HyphenatedNamingConvention.Instance;

    /// <summary>
    /// The name of the YAML configuration file.
    /// </summary>
    internal const string FileName = "refdocgen.yaml";

    /// <inheritdoc/>
    public string Input { get; set; } = string.Empty;

    /// <inheritdoc/>
    public bool ForceCreate { get; set; }

    /// <inheritdoc/>
    public MemberInheritanceMode InheritMembers { get; set; } = DefaultConfigValues.InheritMembers;

    /// <inheritdoc/>
    public AccessModifier MinVisibility { get; set; } = DefaultConfigValues.MinVisibility;

    /// <inheritdoc/>
    public IEnumerable<string> ExcludeNamespaces { get; set; } = [];

    /// <inheritdoc/>
    public string OutputDir { get; set; } = DefaultConfigValues.OutputDir;

    /// <inheritdoc/>
    public IEnumerable<string> ExcludeProjects { get; set; } = [];

    /// <inheritdoc/>
    public string? StaticPagesDir { get; set; }

    /// <inheritdoc/>
    public DocumentationTemplate Template { get; set; } = DefaultConfigValues.Template;

    /// <inheritdoc/>
    public bool Verbose { get; set; }

    /// <inheritdoc/>
    public string? DocVersion { get; set; }

    /// <inheritdoc/>
    [YamlIgnore]
    public bool SaveConfig => false;

    public YamlFileConfiguration()
    {
    }

    /// <summary>
    /// Creates a <see cref="YamlFileConfiguration"/> based on the provided <see cref="IProgramConfiguration"/>.
    /// </summary>
    /// <param name="configuration">The program configuration.</param>
    /// <returns>The program configuration as a <see cref="YamlFileConfiguration"/> instance.</returns>
    private static YamlFileConfiguration From(IProgramConfiguration configuration)
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

    /// <summary>
    /// Gets the program configuration from a YAML file.
    /// </summary>
    /// <param name="filePath">Path to the YAML configuration file.</param>
    /// <returns>The program configuration extracted from the YAML file.</returns>
    /// <exception cref="YamlConfigurationNotFoundException">Thrown when the YAML configuration file is not found.</exception>
    /// <exception cref="InvalidYamlConfigurationException">Thrown when the YAML configuration is invalid.</exception>
    internal static YamlFileConfiguration FromFile(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            throw new YamlConfigurationNotFoundException(filePath); // YAML configuration file not found -> throw
        }

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(namingConvention)
            .Build();

        string yamlText = File.ReadAllText(filePath);
        YamlFileConfiguration? config;

        try
        {
            config = deserializer.Deserialize<YamlFileConfiguration>(yamlText);
        }
        catch (YamlException e)
        {
            throw new InvalidYamlConfigurationException(filePath, e);
        }

        if (config.Input == string.Empty) // no 'input' property in YAML -> throw
        {
            throw new InvalidYamlConfigurationException(filePath, new ArgumentException("The required property 'input' is missing."));
        }

        return config;
    }

    /// <summary>
    /// Saves the configuration into a YAML file.
    /// </summary>
    /// <param name="configuration">The provided program configuration.</param>
    internal static void SaveToFile(IProgramConfiguration configuration)
    {
        var yamlConfig = From(configuration);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(namingConvention)
            .WithIndentedSequences()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitEmptyCollections)
            .Build();

        string yaml = serializer.Serialize(yamlConfig);

        File.WriteAllText(FileName, yaml);
    }
}
