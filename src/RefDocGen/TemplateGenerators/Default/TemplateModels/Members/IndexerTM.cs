namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

/// <summary>
/// Represents the template model for an indexer.
/// </summary>
/// <param name="Type">Type of the indexer.</param>
/// <param name="Parameters">Collection of index parameters.</param>
/// <param name="SummaryDocComment">'summary' documentation comment for the indexer.</param>
/// <param name="ValueDocComment">'value' documentation comment for the indexer.</param>
/// <param name="Modifiers">Collection of indexer modifiers (e.g. public, static, etc.)</param>
/// <param name="HasGetter">Checks if the indexer has getter.</param>
/// <param name="HasSetter">Checks if the indexer has setter.</param>
/// <param name="GetterModifiers">Collection of the getter modifiers (possibly empty).</param>
/// <param name="SetterModifiers">Collection of the setter modifiers (possibly empty).</param>
public record IndexerTM(IEnumerable<ParameterTM> Parameters, string Type, string
    SummaryDocComment, string ValueDocComment, IEnumerable<string> Modifiers, bool HasGetter, bool HasSetter,
    IEnumerable<string> GetterModifiers, IEnumerable<string> SetterModifiers);
