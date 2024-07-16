using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

internal static class MethodParameterDataExtensions
{
    internal static bool HasRefKeyword(this MethodParameterData parameterData)
    {
        return parameterData.IsPassedByReference && !parameterData.IsInput && !parameterData.IsOutput;
    }
}
