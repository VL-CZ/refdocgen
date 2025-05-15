using RefDocGen.CodeElements.Types.Abstract.Exception;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Types.Concrete.Exception;

/// <summary>
/// Class representing a user-documented exception (using the 'exception' tag) that can be thrown by a member.
/// </summary>
internal class ExceptionDocumentation : IExceptionDocumentation
{
    /// <summary>
    /// Initialize a new instance of <see cref="ExceptionDocumentation"/> class.
    /// </summary>
    /// <param name="id">Identifier of the exception.</param>
    /// <param name="docComment">Doc comment provided to the exception.</param>
    public ExceptionDocumentation(string id, XElement docComment)
    {
        Id = id;
        DocComment = docComment;
    }

    /// <inheritdoc/>
    public string Id { get; }

    /// <inheritdoc/>
    public XElement DocComment { get; }
}
