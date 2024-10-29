using RefDocGen.MemberData.Abstract;

namespace RefDocGen.TemplateGenerators.Default.Tools;

/// <summary>
/// Static class containing additional methods related to the 'ref' keyword.
/// </summary>
internal static class RefKeyword
{
    /// <summary>
    /// Checks whether the 'ref' keyword is present in the provided parameter signature.
    /// </summary>
    /// <param name="parameterData">Parameter that we check for 'ref' keyword.</param>
    /// <returns>Boolean representing if the 'ref' keyword is present in the parameter signature.</returns>
    internal static bool IsPresentIn(IParameterData parameterData)
    {
        return parameterData.IsPassedByReference && !parameterData.IsInput && !parameterData.IsOutput;
    }
}
