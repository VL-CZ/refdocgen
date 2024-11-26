using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

/// <summary>
/// Represents declaration of a generic type parameter.
/// </summary>
public interface ITypeParameterDeclaration
{
    /// <summary>
    /// Index of the parameter in the declaring type's parameter collection.
    /// </summary>
    int Index { get; }

    /// <summary>
    /// Name of the type parameter (e.g. 'TKey').
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Doc comment for the type parameter.
    /// </summary>
    XElement DocComment { get; }
}
