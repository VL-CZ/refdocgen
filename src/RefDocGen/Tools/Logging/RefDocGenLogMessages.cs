using Microsoft.Extensions.Logging;

namespace RefDocGen.Tools.Logging;

/// <summary>
/// Source-generated logging methods used across the application.
/// </summary>
internal static partial class RefDocGenLogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Assembly {Name} loaded")]
    internal static partial void LogAssemblyLoaded(ILogger logger, string name);

    [LoggerMessage(Level = LogLevel.Information, Message = "Assembly {Name} excluded")]
    internal static partial void LogAssemblyExcluded(ILogger logger, string name);

    [LoggerMessage(Level = LogLevel.Information, Message = "XML documentation file {Name} loaded")]
    internal static partial void LogXmlDocFileLoaded(ILogger logger, string name);

    [LoggerMessage(Level = LogLevel.Information, Message = "Configuration saved into {File} file")]
    internal static partial void LogConfigurationSaved(ILogger logger, string file);

    [LoggerMessage(Level = LogLevel.Information, Message = "Generating documentation in {Folder} folder")]
    internal static partial void LogGeneratingDocumentation(ILogger logger, string folder);

    [LoggerMessage(Level = LogLevel.Information, Message = "A directory containing static template data copied to {Directory}")]
    internal static partial void LogStaticTemplateDataCopied(ILogger logger, string directory);

    [LoggerMessage(Level = LogLevel.Information, Message = "No directory containing static template data found at path {Directory}")]
    internal static partial void LogNoStaticTemplateDataFound(ILogger logger, string directory);

    [LoggerMessage(Level = LogLevel.Information, Message = "Static page {Directory}/{Page} found")]
    internal static partial void LogStaticPageFound(ILogger logger, string directory, string page);

    [LoggerMessage(Level = LogLevel.Information, Message = "Page {Name} created")]
    internal static partial void LogPageCreated(ILogger logger, string name);

    [LoggerMessage(Level = LogLevel.Information, Message = "Static file {FilePath} copied to {OutputPath}")]
    internal static partial void LogStaticFileCopied(ILogger logger, string filePath, string outputPath);
}
