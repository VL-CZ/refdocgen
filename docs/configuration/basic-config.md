# Basic configuration

All available options are listed below (the same output can be displayed by running `refdocgen --help`):

```
refdocgen INPUT [OPTIONS]

INPUT (pos. 0)                                  Required. The assembly, project, or solution to
                                                document, or a YAML configuration file.

-o DIR, --output-dir=DIR                        (Default: reference-docs) The output directory
                                                for the generated documentation.

-t TEMPLATE, --template=TEMPLATE                (Default: Default) The template to use for the
                                                documentation. Valid values: Default

-v, --verbose                                   (Default: false) Enable verbose output.

-f, --force-create                              (Default: false) Forces the creation of the
                                                documentation. If the output directory already
                                                exists, it will be deleted first.

-s, --save-config                               (Default: false) Save the current configuration
                                                to a YAML file.

--static-pages-dir=DIR                          Directory containing additional static pages to
                                                include in the documentation.

--doc-version=VERSION                           Generate a specific version of the documentation.

--min-visibility=VISIBILITY                     (Default: Public) Minimum visibility level of
                                                types and members to include in the
                                                documentation. Valid values: Private,
                                                FamilyAndAssembly, Assembly, Family,
                                                FamilyOrAssembly, Public

--inherit-members=MODE                          (Default: NonObject) Specify which inherited
                                                members to include in the documentation. Valid
                                                values: None, All, NonObject

--exclude-projects=PROJECT [PROJECT...]         Projects to exclude from the documentation.

--exclude-namespaces=NAMESPACE [NAMESPACE...]   Namespaces to exclude from the documentation.

--help                                          Display this help screen.

--version                                       Display version information.
```

#### Links
- [Custom static pages](./custom-static-pages.md)
- [Doc versioning](./doc-versioning.md)
- [YAML config](./YAML-config.md)
