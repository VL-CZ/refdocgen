using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.DocVersioning;
using RefDocGen.TemplateGenerators.Shared.StaticPages;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Menu;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.Tools;

namespace RefDocGen.TemplateGenerators.Shared;

/// <summary>
/// Class responsible for generating the Razor templates and populating them with the type data.
/// </summary>
/// <typeparam name="TDelegateTemplate">Type of the Razor component representing a delegate type.</typeparam>
/// <typeparam name="TEnumTemplate">Type of the Razor component representing a enum type.</typeparam>
/// <typeparam name="TNamespaceTemplate">Type of the Razor component representing a namespace.</typeparam>
/// <typeparam name="TAssemblyTemplate">Type of the Razor component representing an assembly.</typeparam>
/// <typeparam name="TObjectTypeTemplate">Type of the Razor component representing an object type.</typeparam>
/// <typeparam name="TStaticPageTemplate">Type of the Razor component representing a static page.</typeparam>
internal class RazorTemplateGenerator<
        TObjectTypeTemplate,
        TDelegateTemplate,
        TEnumTemplate,
        TNamespaceTemplate,
        TAssemblyTemplate,
        TApiTemplate,
        TStaticPageTemplate
    > : ITemplateGenerator

    where TDelegateTemplate : IComponent
    where TEnumTemplate : IComponent
    where TNamespaceTemplate : IComponent
    where TAssemblyTemplate : IComponent
    where TObjectTypeTemplate : IComponent
    where TStaticPageTemplate : IComponent
    where TApiTemplate : IComponent
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
    /// <see cref="RazorTemplateGenerator{TDelegateTemplate, TEnumTemplate, TNamespaceDetailTemplate, TNamespaceListTemplate, TObjectTypeTemplate, TStaticPageTemplate}"/> class.
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

        GenerateObjectTypeTemplates(typeRegistry.ObjectTypes);
        GenerateEnumTemplates(typeRegistry.Enums);
        GenerateDelegateTemplates(typeRegistry.Delegates);
        GenerateNamespaceTemplates(typeRegistry.GetNamespaces());
        GenerateAssemblyTemplates(typeRegistry.GetAssemblies());

        CopyStaticTemplateFilesDirectory();

        versionManager?.UpdateOlderVersions(pagesGenerated);
        versionManager?.SaveCurrentVersionData(pagesGenerated);
    }

    /// <summary>
    /// Generate the templates representing the individual object types.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateObjectTypeTemplates(IEnumerable<IObjectTypeData> types)
    {
        var creator = new ObjectTypeTMCreator(docCommentTransformer);
        var typeTemplateModels = types.Select(creator.GetFrom);
        GenerateTemplates<TObjectTypeTemplate, ObjectTypeTM>(typeTemplateModels);
    }

    /// <summary>
    /// Generate the templates representing the individual enum types.
    /// </summary>
    /// <param name="enums">The enum data to be used in the templates.</param>
    private void GenerateEnumTemplates(IEnumerable<IEnumTypeData> enums)
    {
        var creator = new EnumTMCreator(docCommentTransformer);
        var enumTMs = enums.Select(creator.GetFrom);
        GenerateTemplates<TEnumTemplate, EnumTypeTM>(enumTMs);
    }

    /// <summary>
    /// Generate the templates representing the individual delegate types.
    /// </summary>
    /// <param name="delegates">The delegate data to be used in the templates.</param>
    private void GenerateDelegateTemplates(IEnumerable<IDelegateTypeData> delegates)
    {
        var creator = new DelegateTMCreator(docCommentTransformer);
        var delegateTMs = delegates.Select(creator.GetFrom);
        GenerateTemplates<TDelegateTemplate, DelegateTypeTM>(delegateTMs);
    }

    /// <summary>
    /// Generate the templates representing the namespaces.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateNamespaceTemplates(IEnumerable<NamespaceData> namespaces)
    {
        var namespaceTMs = namespaces.Select(NamespaceTMCreator.GetFrom);
        GenerateTemplates<TNamespaceTemplate, NamespaceTM>(namespaceTMs);
    }

    /// <summary>
    /// Generate the templates representing the assemblies.
    /// </summary>
    /// <param name="types">The type data to be used in the templates.</param>
    private void GenerateAssemblyTemplates(IEnumerable<AssemblyData> assemblies)
    {
        var assemblyTMs = assemblies.Select(NamespaceTMCreator.GetFrom);
        GenerateTemplates<TAssemblyTemplate, AssemblyTM>(assemblyTMs);

        // namespace list template
        GenerateTemplate<TApiTemplate, IEnumerable<AssemblyTM>>(assemblyTMs, indexPageId);

        if (!isUserDefinedIndexPage) // no user-specified 'index' page -> add the default one redirecting to API page.
        {
            string outputIndexPage = Path.Join(outputDirectory, indexPageId + ".html");
            File.Copy(defaultIndexPage, outputIndexPage, true);
        }
    }

    /// <summary>
    /// Generate the given templates using the provided template models.
    /// </summary>
    /// <param name="templateModels">Template models used for the templates generation.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to generate.</typeparam>
    private void GenerateTemplates<TTemplate, TTemplateModel>(IEnumerable<TTemplateModel> templateModels)
        where TTemplate : IComponent
        where TTemplateModel : ITemplateModelWithId
    {
        foreach (var tm in templateModels)
        {
            GenerateTemplate<TTemplate, TTemplateModel>(tm, tm.Id);
        }
    }

    /// <summary>
    /// Generate the given template using the provided template model.
    /// </summary>
    /// <param name="templateModel">Template model used for the template generation.</param>
    /// <param name="outputFile">Name of the output file containing the generated template populated with the <paramref name="templateModel"/> data.</param>
    /// <typeparam name="TTemplateModel">Type of the template model to be used in the template.</typeparam>
    /// <typeparam name="TTemplate">Type of the Razor component representing the template to generate.</typeparam>
    private void GenerateTemplate<TTemplate, TTemplateModel>(TTemplateModel templateModel, string outputFile)
        where TTemplate : IComponent
    {
        _ = Directory.CreateDirectory(OutputApiDirectory);

        string outputFileName = Path.Join(OutputApiDirectory, $"{outputFile}.html");

        string pagePath = Path.GetRelativePath(outputDirectory, outputFileName);
        string[]? versions = versionManager?.GetVersions(pagePath);

        string html = htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var paramDictionary = new Dictionary<string, object?>()
            {
                ["Model"] = templateModel,
                ["TopMenuData"] = topMenuData,
                ["Versions"] = versions
            };

            var parameters = ParameterView.FromDictionary(paramDictionary);
            var output = await htmlRenderer.RenderComponentAsync<TTemplate>(parameters);

            return output.ToHtmlString();
        }).Result;


        File.WriteAllText(outputFileName, html);
        _ = pagesGenerated.Add(pagePath);
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
            typeof(TDelegateTemplate),
            typeof(TEnumTemplate),
            typeof(TNamespaceTemplate),
            typeof(TAssemblyTemplate),
            typeof(TObjectTypeTemplate)
        ];

        string? templatesNs = typeof(TObjectTypeTemplate).Namespace ?? "";

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

        foreach (var page in pages) // wrap each page in the static page template and copy it into the output directory
        {
            string outputPath = Path.Combine(outputDirectory, page.PageDirectory);
            var dir = Directory.CreateDirectory(outputPath);

            string outputFile = Path.Combine(outputPath, $"{page.PageName}.html");
            string pagePath = Path.GetRelativePath(outputDirectory, outputFile);

            string[]? versions = versionManager?.GetVersions(pagePath);

            string html = htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var paramDictionary = new Dictionary<string, object?>()
                {
                    ["Contents"] = page.HtmlBody,
                    ["TopMenuData"] = topMenuData,
                    ["CustomStyles"] = cssFile.Exists ? StaticPageProcessor.cssFilePath : null,
                    ["NestingLevel"] = page.FolderDepth,
                    ["Versions"] = versions
                };

                var parameters = ParameterView.FromDictionary(paramDictionary);
                var output = await htmlRenderer.RenderComponentAsync<TStaticPageTemplate>(parameters);

                return output.ToHtmlString();
            }).Result;

            File.WriteAllText(outputFile, html);

            _ = pagesGenerated.Add(pagePath);
        }
    }
}
