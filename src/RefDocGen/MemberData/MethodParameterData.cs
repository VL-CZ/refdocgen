using System.Reflection;

namespace RefDocGen.MemberData;

public record MethodParameterData(ParameterInfo ParameterInfo)
{
    public string Name => ParameterInfo.Name;

    public string Type => ParameterInfo.ParameterType.Name;

    public bool IsParamsArray => ParameterInfo.GetCustomAttribute(typeof(ParamArrayAttribute)) != null;

    public bool IsOptional => ParameterInfo.IsOptional;

    public bool IsInput => ParameterInfo.IsIn;

    public bool IsOutput => ParameterInfo.IsOut;

    public bool IsPassedByReference => ParameterInfo.ParameterType.IsByRef;

    public bool HasRefKeyword => IsPassedByReference && !IsInput && !IsOutput;
}
