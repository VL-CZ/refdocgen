using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.DocVersioning;
using RefDocGen.TemplateGenerators.Shared.StaticPages;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Assemblies;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared;

/// <summary>
/// Class responsible for generating the Razor templates and populating them with the type data.
/// </summary>
/// <typeparam name="TDelegatePageTemplate">Type of the Razor component representing a delegate page.</typeparam>
/// <typeparam name="TEnumPageTemplate">Type of the Razor component representing a enum page.</typeparam>
/// <typeparam name="TNamespacePageTemplate">Type of the Razor component representing a namespace page.</typeparam>
/// <typeparam name="TAssemblyPageTemplate">Type of the Razor component representing an assembly page.</typeparam>
/// <typeparam name="TApiPageTemplate">Type of the Razor component representing the main API page.</typeparam>
/// <typeparam name="TObjectTypePageTemplate">Type of the Razor component representing an object type page.</typeparam>
/// <typeparam name="TStaticPageTemplate">Type of the Razor component representing a static page template.</typeparam>
/// <typeparam name="TSearchPageTemplate">Type of the Razor component representing a search page.</typeparam>
internal class RazorTemplateGenerator<
        TObjectTypePageTemplate,
        TDelegatePageTemplate,
        TEnumPageTemplate,
        TNamespacePageTemplate,
        TAssemblyPageTemplate,
        TApiPageTemplate,
        TStaticPageTemplate,
        TSearchPageTemplate
    > : ITemplateGenerator

    where TDelegatePageTemplate : IComponent
    where TEnumPageTemplate : IComponent
    where TNamespacePageTemplate : IComponent
    where TAssemblyPageTemplate : IComponent
    where TObjectTypePageTemplate : IComponent
    where TStaticPageTemplate : IComponent
    where TApiPageTemplate : IComponent
    where TSearchPageTemplate : IComponent
{
    /// <summary>
    /// Namespace prefix of any template generator.
    /// </summary>
    private const string templateGeneratorsNsPrefix = "RefDocGen.TemplateGenerators.";

    /// <summary>
    /// Identifier of the <c>index</c> page.
    /// </summary>
    private const string indexPageId = "index";

    /// <summary>
    /// Path to the directory containing static files (typically css and js related to templates), relative to <see cref="templatesDirectory"/>.
    /// </summary>
    private const string staticTemplateFilesDirectory = "Static";

    /// <summary>
    /// Path to the default index page, redirecting to API page.
    /// </summary>
    private readonly string defaultIndexPage;

    /// <summary>
    /// The directory, where the generated output will be stored.
    /// </summary>
    private string outputDirectory;

    /// <summary>
    /// Rendered of the Razor components.
    /// </summary>
    private readonly HtmlRenderer htmlRenderer;

    /// <summary>
    /// Path to the directory containing the Razor templates, relative to <c>TemplateGenerators</c> folder.
    /// </summary>
    private readonly string templatesDirectory;

    /// <summary>
    /// An object representing the links and folders contained in the top menu.
    /// </summary>
    private TopMenuDataTM topMenuData = TopMenuTMCreator.DefaultMenuData;

    /// <summary>
    /// Path to the directory containing the static pages created by user. <c>null</c>, if the directory is not specified.
    /// </summary>
    private readonly string? staticPagesDirectory;

    /// <summary>
    /// Transformer of the XML doc comments into HTML.
    /// </summary>
    private readonly IDocCommentTransformer docCommentTransformer;

    /// <summary>
    /// Indicates whether there's an 'index' page defined by the user.
    /// </summary>
    private bool isUserDefinedIndexPage;

    /// <summary>
    /// Current version of the documentation being generated. <see langword="null"/> if the version is not specified.
    /// </summary>
    private readonly string? docVersion;

    /// <summary>
    /// Set of paths of all generated pages in the current doc version, relative to <see cref="outputDirectory"/>.
    /// </summary>
    private readonly HashSet<string> pagesGenerated = [];

    /// <summary>
    /// Manager of the documentation version.
    /// </summary>
    private DocVersionManager? versionManager;

    /// <summary>
    /// The directory, where the generated output API pages will be stored.
    /// </summary>
    private string OutputApiDirectory => Path.Join(outputDirectory, "api");

    /// <summary>
    /// Initialize a new instance of
    /// <see cref="RazorTemplateGenerator{TObjectTypeTemplate, TDelegateTemplate, TEnumTemplate, TNamespaceTemplate, TAssemblyTemplate, TApiTemplate, TStaticPageTemplate, TSearchPageTemplate}"/> class.
    /// </summary>
    /// <param name="htmlRenderer">Renderer of the Razor components.</param>
    /// <param name="docCommentTransformer">Transformer of the XML doc comments into HTML.</param>
    /// <param name="outputDirectory">The directory, where the generated output will be stored.</param>
    /// <param name="staticPagesDirectory">Path to the directory containing the static pages created by user. <c>null</c> indicates that the directory is not specified.</param>
    /// <param name="docVersion">Version of the documentation (e.g. 'v1.0'). Pass <c>null</c> if no specific version should be generated.</param>
    internal RazorTemplateGenerator(
        HtmlRenderer htmlRenderer,
        IDocCommentTransformer docCommentTransformer,
        string outputDirectory,
        string? staticPagesDirectory = null,
        string? docVersion = null)
    {
        this.htmlRenderer = htmlRenderer;
        this.docCommentTransformer = docCommentTransformer;
        this.outputDirectory = outputDirectory;
        this.staticPagesDirectory = staticPagesDirectory;
        this.docVersion = docVersion;

        defaultIndexPage = Path.Join("TemplateGenerators", "Shared", "StaticData", "defaultIndexPage.html");
        templatesDirectory = GetTemplatesDirectory();

    }

    /// <inheritdoc/>
    public void GenerateTemplates(ITypeRegistry typeRegistry)
    {
        docCommentTransformer.TypeRegistry = typeRegistry;

        if (docVersion is not null) // a specific version of documentation is being generated
        {
            string rootOutputDirectory = outputDirectory;

            outputDirectory = Path.Join(outputDirectory, docVersion); // set output directory
            _ = Directory.CreateDirectory(outputDirectory);

            versionManager = new(rootOutputDirectory, docVersion);
        }

        CopyStaticPages();

        GenerateObjectTypePages(typeRegistry.ObjectTypes);
        GenerateEnumPages(typeRegistry.Enums);
        GenerateDelegatePages(typeRegistry.Delegates);
        GenerateNamespacePages(typeRegistry.Namespaces);
        GenerateAssemblyPages(typeRegistry.Assemblies);
        GenerateApiHomepage(typeRegistry.Assemblies);
        GenerateSearchPage(typeRegistry);

        if (!isUserDefinedIndexPage) // no user-specified 'index' page -> add the default one redirecting to API page.
        {
            string outputIndexPage = Path.Join(outputDirectory, indexPageId + ".html");
            File.Copy(defaultIndexPage, outputIndexPage, true);
        }

        CopyStaticTemplateFilesDirectory();

        versionManager?.UpdateOlderVersions(pagesGenerated);
        versionManager?.SaveCurrentVersionData(pagesGenerated);
    }

    /// <summary>
    /// Generate the template representing the search page.
    /// </summary>
    /// <param name="typeRegistry">Type registry containing the declared types.</param>
    private void GenerateSearchPage(ITypeRegistry typeRegistry)
    {
        var model = SearchResultTMCreator.GetFrom(typeRegistry);

        var paramDictionary = new Dictionary<string, object?>()
        {
            ["Model"] = model
        };

        ProcessTemplate<TSearchPageTemplate>(paramDictionary, outputDirectory, "search");
    }

    /// <summary>
    /// Generate the pages representing the individual object types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateObjectTypePages(IEnumerable<IObjectTypeData> types)
    {
        var creator = new ObjectTypeTMCreator(docCommentTransformer);
        var typeTemplateModels = types.Select(creator.GetFrom);
        ProcessApiTemplates<TObjectTypePageTemplate, ObjectTypeTM>(typeTemplateModels);
    }

    /// <summary>
    /// Generate the pages representing the individual enum types.
    /// </summary>
    /// <param name="enums">The enum data to be used in the templates.</param>
    private void GenerateEnumPages(IEnumerable<IEnumTypeData> enums)
    {
        var creator = new EnumTMCreator(docCommentTransformer);
        var enumTMs = enums.Select(creator.GetFrom);
        ProcessApiTemplates<TEnumPageTemplate, EnumTypeTM>(enumTMs);
    }

    /// <summary>
    /// Generate the pages representing the individual delegate types.
    /// </summary>
    /// <param name="delegates">The delegate data to be used in the templates.</param>
    private void GenerateDelegatePages(IEnumerable<IDelegateTypeData> delegates)
    {
        var creator = new DelegateTMCreator(docCommentTransformer);
        var delegateTMs = delegates.Select(creator.GetFrom);
        ProcessApiTemplates<TDelegatePageTemplate, DelegateTypeTM>(delegateTMs);
    }

    /// <summary>
    /// Generate the pages representing the individual namespaces.
    /// </summary>
    /// <param name="namespaces">The namespace data to be used in the templates.</param>
    private void GenerateNamespacePages(IEnumerable<NamespaceData> namespaces)
    {
        var namespaceTMs = namespaces.Select(NamespaceTMCreator.GetFrom);
        ProcessApiTemplates<TNamespacePageTemplate, NamespaceTM>(namespaceTMs);
    }

    /// <summary>
    /// Generate the pages representing the individual assemblies.
    /// </summary>
    /// <param name="assemblies">The assembly data to be used in the templates.</param>
    private void GenerateAssemblyPages(IEnumerable<AssemblyData> assemblies)
    {
        var assemblyTMs = assemblies.Select(AssemblyTMCreator.GetFrom);
        ProcessApiTemplates<TAssemblyPageTemplate, AssemblyTM>(assemblyTMs);
    }

    /// <summary>
    /// Generate the API home page.
    /// </summary>
    /// <param name="assemblies">The assembly data to be used in the template.</param>
    private void GenerateApiHomepage(IEnumerable<AssemblyData> assemblies)
    {
        var assemblyTMs = assemblies.Select(AssemblyTMCreator.GetFrom);
        ProcessApiTemplate<TApiPageTemplate, IEnumerable<AssemblyTM>>(assemblyTMs, indexPageId);
    }

    /// <summary>
    /// Processes the given API templates using the provided template models and stores the resulting files in the output folder.
    /// </summary>
    /// <param name="templateModels">Template models used for the templates generation.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to generate.</typeparam>
    private void ProcessApiTemplates<TTemplate, TTemplateModel>(IEnumerable<TTemplateModel> templateModels)
        where TTemplate : IComponent
        where TTemplateModel : ITemplateModelWithId
    {
        foreach (var tm in templateModels)
        {
            ProcessApiTemplate<TTemplate, TTemplateModel>(tm, tm.Id);
        }
    }

    /// <summary>
    /// Processes the given API template using the provided template model and stores the resulting file in the output folder.
    /// </summary>
    /// <param name="templateModel">Template model used for the template processing.</param>
    /// <param name="outputFile">Name of the output file containing the processed template populated with the <paramref name="templateModel"/> data.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to process.</typeparam>
    private void ProcessApiTemplate<TTemplate, TTemplateModel>(TTemplateModel templateModel, string outputFile)
        where TTemplate : IComponent
    {
        var paramDictionary = new Dictionary<string, object?>()
        {
            ["Model"] = templateModel
        };

        ProcessTemplate<TTemplate>(paramDictionary, OutputApiDirectory, outputFile);
    }

    /// <summary>
    /// Copies the directory containing static template files (css, js, etc.) to the output directory.
    /// </summary>
    private void CopyStaticTemplateFilesDirectory()
    {
        var staticFilesDir = new DirectoryInfo(Path.Combine(templatesDirectory, staticTemplateFilesDirectory));
        string outputDirPath = Path.Combine(outputDirectory, staticTemplateFilesDirectory);

        if (staticFilesDir.Exists)
        {
            staticFilesDir.CopyTo(outputDirPath, true);
        }
        else
        {
            // TODO: log static files dir not found
        }
    }

    /// <summary>
    /// Returns base directory containing the Razor templates and static files.
    /// </summary>
    /// <returns>Path to the directory containing the Razor templates and static files.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown in 2 cases
    /// <list type="bullet">
    /// <item>The templates aren't stored under <c>RefDocGen/TemplateGenerators</c> directory.</item>
    /// <item>The templates (i.e. the generic type parameters) aren't stored in the same directory.</item>
    /// </list>
    /// </exception>
    private string GetTemplatesDirectory()
    {
        const string baseFolder = "TemplateGenerators";

        Type[] templateTypes = [
            typeof(TDelegatePageTemplate),
            typeof(TEnumPageTemplate),
            typeof(TNamespacePageTemplate),
            typeof(TAssemblyPageTemplate),
            typeof(TObjectTypePageTemplate)
        ];

        string? templatesNs = typeof(TObjectTypePageTemplate).Namespace ?? "";

        if (templateTypes.Any(t => t.Namespace != templatesNs))
        {
            throw new ArgumentException("Invalid configuration, all 5 templates must be in the same directory.");
        }

        if (!templatesNs.StartsWith(templateGeneratorsNsPrefix, StringComparison.Ordinal))
        {
            throw new ArgumentException("Invalid configuration, the templates must be stored in a folder contained in 'RefDocGen/TemplateGenerators' directory.");
        }

        string relativeTemplateNs = templatesNs[templateGeneratorsNsPrefix.Length..];

        string[] templatePathFragments = [baseFolder, .. relativeTemplateNs.Split('.')];
        return Path.Combine(templatePathFragments);
    }

    /// <summary>
    /// Copies and processes the static pages created by user and adds them into the documentation.
    /// </summary>
    private void CopyStaticPages()
    {
        if (staticPagesDirectory is null) // no static pages directory specified -> return
        {
            return;
        }

        var pageProcessor = new StaticPageProcessor(staticPagesDirectory);

        var pages = pageProcessor.GetStaticPages();
        var cssFile = pageProcessor.GetCssFile();

        topMenuData = new TopMenuTMCreator().CreateFrom(pages); // get menu items based on the static pages

        pageProcessor.CopyNonPageFiles(outputDirectory); // copy non-page files

        if (pages.Any(p => p.IsIndexPage))
        {
            isUserDefinedIndexPage = true; // mark user-defined 'index' page
        }

        foreach (var page in pages) // wrap each page in the static page template, process it, and copy it into the output directory
        {
            string outputPath = Path.Combine(outputDirectory, page.PageDirectory);

            var paramDictionary = new Dictionary<string, object?>()
            {
                ["Contents"] = page.HtmlBody,
                ["CustomStyles"] = cssFile.Exists ? StaticPageProcessor.cssFilePath : null,
                ["NestingLevel"] = page.FolderDepth,
            };

            ProcessTemplate<TStaticPageTemplate>(paramDictionary, outputPath, page.PageName);
        }
    }

    /// <summary>
    /// Processes any template, populates it with given data and stores the resulting file in the selected directory.
    /// </summary>
    /// <typeparam name="TTemplate">Type of the template to process.</typeparam>
    /// <param name="customTemplateParameters">Custom paramters to be passed to the template. Note that the <c>TopMenuData</c> and <c>Versions</c> paramters are passed by default.</param>
    /// <param name="templateOutputDirectory">Absolute path to the directory where the processed template should be stored.</param>
    /// <param name="templateFileName">Name of the file (without extension) containing the templated processed with <paramref name="customTemplateParameters"/> data.</param>
    private void ProcessTemplate<TTemplate>(Dictionary<string, object?> customTemplateParameters, string templateOutputDirectory, string templateFileName)
        where TTemplate : IComponent
    {
        _ = Directory.CreateDirectory(templateOutputDirectory);

        string outputFileName = Path.Join(templateOutputDirectory, $"{templateFileName}.html");

        string pagePath = Path.GetRelativePath(outputDirectory, outputFileName);
        string[]? versions = versionManager?.GetVersions(pagePath);

        string html = htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var sharedTemplateParameters = new Dictionary<string, object?>()
            {
                ["TopMenuData"] = topMenuData,
                ["Versions"] = versions
            };

            var templateParameters = sharedTemplateParameters.Merge(customTemplateParameters);

            var parameters = ParameterView.FromDictionary(templateParameters);
            var output = await htmlRenderer.RenderComponentAsync<TTemplate>(parameters);

            return output.ToHtmlString();
        }).Result;


        File.WriteAllText(outputFileName, html);
        _ = pagesGenerated.Add(pagePath);
    }
}
