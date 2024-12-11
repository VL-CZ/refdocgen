using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="Name">The name of the type.</param>
/// <param name="Namespace">The namespace containing the type.</param>
/// <param name="TypeKindName">Name of the type kind.</param>
/// <param name="Modifiers">Collection of modifiers for the type (e.g., public, abstract).</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the type.</param>
/// <param name="RemarksDocComment">'remarks' documentation comment for the type.</param>
/// <param name="Constructors">Template models of the type constructors.</param>
/// <param name="Fields">Template models of the fields declared in the type.</param>
/// <param name="Properties">Template models of the properties declared in the type.</param>
/// <param name="Methods">Template models of the methods declared in the type.</param>
/// <param name="Operators">Template models of the operators declared in the type.</param>
/// <param name="Indexers">Template models of the indexers declared in the type.</param>
/// <param name="TypeParameters">Template models of the generic type parameters declared in the type.</param>
/// <param name="BaseTypeName">Name of the base type, null if the type doesn't have any base type.</param>
/// <param name="ImplementedInterfaces">Collection of interfaces implemented by the type.</param>
public record ObjectTypeTM(
    string Id,
    string Name,
    string Namespace,
    string SummaryDocComment,
    string RemarksDocComment,
    string TypeKindName,
    IEnumerable<string> Modifiers,
    ConstructorTM[] Constructors,
    FieldTM[] Fields,
    PropertyTM[] Properties,
    MethodTM[] Methods,
    MethodTM[] Operators,
    IndexerTM[] Indexers,
    TypeParameterTM[] TypeParameters,
    string? BaseTypeName,
    IEnumerable<string> ImplementedInterfaces) : ITemplateModelWithId;
