using System.Reflection;
using System.Xml.Linq;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.CodeElements.Members.Abstract;

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
    /// Position of the parameter in the parameter list (zero based).
    /// </summary>
    int Position { get; }

    /// <summary>
    /// Name of the parameter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Type of the parameter.
    /// </summary>
    ITypeNameData Type { get; }

    /// <summary>
    /// Checks if the parameter is a <c>params</c> collection.
    /// <para>For further information, see <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/method-parameters#params-modifier"/></para>
    /// </summary>
    bool IsParamsCollection { get; }

    /// <summary>
    /// Checks if the parameter is the 1st parameter of an extension method.
    /// </summary>
    bool IsExtensionParameter { get; }

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
    bool IsByRef { get; }

    /// <summary>
    /// Default value of the parameter.
    /// <para>
    /// Returns <see cref="DBNull.Value"/> if the parameter has no default value.
    /// (note that this is done, because <see langword="null"/> can be declared as a default value of the parameter.
    /// </para>
    /// </summary>
    object? DefaultValue { get; }

    /// <summary>
    /// XML doc comment for the parameter.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Collection of attributes applied to the parameter.
    /// </summary>
    IReadOnlyList<IAttributeData> Attributes { get; }
}
