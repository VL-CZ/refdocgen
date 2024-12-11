namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for a property.
/// </summary>
/// <param name="Name">Name of the property.</param>
/// <param name="Type">Type of the property.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the property.</param>
/// <param name="ValueDocComment">'value' documentation comment for the property.</param>
/// <param name="Modifiers">Collection of property modifiers (e.g. public, static, etc.)</param>
/// <param name="HasGetter">Checks if the property has getter.</param>
/// <param name="HasSetter">Checks if the property has setter.</param>
/// <param name="GetterModifiers">Collection of the getter modifiers (possibly empty).</param>
/// <param name="SetterModifiers">Collection of the setter modifiers (possibly empty).</param>
public record PropertyTM(string Name, string Type, string SummaryDocComment, string ValueDocComment, IEnumerable<string> Modifiers, bool HasGetter, bool HasSetter,
    IEnumerable<string> GetterModifiers, IEnumerable<string> SetterModifiers);
