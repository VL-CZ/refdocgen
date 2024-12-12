using RefDocGen.CodeElements.Abstract.Types.Exception;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types.Exception;

/// <summary>
/// Class representing a user-documented exception (using the 'exception' tag) that can be thrown by a member.
/// </summary>
internal class ExceptionDocumentation : IExceptionDocumentation
{
    /// <summary>
    /// Initialize a new instance of <see cref="ExceptionDocumentation"/> class.
    /// </summary>
    /// <param name="name">Fully qualified name of the exception.</param>
    /// <param name="docComment">Doc comment provided to the exception.</param>
    public ExceptionDocumentation(string name, XElement docComment)
    {
        TypeName = name;
        DocComment = docComment;
    }

    /// <inheritdoc/>
    public string TypeName { get; }

    /// <inheritdoc/>
    public XElement DocComment { get; }
}
