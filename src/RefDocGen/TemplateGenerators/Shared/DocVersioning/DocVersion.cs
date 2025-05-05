namespace RefDocGen.TemplateGenerators.Shared.DocVersioning;

/// <summary>
/// Represents a certain version of the documentation.
/// </summary>
/// <param name="Version">Identifier of the documentation version.</param>
/// <param name="PageUrls">URLs of all pages (relative to the version directory) contained in the given doc version.</param>
internal record DocVersion(string Version, HashSet<string> PageUrls);
