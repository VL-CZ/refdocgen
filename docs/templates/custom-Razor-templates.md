# Creating custom Razor templates

You can use custom Razor templates with the `RazorTemplateProcessor` class to define your own documentation UI. The overall structure and output folder layout will remain the same as with the default templates.

Follow these steps:

1. Clone the `RefDocGen` project and navigate to the `src/RefDocGen/TemplateProcessors` folder.
2. Copy the `Todo` subfolder and rename it to match your template design (e.g., `RazorMinimal`).
3. Implement the eight Razor templates in the `Templates` subfolder. Each template includes a description of its intended usage and parameters.
4. You may use any UI framework, but:
	- All template parameters must remain unchanged.
	- Member HTML elements on type pages should have their `id` set to the member ID.
	- Place custom CSS and JavaScript in the `Templates/Static` folder. Use the provided `styles.css` and `script.js` files if possible.
	- To support versioning, each page must include an element with `id="version-list"`. The available version identifiers will be inserted automatically and can be processed with JavaScript.
	- Data such as field modifiers are represented as `LanguageSpecificData<T>`, which works as a dictionary indexed by language IDs. For C#, use the `CSharpData` property.

## Configuration for XML tag transformation
In addition to the templates, you must configure how the inner XML tags are transformed to HTML. There are two options:

#### 1. Reuse the default configuration (recommended)

The default configuration is represented by the `DocCommentHtmlConfiguration` class, which:
    - Replaces XML tags (including `<seealso>` tags) with HTML elements (see Table 4.2 in the docs).
    - These HTML elements are assigned CSS classes starting with `refdocgen`, as shown below:

```
XML Tag                                         HTML Representation
-----------------------------------------       ---------------------------------------------------
<para>                                      	<div class="refdocgen-paragraph">
<list type="bullet">                        	<ul class="refdocgen-bullet-list">
<list type="number">                        	<ol class="refdocgen-number-list">
<list type="table">                         	<table class="refdocgen-table">
<listheader>                                	<thead class="refdocgen-tableheader">
<c>                                         	<code class="refdocgen-inline-code">
<example>                                   	<div class="refdocgen-example">
<paramref>                                  	<code class="refdocgen-paramref">
<typeparamref>                              	<code class="refdocgentypeparamref">
<see href="...">                            	<a href="..." class="refdocgen-seehref">
<see langword="...">                        	<code class="refdocgen-seelangword">
<seealso href="...">                        	<a href="..." class="refdocgenseealso-href">
<code>                                      	<pre><code class="refdocgen-codeblock"></code></pre>
<term> within a bullet/number list          	<span class="refdocgen-list-term">
<description> within a bullet/number list   	<span class="refdocgen-listdescription">
<item> within a bullet/number list          	<li class="refdocgen-list-item">
<term> within a table                       	<td class="refdocgen-table-term">
<description> within a table                	<td class="refdocgen-table-element">
<item> within a table                       	<tr class="refdocgen-table-item">
<see cref="..."> (link resolved)            	<a href="..." class="refdocgen-seecref">
<seealso cref="..."> (link resolved)        	<a href="..." class="refdocgenseealso-cref">
<see cref="..."> (link not resolved)        	<code class="refdocgen-see-cref-notfound">
<seealso cref="..."> (link not resolved)    	<code class="refdocgen-seealso-crefnot-found">
```

All of these CSS classes can be styled as needed. For example:

```css
.refdocgen-paragraph {
    /* custom styles */
}
```

You can also inherit existing CSS classes using a CSS preprocessor, similar to how the default UI does it (see `TemplateProcessors/Default/Templates/Scss/styles.scss`).

#### 2. Create a custom configuration

For more control over the transformation, you can create a custom configuration.

To customize how XML tags are transformed to HTML, implement the `IDocCommentHtmlConfiguration` interface in your template folder (e.g., `RazorMinimal`).
Each property of this interface defines the target HTML for a specific XML tag. For example, to replace the `<c>` XML tag (inline code) with a `<span class="my-class">` element, set the corresponding property as follows:

```csharp
public XElement InlineCodeElement => new XElement("span",
    new XAttribute("class", "my-class")
);
```

## Registering Custom Templates

After you create the templates and configuration, you must register the templates:
1. Open `Program.cs` and find the comments marked `#ADD_TEMPLATE`.
2. Add a new value to the `DocumentationTemplate` enum (e.g., `Minimal`).
3. Instantiate your template processor and add it to the dictionary under the new enum value. Use `RazorTemplateProcessor.With` with your template types and configuration, passing the same constructor arguments as for `DefaultTemplateProcessor`.

    ```csharp
    RazorTemplateProcessor<
        CustomObjectTypePage,
        CustomDelegateTypePage,
        CustomEnumTypePage,
        CustomNamespacePage,
        CustomAssemblyPage,
        CustomApiHomePage,
        CustomStaticPage,
        CustomSearchPage
    >.With(
        new DocCommentHtmlConfiguration(),// the default configuration
        // use a custom one, if provided
        htmlRenderer,
        availableLanguages,
        config.StaticPagesDirectory,
        config.Version)
    ```

4. You can now select your custom templates when generating documentation (by setting the `--template` option to `Minimal`).
