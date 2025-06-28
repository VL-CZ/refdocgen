using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RefDocGen.Tools.Config;

internal interface IConfiguration
{
    string Input { get; set; }
    bool ForceCreate { get; set; }
    MemberInheritanceMode InheritMembers { get; set; }
    AccessModifier MinVisibility { get; set; }
    IEnumerable<string> ExcludeNamespaces { get; set; }
    string OutputDir { get; set; }
    IEnumerable<string> ExcludeProjects { get; set; }
    string? StaticPagesDir { get; set; }
    DocumentationTemplate Template { get; set; }
    bool Verbose { get; set; }
    string? DocVersion { get; set; }
}

class YamlFileConfiguration : IConfiguration
{
    public required string Input { get; set; }
    public bool ForceCreate { get; set; }
    public MemberInheritanceMode InheritMembers { get; set; }
    public AccessModifier MinVisibility { get; set; }
    public IEnumerable<string> ExcludeNamespaces { get; set; } = [];
    public string OutputDir { get; set; } = "reference-docs";
    public IEnumerable<string> ExcludeProjects { get; set; } = [];
    public string? StaticPagesDir { get; set; }
    public DocumentationTemplate Template { get; set; }
    public bool Verbose { get; set; }
    public string? DocVersion { get; set; }

    public YamlFileConfiguration()
    {
    }
}

class YamlConfiguration
{
    internal static IConfiguration FromFile(string filePath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var text = File.ReadAllText(filePath);
        return deserializer.Deserialize<YamlFileConfiguration>(text);
    }
}
