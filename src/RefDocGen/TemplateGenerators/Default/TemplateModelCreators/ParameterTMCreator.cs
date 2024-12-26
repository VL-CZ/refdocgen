using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual method parameters.
/// </summary>
internal class ParameterTMCreator
{
    /// <summary>
    /// Creates a <see cref="ParameterTM"/> instance based on the provided <see cref="IParameterData"/> object.
    /// </summary>
    /// <param name="parameterData">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTM"/> instance based on the provided <paramref name="parameterData"/>.</returns>
    internal static ParameterTM GetFrom(IParameterData parameterData)
    {
        List<Keyword> modifiers = [];

        if (parameterData.IsInput)
        {
            modifiers.Add(Keyword.In);
        }

        if (parameterData.IsOutput)
        {
            modifiers.Add(Keyword.Out);
        }

        if (RefKeyword.IsPresentIn(parameterData))
        {
            modifiers.Add(Keyword.Ref);
        }

        if (parameterData.IsParamsCollection)
        {
            modifiers.Add(Keyword.Params);
        }

        return new ParameterTM(parameterData.Name, CSharpTypeName.Of(parameterData.Type), parameterData.DocComment.Value,
            modifiers.GetStrings(), parameterData.DefaultValue);
    }
}
