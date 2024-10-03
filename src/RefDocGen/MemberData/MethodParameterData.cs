using RefDocGen.DocExtraction;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a method parameter
/// </summary>
/// <param name="ParameterInfo"><see cref="System.Reflection.ParameterInfo"/> object representing the parameter</param>
public record MethodParameterData(ParameterInfo ParameterInfo)
{
    /// <summary>
    /// Name of the parameter.
    /// </summary>
    public string Name => ParameterInfo.Name ?? string.Empty;

    /// <summary>
    /// Type of the parameter.
    /// </summary>
    public string Type => ParameterInfo.ParameterType.Name;

    /// <summary>
    /// Checks if the parameter is a <c>params</c> collection.
    /// <para>For further information, see <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters#params-modifier"/></para>
    /// </summary>
    public bool IsParamsCollection => ParameterInfo.GetCustomAttribute(typeof(ParamArrayAttribute)) != null;

    /// <summary>
    /// Checks if the parameter is optional.
    /// </summary>
    public bool IsOptional => ParameterInfo.IsOptional;

    /// <summary>
    /// Checks if the parameter is an input parameter
    /// </summary>
    public bool IsInput => ParameterInfo.IsIn;

    /// <summary>
    /// Checks if the parameter is an output parameter
    /// </summary>
    public bool IsOutput => ParameterInfo.IsOut;

    /// <summary>
    /// Checks if the parameter is passed by reference
    /// </summary>
    public bool IsPassedByReference => ParameterInfo.ParameterType.IsByRef;

    /// <summary>
    /// XML doc comment for the parameter
    /// </summary>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummaryNode;
}
