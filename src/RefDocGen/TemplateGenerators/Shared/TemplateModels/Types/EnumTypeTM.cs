using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for an enum, including its members.
/// </summary>
/// <param name="Id">Unique identifier of the enum.</param>
/// <param name="Name">Name of the enum.</param>
/// <param name="Namespace">The namespace containing the enum.</param>
/// <param name="Assembly">The assembly containing the enum.</param>
/// <param name="Modifiers">Collection of modifiers for the type (e.g., public, abstract).</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the enum. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the enum. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Members">Template models of the enum members.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the enum.</param>
/// <param name="Attributes">Array of attributes applied to the enum.</param>
/// <param name="DeclaringType">
/// The type that contains the declaration of this type.
/// <para>
/// <c>null</c> if the type is not nested.
/// </para>
/// </param>
public record EnumTypeTM(
    string Id,
    string Name,
    string Namespace,
    string Assembly,
    LanguageSpecificData<string[]> Modifiers,
    EnumMemberTM[] Members,
    AttributeTM[] Attributes,
    TypeLinkTM? DeclaringType,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments) : ITemplateModelWithId, ITypeDeclarationNameTM
{
    /// <inheritdoc/>
    TypeParameterTM[] ITypeDeclarationNameTM.TypeParameters => [];
}
