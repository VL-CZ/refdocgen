using RefDocGen.AssemblyAnalysis;
using RefDocGen.CodeElements;

namespace RefDocGen.Tools.Config;

/// <summary>
/// Contains default values for <c>RefDocGen</c> configuration.
/// </summary>
internal static class DefaultConfigValues
{
    /// <summary>
    /// The default <c>inherit-members</c> configuration value.
    /// </summary>
    internal const MemberInheritanceMode InheritMembers = MemberInheritanceMode.NonObject;

    /// <summary>
    /// The default <c>min-visibility</c> configuration value.
    /// </summary>
    internal const AccessModifier MinVisibility = AccessModifier.Public;

    /// <summary>
    /// The default <c>output-dir</c> configuration value.
    /// </summary>
    internal const string OutputDir = "reference-docs";

    /// <summary>
    /// The default <c>template</c> configuration value.
    /// </summary>
    internal const DocumentationTemplate Template = DocumentationTemplate.Default;
}
