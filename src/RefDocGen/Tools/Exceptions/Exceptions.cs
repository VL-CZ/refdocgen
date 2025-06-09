using System.Xml.Linq;

namespace RefDocGen.Tools;

internal class RefDocGenFatalException : Exception
{
    public RefDocGenFatalException(string message) : base(message)
    {
    }

    public RefDocGenFatalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

internal class AssemblyNotLoadedException : RefDocGenFatalException
{
    public AssemblyNotLoadedException(string assemblyPath) : base(assemblyPath)
    {
    }
}

internal class XmlDocNotLoaded : RefDocGenFatalException
{
    public XmlDocNotLoaded(string xmlDocPath) : base(xmlDocPath)
    {

    }
}

internal class DocVersionNameException : RefDocGenFatalException
{
    public DocVersionNameException(string version) : base(version)
    {

    }
}

internal class DuplicateDocVersion : RefDocGenFatalException
{
    public DuplicateDocVersion(string version) : base(version)
    {

    }
}

internal class InvalidTemplateConfiguration : RefDocGenFatalException
{
    public InvalidTemplateConfiguration(string version) : base(version)
    {

    }
}

internal class InvalidInnerXmlTagConfiguration : RefDocGenFatalException
{
    public InvalidInnerXmlTagConfiguration(XElement version) : base(version.ToString())
    {

    }
}

internal class StaticFilesFolderNotFound : RefDocGenFatalException
{
    public StaticFilesFolderNotFound(string version) : base(version)
    {

    }
}

internal class InvalidLanguageName : RefDocGenFatalException
{
    public InvalidLanguageName(string version) : base(version)
    {

    }
}

internal class DuplicateLanguageName : RefDocGenFatalException
{
    public DuplicateLanguageName(string version) : base(version)
    {

    }
}

internal class InvalidTemplate : RefDocGenFatalException
{
    public InvalidTemplate(string version) : base(version)
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
