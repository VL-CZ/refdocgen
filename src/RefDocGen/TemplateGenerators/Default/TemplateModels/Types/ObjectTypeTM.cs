using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

/// <summary>
/// Represents the template model for a type.
/// </summary>
/// <param name="Id">Unique identifier of the type.</param>
/// <param name="Name">The name of the type.</param>
/// <param name="Namespace">The namespace containing the type.</param>
/// <param name="DocComment">Documentation comment for the type.</param>
/// <param name="TypeKindName">Name of the type kind.</param>
/// <param name="Modifiers">Collection of modifiers for the type (e.g., public, abstract).</param>
/// <param name="Constructors">Template models of the type constructors.</param>
/// <param name="Fields">Template models of the fields declared in the type.</param>
/// <param name="Properties">Template models of the properties declared in the type.</param>
/// <param name="Methods">Template models of the methods declared in the type.</param>
/// <param name="BaseTypeName">Name of the base type, null if the type doesn't have any base type.</param>
/// <param name="ImplementedInterfaces">Collection of interfaces implemented by the type.</param>
public record ObjectTypeTM(
    string Id,
    string Name,
    string Namespace,
    string DocComment,
    string TypeKindName,
    IEnumerable<string> Modifiers,
    ConstructorTM[] Constructors,
    FieldTM[] Fields,
    PropertyTM[] Properties,
    MethodTM[] Methods,
    string? BaseTypeName,
    IEnumerable<string> ImplementedInterfaces) : ITemplateModelWithId;
