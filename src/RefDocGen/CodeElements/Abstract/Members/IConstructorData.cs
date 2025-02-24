using RefDocGen.CodeElements.Abstract.Types.Exception;
using System.Reflection;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a constructor.
/// </summary>
public interface IConstructorData : IParameterizedMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.ConstructorInfo"/> object representing the constructor.
    /// </summary>
    ConstructorInfo ConstructorInfo { get; }

    /// <summary>
    /// Represents a collection of exceptions documented for the member.
    /// </summary>
    /// <remarks>
    /// This collection includes only the exceptions explicitly documented using the <c>exception</c> XML tag. 
    /// It does not include all possible exceptions that might occur during execution.
    /// </remarks>
    IEnumerable<IExceptionDocumentation> DocumentedExceptions { get; }
}
