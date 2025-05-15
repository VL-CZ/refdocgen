using RefDocGen.CodeElements;

namespace RefDocGen.AssemblyAnalysis;

/// <param name="MinVisibility">Minimal visibility of the types and members to include.</param>
/// <param name="MemberInheritanceMode">Specifies which inherited members should be included in types.</param>
/// <param name="AssembliesToExclude">Names of assemblies to be excluded from the reference documentation.</param>
/// <param name="NamespacesToExclude">Namespaces to be excluded from the reference documentation.</param>
public record AssemblyDataConfiguration(
    AccessModifier MinVisibility,
    MemberInheritanceMode MemberInheritanceMode,
    IEnumerable<string> AssembliesToExclude,
    IEnumerable<string> NamespacesToExclude);
