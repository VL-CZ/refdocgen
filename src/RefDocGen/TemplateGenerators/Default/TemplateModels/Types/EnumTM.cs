using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

/// <summary>
/// Represents the template model for an enum, including its members.
/// </summary>
/// <param name="Id">Unique identifier of the enum.</param>
/// <param name="Name">Name of the enum.</param>
/// <param name="Namespace">Namespace containing the enum.</param>
/// <param name="DocComment">Documentation comment for the enum.</param>
/// <param name="Modifiers">Collection of modifiers for the type (e.g., public, abstract).</param>
/// <param name="Members">Template models of the enum members.</param>
public record EnumTM(string Id, string Name, string Namespace, string DocComment, IEnumerable<string> Modifiers, IEnumerable<EnumMemberTM> Members) : ITemplateModelWithId;
