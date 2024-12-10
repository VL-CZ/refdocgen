using RefDocGen.CodeElements.Abstract.Members;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types.Delegate;

/// <summary>
/// Represents data of a delegate.
/// </summary>
public interface IDelegateTypeData : ITypeDeclaration
{
    /// <summary>
    /// Return type of the delegate.
    /// </summary>
    ITypeNameData ReturnType { get; }

    /// <summary>
    /// Documentation comment for the delegate return value.
    /// </summary>
    XElement ReturnValueDocComment { get; }

    /// <summary>
    /// Readonly list of the delegate parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }
}
