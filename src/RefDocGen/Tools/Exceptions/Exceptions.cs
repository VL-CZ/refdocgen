using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using System.Globalization;
using System.Xml.Linq;

namespace RefDocGen.Tools.Exceptions;

/// <summary>
/// Common base class for all of the fatal exceptions, caused by an invalid program configuration.
/// </summary>
internal class RefDocGenFatalException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefDocGenFatalException"/> class
    /// with a formatted message and an optional inner exception.
    /// </summary>
    /// <param name="messageTemplate">
    /// A composite format string that defines the exception message.
    /// </param>
    /// <param name="innerException">
    /// The optional inner exception.
    /// </param>
    /// <param name="messageTemplateArgs">
    /// An array of arguments to be passed into the <paramref name="messageTemplate"/>.
    /// </param>
    protected RefDocGenFatalException(string messageTemplate, Exception? innerException, params string[] messageTemplateArgs)
        : base(string.Format(CultureInfo.InvariantCulture, messageTemplate, messageTemplateArgs), innerException)
    {
    }

    /// <inheritdoc cref="RefDocGenFatalException(string, Exception?, string[])"/>
    protected RefDocGenFatalException(string messageTemplate, params string[] messageTemplateArgs)
        : this(messageTemplate, null, messageTemplateArgs)
    {
    }
}

/// <summary>
/// Thrown when the provided assembly is not found.
/// </summary>
internal class AssemblyNotFoundException : RefDocGenFatalException
{
    private const string messageTemplate = "The assembly at path '{0}' was not found.";

    public AssemblyNotFoundException(string assemblyPath) : base(messageTemplate, assemblyPath)
    {
    }
}

/// <summary>
/// Thrown when the provided assembly is not found.
/// </summary>
internal class OutputDirectoryNotEmptyException : RefDocGenFatalException
{
    private const string messageTemplate = "The output directory at path '{0}' is not empty. Use the '--force-create' option to overwrite it.";

    public OutputDirectoryNotEmptyException(string outputDirPath) : base(messageTemplate, outputDirPath)
    {
    }
}

/// <summary>
/// Thrown when the XML documentation file is not found.
/// </summary>
internal class XmlDocFileNotFoundException : RefDocGenFatalException
{
    private const string messageTemplate = "The XML documentation file at path '{0}' was not found.";

    public XmlDocFileNotFoundException(string xmlDocPath)
        : base(messageTemplate, xmlDocPath)
    {
    }
}

/// <summary>
/// Thrown when the documentation version name is invalid.
/// </summary>
internal class InvalidDocVersionNameException : RefDocGenFatalException
{
    private const string messageTemplate = "The documentation version name '{0}' is invalid. " +
        "A valid documentation version name may contain only ASCII letters, digits, and the characters '-','.' '_', and '~'.";

    public InvalidDocVersionNameException(string version)
        : base(messageTemplate, version)
    {
    }
}

/// <summary>
/// Thrown when a duplicate documentation version is encountered.
/// </summary>
internal class DuplicateDocVersionNameException : RefDocGenFatalException
{
    private const string messageTemplate = "A documentation version with the name '{0}' already exists.";

    public DuplicateDocVersionNameException(string version)
        : base(messageTemplate, version)
    {
    }
}

/// <summary>
/// Thrown when the template location is invalid.
/// </summary>
internal class InvalidTemplateLocationException : RefDocGenFatalException
{
    private const string messageTemplate = "The location of the templates: \n{0} \n is invalid. " +
        "All templates must be in stored the same directory contained within 'RefDocGen/TemplateProcessors' folder.";

    public InvalidTemplateLocationException(Type[] templateTypes)
        : base(messageTemplate, GetTemplateTypeNamesString(templateTypes))
    {
    }

    /// <summary>
    /// Gets string containing the names of the template types.
    /// </summary>
    private static string GetTemplateTypeNamesString(Type[] templateTypes)
    {
        var templateNames = templateTypes.Select(t => t.FullName);
        return string.Join(", ", templateNames);
    }
}

/// <summary>
/// Thrown when the provided <see cref="IDocCommentHtmlConfiguration"/> is invalid.
/// </summary>
internal class InvalidDocCommentHtmlConfigurationException : RefDocGenFatalException
{
    private const string messageTemplate = "The following HTML provided for an inner XML documentation tag is invalid. \n{0} \n " +
        "There must be exactly one empty descendant (or self) element.";

    public InvalidDocCommentHtmlConfigurationException(XElement element)
        : base(messageTemplate, element.ToString())
    {
    }
}

/// <summary>
/// Thrown when the static pages directory is not found.
/// </summary>
internal class StaticPagesDirectoryNotFoundException : RefDocGenFatalException
{
    private const string messageTemplate = "The static pages directory at path '{0}' was not found.";

    public StaticPagesDirectoryNotFoundException(string staticPagesDirectory)
        : base(messageTemplate, staticPagesDirectory)
    {
    }
}

/// <summary>
/// Thrown when the language ID is invalid.
/// </summary>
internal class InvalidLanguageIdException : RefDocGenFatalException
{
    private const string messageTemplate = "The language identifier '{0}' is invalid. A valid language identifier may contain only ASCII letters, digits, and the characters '-','.' '_', and '~'";

    public InvalidLanguageIdException(string languageId)
        : base(messageTemplate, languageId)
    {
    }
}

/// <summary>
/// Thrown when a duplicate language ID is encountered.
/// </summary>
internal class DuplicateLanguageIdException : RefDocGenFatalException
{
    private const string messageTemplate = "The language identifier '{0}' is duplicate.";

    public DuplicateLanguageIdException(string languageId)
        : base(messageTemplate, languageId)
    {
    }
}

/// <summary>
/// Thrown when a language-specific component is not found.
/// </summary>
internal class LanguageSpecificComponentNotFoundException : RefDocGenFatalException
{
    private const string messageTemplate = "The language-specific component '{0}' wasn't found.";

    public LanguageSpecificComponentNotFoundException(string componentName)
        : base(messageTemplate, componentName)
    {
    }
}

/// <summary>
/// Thrown when rendering of a template fails (typically due to errors in the template or its data).
/// </summary>
internal class TemplateRenderingException : RefDocGenFatalException
{
    private const string messageTemplate = "An error occurred while rendering the '{0}' template; see the inner exception for further details.";

    public TemplateRenderingException(Type templateType, Exception innerException)
        : base(messageTemplate, innerException, templateType.FullName ?? templateType.Name)
    {
    }
}
