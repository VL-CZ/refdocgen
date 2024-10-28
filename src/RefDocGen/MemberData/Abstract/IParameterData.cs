using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Abstract;

/// <summary>
/// Represents data of a method/constructor parameter.
/// </summary>
public interface IParameterData
{
    /// <summary>
    /// <see cref="System.Reflection.ParameterInfo"/> object representing the parameter.
    /// </summary>
    ParameterInfo ParameterInfo { get; }

    /// <summary>
    /// Name of the parameter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Type of the parameter.
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Checks if the parameter is a <c>params</c> collection.
    /// <para>For further information, see <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters#params-modifier"/></para>
    /// </summary>
    bool IsParamsCollection { get; }

    /// <summary>
    /// Checks if the parameter is optional.
    /// </summary>
    bool IsOptional { get; }

    /// <summary>
    /// Checks if the parameter is an input parameter.
    /// </summary>
    bool IsInput { get; }

    /// <summary>
    /// Checks if the parameter is an output parameter.
    /// </summary>
    bool IsOutput { get; }

    /// <summary>
    /// Checks if the parameter is passed by reference.
    /// </summary>
    bool IsPassedByReference { get; }

    /// <summary>
    /// XML doc comment for the parameter.
    /// </summary>
    XElement DocComment { get; }
}
