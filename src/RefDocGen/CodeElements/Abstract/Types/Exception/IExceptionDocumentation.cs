using RefDocGen.CodeElements.Abstract.Types.TypeName;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types.Exception;

/// <summary>
/// Represents a user-documented exception (using the 'exception' tag) that can be thrown by a member.
/// </summary>
/// <remarks>
/// This interface doesn't represent exceptions declared as classes (these are represented by <see cref="ITypeDeclaration"/> interface).
/// </remarks>
public interface IExceptionDocumentation
{
    /// <summary>
    /// Identifier of the exception.
    /// The format is same as for <see cref="ITypeNameBaseData.Id"/>
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Doc comment provided to the exception.
    /// </summary>
    XElement DocComment { get; }
}
