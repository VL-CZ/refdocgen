namespace RefDocGen.TemplateGenerators.Default.TemplateModels;

/// <summary>
/// Represents the template model for a class.
/// </summary>
/// <param name="Name">The name of the class.</param>
/// <param name="DocComment">Documentation comment for the class.</param>
/// <param name="Modifiers">A collection of modifiers for the class (e.g., public, abstract).</param>
/// <param name="Constructors">Template models of the class constructors.</param>
/// <param name="Fields">Template models of the fields declared in the class.</param>
/// <param name="Properties">Template models of the properties declared in the class.</param>
/// <param name="Methods">Template models of the methods declared in the class.</param>
public record ClassTemplateModel(string Name, string DocComment, IEnumerable<string> Modifiers, ConstructorTemplateModel[] Constructors,
    FieldTemplateModel[] Fields, PropertyTemplateModel[] Properties, MethodTemplateModel[] Methods) : INamedTemplateModel;
