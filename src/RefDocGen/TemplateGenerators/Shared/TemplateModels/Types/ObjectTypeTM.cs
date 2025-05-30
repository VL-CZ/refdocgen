using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="Name">The name of the type.</param>
/// <param name="Namespace">The namespace containing the type.</param>
/// <param name="Assembly">The assembly containing the type.</param>
/// <param name="Modifiers">Collection of modifiers for the type (e.g., public, abstract).</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the type. <c>null</c> if the doc comment is not provided.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the type. <c>null</c> if the doc comment is not provided.</param>
/// <param name="Constructors">Template models of the type constructors.</param>
/// <param name="Fields">Template models of the fields contained in the type.</param>
/// <param name="Properties">Template models of the properties contained in the type.</param>
/// <param name="Methods">Template models of the methods contained in the type.</param>
/// <param name="Operators">Template models of the operators contained in the type.</param>
/// <param name="Indexers">Template models of the indexers contained in the type.</param>
/// <param name="Events">Template models of the events contained in the type.</param>
/// <param name="TypeParameters">Template models of the generic type parameters contained in the type.</param>
/// <param name="BaseType">Base type of this type, null if the type doesn't have any base type.</param>
/// <param name="ImplementedInterfaces">Collection of interfaces implemented by the type.</param>
/// <param name="SeeAlsoDocComments">Collection of <c>seealso</c> documentation comments for the type.</param>
/// <param name="Attributes">Array of attributes applied to the type.</param>
/// <param name="NestedTypes">Template models of the nested types contained in the type.</param>
/// <param name="DeclaringType">
/// The type that contains the declaration of this type.
/// <para>
/// <c>null</c> if the type is not nested.
/// </para>
/// </param>
public record ObjectTypeTM(
    string Id,
    string Name,
    string Namespace,
    string Assembly,
    LanguageSpecificData<string[]> Modifiers,
    ConstructorTM[] Constructors,
    FieldTM[] Fields,
    PropertyTM[] Properties,
    MethodTM[] Methods,
    OperatorTM[] Operators,
    IndexerTM[] Indexers,
    EventTM[] Events,
    TypeNameTM[] NestedTypes,
    TypeParameterTM[] TypeParameters,
    GenericCodeLinkTM? BaseType,
    GenericCodeLinkTM[] ImplementedInterfaces,
    AttributeTM[] Attributes,
    CodeLinkTM? DeclaringType,
    string? SummaryDocComment,
    string? RemarksDocComment,
    string[] SeeAlsoDocComments) : ITemplateModelWithId, ITypeDeclarationNameTM;
