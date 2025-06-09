using System.Xml.Linq;

namespace RefDocGen.Tools.Exceptions;

internal class RefDocGenFatalException : Exception
{
    public RefDocGenFatalException() : base()
    {
    }

    public RefDocGenFatalException(string message) : base(message)
    {
    }

    public RefDocGenFatalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

internal class AssemblyNotFoundException : RefDocGenFatalException
{
    private const string template = "The assembly at path {0} was not found.";

    public AssemblyNotFoundException(string assemblyPath) : base(string.Format(template, assemblyPath))
    {
    }
}

internal class XmlDocNotFoundException : RefDocGenFatalException
{
    public XmlDocNotFoundException(string xmlDocPath) : base(xmlDocPath)
    {

    }
}

internal class InvalidDocVersionNameException : RefDocGenFatalException
{
    public InvalidDocVersionNameException(string version) : base(version)
    {

    }
}

internal class DuplicateDocVersionNameException : RefDocGenFatalException
{
    public DuplicateDocVersionNameException(string version) : base(version)
    {

    }
}

internal class InvalidTemplateConfigurationException : RefDocGenFatalException
{
    public InvalidTemplateConfigurationException(string version) : base(version)
    {

    }
}

internal class InvalidInnerXmlTagConfigurationException : RefDocGenFatalException
{
    public InvalidInnerXmlTagConfigurationException(XElement version) : base(version.ToString())
    {

    }
}

internal class StaticFilesFolderNotFoundException : RefDocGenFatalException
{
    public StaticFilesFolderNotFoundException(string version) : base(version)
    {

    }
}

internal class InvalidLanguageIdException : RefDocGenFatalException
{
    public InvalidLanguageIdException(string version) : base(version)
    {

    }
}

internal class DuplicateLanguageIdException : RefDocGenFatalException
{
    public DuplicateLanguageIdException(string version) : base(version)
    {

    }
}

internal class InvalidTemplateException : RefDocGenFatalException
{
    public InvalidTemplateException(string version) : base(version)
    {

    }
}

internal class UrlValidator
{
    private static readonly char[] allowedCharacters = ['-', '_', '~', '.'];

    internal static bool IsValid(string url)
    {
        return url.All(c => char.IsAsciiLetter(c) || char.IsAsciiDigit(c) || allowedCharacters.Contains(c));
    }
}
