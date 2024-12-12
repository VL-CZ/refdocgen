using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents a user-documented exception (using the 'exception' tag) that can be thrown by a member.
/// </summary>
public interface IExceptionData
{
    /// <summary>
    /// Fully qualified name of the exception.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Doc comment provided to the exception.
    /// </summary>
    XElement DocComment { get; }
}
