using RefDocGen.CodeElements.Abstract.Members;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing a user-documented exception (using the 'exception' tag) that can be thrown by a member.
/// </summary>
internal class ExceptionData : IExceptionData
{
    /// <summary>
    /// Initialize a new instance of <see cref="ExceptionData"/> class.
    /// </summary>
    /// <param name="name">Fully qualified name of the exception.</param>
    /// <param name="docComment">Doc comment provided to the exception.</param>
    public ExceptionData(string name, XElement docComment)
    {
        Name = name;
        DocComment = docComment;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public XElement DocComment { get; }
}
