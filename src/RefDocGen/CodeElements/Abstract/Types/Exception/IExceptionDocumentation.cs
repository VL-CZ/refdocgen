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
    /// Fully qualified name of the exception.
    /// </summary>
    string TypeName { get; }

    /// <summary>
    /// Doc comment provided to the exception.
    /// </summary>
    XElement DocComment { get; }
}
