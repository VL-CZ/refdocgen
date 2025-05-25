namespace RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

/// <summary>
/// Template model representing a search result displayed at the <c>search</c> page.
/// </summary>
/// <param name="Name">Name of the search result.</param>
/// <param name="DocComment">Documentation provided to the search result.</param>
/// <param name="Id">ID of the search result. In case of a member, the ID containing type is returned.</param>
/// <param name="IdFragment">In case of a member, this variable represents the ID of the member.</param>
public record SearchResultTM(LocalizedData<string> Name, string DocComment, string Id, string? IdFragment = null) : ITemplateModelWithId
{
    /// <summary>
    /// URL of the search result, relative to the <c>search</c> page.
    /// </summary>
    public string Url =>
        IdFragment is not null
        ? $"api/{Id}.html#{IdFragment}"
        : $"api/{Id}.html";
};
