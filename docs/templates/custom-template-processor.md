# Custom Template Processor

You can customize the output by implementing your own template processor. This lets you use any templating language and output format (e.g., Markdown, HTML).

To create a custom processor:
1. In the RefDocGen project, add a new folder (e.g., `Custom`) under `TemplateProcessors`.
2. Create a class (e.g., `CustomTemplateProcessor`) that implements the `ITemplateProcessor` interface. This interface requires a `ProcessTemplates` method, which generates documentation from the type registry.

Recommendations:
- Keep related code in the same folder or subfolders.
- Pass extra parameters via the `CustomTemplateProcessor` constructor if needed.
- Throw descriptive exceptions on errors.

## Reusing Existing Classes

If your documentation output is HTML, you can reuse the `StaticPageProcessor` class to collect static pages provided by the user. The `DocVersionManager` class may also be used in your custom template processor. The only requirement is that each HTML page must contain an element with `id="version-list"`, where the JSON array of available page versions will be automatically placed.

You can also reuse the `DocCommentTransformer` class nd its default configuration (`DocCommentHtmlConfiguration`). Additionally, you may reuse the template model creators (`TemplateProcessors/Shared/TemplateModelCreators`) if both of the following conditions are met:

1. The default template models (`TemplateProcessors/Shared/TemplateModels`) are suitable for your templates.
2. The documentation output structure matches the default templates: each type, namespace, and assembly is represented by a separate page in the `api` folder, with page URLs equal to the corresponding template model ID. On type pages, each member element should have the `id` attribute set to the corresponding member ID.

In this case, generating HTML is similar to the `RazorTemplateProcessor`, except you use a different templating language. Use the `RazorTemplateProcessor` class as a reference. Note that default template models store selected data as language-specific (use the `CSharpData` property to get the C# value).

## Registering the Template Processor

To register your custom template processor, follow these steps:

1. In `Program.cs`, find the comments marked with `#ADD_TEMPLATE_PROCESSOR`.
2. Add a new `DocumentationTemplate` enum value for your custom processor.
3. Add your processor to the set of available documentation templates by inserting an instance into the dictionary, using the new enum value as the key.
4. The processor will then be registered and appear in the list of available documentation templates displayed on the command line.
