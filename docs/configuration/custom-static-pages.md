## Adding custom static pages

You can include static pages (like *index* or *FAQ*) in the generated documentation, following the steps below:

- create a directory (e.g., `static-pages/`) with `.html` or `.md` files
- each file represents a page and should contain its the body content
- run the generator with:

```
--static-pages-dir static-pages/
```
- thus, the pages are included in the documentation, and links to them appear in the top menu

**Important: The static pages are not designed to offer the functionality of a full-fledged SSG. If you want more control over the pages, is advised to use an SSG, such as Jekyll, for user documentation.**

Additional notes:
- it is possible to use relative links between pages
- you can include images, JS, or any other resources in the static pages directory, and then reference them from the pages
- to add custom CSS styles, put them into `/css/styles.css` and they will be included automatically (however, use custom styles only for minor tweaks, rather than completely changing the overall appearance of the page)
- it is possible to put the pages (and other files) into subdirectories of the `static-pages/` directory (however, pages nested three or more levels deep will not appear in the top menu)

#### Example

Directory structure:
```
static-pages/
    index.html
    FAQ.md
```

`index.html`
```html
<h1>
    MyLibrary reference documentation
</h1>
<div>
    This page contains the reference documentation of MyLibrary.
</div>
```

`FAQ.md`
```markdown
# FAQ

## Q1: How to install the library?
## A1: ...
```
