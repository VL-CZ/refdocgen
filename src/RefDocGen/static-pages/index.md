# RefDocGen - Reference Documentation Generator for .NET

RefDocGen is a reference documentation generator for .NET.

## Features

- easy to use
- supports all standard XML documentation tags
- resolves `inheritdoc` tags
- modern, responsive UI supporting both light and dark mode
- support for documentation versioning 
- option to add custom pages (*index*, *FAQ*, ...)
- built-in search functionality

## Demos
- [reference documentation of this project](./api/index.html) (also accessible via the *API* link in the top menu)
- [reference documentation of an example library](https://vl-cz.github.io/refdocgen-demo-example-library/)
- [reference documentation of `Newtonsoft.JSON`](https://vl-cz.github.io/refdocgen-demo-third-party/v-newtonsoft/)
- [reference documentation of `YamlDotNet`](https://vl-cz.github.io/refdocgen-demo-third-party/v-yamldotnet/)
- [reference documentation of `Serilog`](https://vl-cz.github.io/refdocgen-demo-third-party/v-serilog/)

## Installation

Prerequisites:
- .NET 8 (or higher)

Install as a .NET global tool from [NuGet](https://www.nuget.org/packages/RefDocGen):

```
dotnet tool install --global RefDocGen
```

This makes the `refdocgen` command available on the command line.

## Usage
### Prerequisites

Before generating documentation, it is necessary to build the project/solution in *Debug* configuration.
The following MSBuild properties must be set:

- `GenerateDocumentationFile=true` - creates the XML file with documentation
- `CopyLocalLockFileAssemblies=true` - ensures all dependencies are copied to the output folder

Ideally, specify these properties on the command line, as follows:
```
dotnet build -p:GenerateDocumentationFile=true -p:CopyLocalLockFileAssemblies=true
```

It is also possible to set them in the project file or in the *Directory.Build.props*.

**Important: RefDocGen doesn't support .NET Framework projects.**

### Running

Run the following command to generate reference documentation:

```
refdocgen INPUT [OPTIONS]
```

The only mandatory argument is the `INPUT` - an assembly/project/solution to document, or a YAML configuration file (further explained below).

#### Examples

```bash
refdocgen MyLibrary.dll
refdocgen MyLibrary.csproj
refdocgen MyLibrary.sln
```

### Limitations 

#### Default UI languages
While *RefDocGen* supports programs written in any .NET language, the default UI displays type and member signatures only in C# syntax.
However, the default UI is designed to be extensible, so adding support for other languages, such as F#, is possible in the future.

#### Supported modifiers
*RefDocGen* recognizes most of the C# modifiers and displays them.
However, some modifiers are not supported and therefore do not appear in the generated documentation.  
These are typically implementation details that do not affect the public API.

More specifically, the following modifiers are not supported: `extern`, `file`, `managed`, `new`, `partial`, `record`, `scoped`, `unmanaged`, `unsafe`, `volatile`.

Additionally, these type parameter constraints are not supported:
`notnull`, `unmanaged`, `default`, `allows ref struct`.

#### Non-nullable reference types
Currently, *RefDocGen* doesn't differentiate between nullable non-nullable reference types.
