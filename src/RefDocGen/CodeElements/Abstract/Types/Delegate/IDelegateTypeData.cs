using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types.Delegate;

/// <summary>
/// Represents data of a delegate.
/// </summary>
public interface IDelegateTypeData : ITypeDeclaration
{
    /// <summary>
    /// The method used for delegate invocation (i.e. <c>Invoke</c>).
    /// </summary>
    IMethodData InvokeMethod { get; }

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

    /// <summary>
    /// Represents a collection of exceptions documented for the delegate.
    /// </summary>
    /// <remarks>
    /// This collection includes only the exceptions explicitly documented using the <c>exception</c> XML tag. 
    /// It does not include all possible exceptions that might occur during execution.
    /// </remarks>
    IReadOnlyList<IExceptionDocumentation> Exceptions { get; }
}
