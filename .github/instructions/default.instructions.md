# RefDocGen - Coding Instructions

## Project Overview

RefDocGen is a **reference documentation generator for .NET**, installed as a .NET global tool (`refdocgen`). It extracts XML documentation comments from .NET assemblies and generates static HTML reference documentation using server-side Razor rendering.

## Tech Stack

- **.NET 8** (target framework: `net8.0`)
- **C#** with nullable reference types enabled and implicit usings
- **Razor SDK** (`Microsoft.NET.Sdk.Razor`) — used for server-side HTML rendering via `HtmlRenderer`, not as a web app
- **Key libraries**: AngleSharp (HTML parsing), CommandLineParser (CLI), Markdig (Markdown), YamlDotNet (YAML config), Serilog (logging), Microsoft.Build (MSBuild integration)
- **Testing**: xUnit + Shouldly (assertions) + NSubstitute (mocking) + AngleSharp (integration test HTML assertions)
- **E2E tests**: Playwright (Node.js, separate from .NET test projects)

## Solution Structure

| Project | Path | Purpose |
|---------|------|---------|
| `RefDocGen` | `src/RefDocGen/` | Main application (console tool) |
| `RefDocGen.UnitTests` | `tests/RefDocGen.UnitTests/` | Unit tests |
| `RefDocGen.IntegrationTests` | `tests/RefDocGen.IntegrationTests/` | Integration tests (generates HTML, asserts on DOM) |
| `RefDocGen.ExampleLibrary` | `tests/RefDocGen.ExampleLibrary/` | C# example library used as test input |
| `RefDocGen.ExampleFSharpLibrary` | `tests/RefDocGen.ExampleFSharpLibrary/` | F# example library used as test input |
| `RefDocGen.ExampleVbLibrary` | `tests/RefDocGen.ExampleVbLibrary/` | VB.NET example library used as test input |
| `RefDocGen.EndToEndTests` | `tests/RefDocGen.EndToEndTests/` | Playwright E2E tests (Node.js) |

## Key Namespaces

| Namespace | Purpose |
|-----------|---------|
| `RefDocGen` | Entry point (`Program`, `DocGenerator`) |
| `RefDocGen.AssemblyAnalysis` | Assembly loading, type/member extraction |
| `RefDocGen.CodeElements` | Core data model: types, members, enums, access modifiers |
| `RefDocGen.CodeElements.Members.Abstract` | Member interfaces (`IMemberData`, `IMethodData`, etc.) |
| `RefDocGen.CodeElements.Members.Concrete` | Member implementations |
| `RefDocGen.CodeElements.Types.Abstract` | Type interfaces (`IObjectTypeData`, `ITypeDeclaration`, etc.) |
| `RefDocGen.CodeElements.Types.Concrete` | Type implementations |
| `RefDocGen.CodeElements.TypeRegistry` | Central type storage (`ITypeRegistry`) |
| `RefDocGen.Config` | CLI and YAML configuration |
| `RefDocGen.DocExtraction` | XML doc comment extraction and `inheritdoc` resolution |
| `RefDocGen.TemplateProcessors` | Template processing abstraction and implementations |
| `RefDocGen.TemplateProcessors.Shared` | Shared template infrastructure, model creators, template models |
| `RefDocGen.Tools` | Extension methods, exceptions, XML utilities |

## Architecture Patterns

- **Abstract/Concrete split**: Interfaces live in `Abstract/` folders, implementations in `Concrete/` folders (applies to both `Members/` and `Types/`)
- **Public interfaces, internal implementations**: Public API is exposed via interfaces; concrete classes are `internal`
- **`InternalsVisibleTo`**: Unit tests and integration tests can access internal members
- **Record types** for simple data carriers (e.g., `AssemblyData`, `NamespaceData`, `AssemblyDataConfiguration`)
- **Template architecture**: `ITemplateProcessor` → `RazorTemplateProcessor<T1..T8>` (generic base) → `DefaultTemplateProcessor` (concrete)
- **Template model creators** follow `*TMCreator` naming (e.g., `ObjectTypeTMCreator`, `EnumTMCreator`)
- **Extension point markers** in code: `#ADD_TEMPLATE`, `#ADD_TEMPLATE_PROCESSOR`, `#ADD_LANGUAGE`
- **Immutable collections**: Heavy use of `IReadOnlyDictionary`, `IReadOnlyList`, `IEnumerable`

## Coding Conventions

### Naming

- **Types** (class, struct, interface, enum, delegate): `PascalCase`
- **Interfaces**: Prefix with `I` (e.g., `IMemberData`, `ITemplateProcessor`)
- **Properties, methods, events**: `PascalCase`
- **Local variables, parameters**: `camelCase`
- **Private/protected fields**: `camelCase` without underscore prefix (e.g., `private readonly ILogger logger;`)
- **Private constants**: `camelCase` (e.g., `private const string delegateMethodName = "Invoke";`)

### Style

- **File-scoped namespaces** — always use `namespace Foo;` not `namespace Foo { }`
- **No top-level statements** — `Program.cs` uses explicit `Main` method pattern
- **No primary constructors** — use traditional constructor syntax
- **`var` usage**: Do not use `var` for built-in types; use `var` when the type is apparent or elsewhere
- **Indentation**: 4 spaces (2 spaces for YAML/YML files)
- **Line endings**: CRLF
- **Final newline**: Yes
- **Trim trailing whitespace**: Yes
- **`internal` access modifier**: Used extensively for concrete implementations
- **Discard pattern**: Use `_ =` for unused return values (e.g., `_ = services.AddLogging(…);`)
- **Collection expressions** (C# 12): Use `[.. collection]` syntax where applicable
- **`required` modifier**: Used on properties in configuration classes

### XML Documentation

- Write comprehensive XML doc comments on **all public and internal members**
- Use standard tags: `<summary>`, `<param>`, `<returns>`, `<remarks>`, `<see cref="…"/>`
- Use `/// <inheritdoc/>` to inherit documentation from interfaces/base classes in implementations
- The project itself generates documentation (`GenerateDocumentationFile=true`)

### Error Handling

- Custom exception hierarchy rooted at `RefDocGenFatalException` (internal)
- Use `string.Format(CultureInfo.InvariantCulture, …)` for exception message formatting
- `ExceptionDispatchInfo` used to capture and rethrow with original stack trace

## Testing Conventions

### Unit Tests (`RefDocGen.UnitTests`)

- **Framework**: xUnit with `[Fact]` and `[Theory]`/`[InlineData]`
- **Assertions**: Shouldly (`.ShouldBe()`, `Should.Throw<>()`)
- **Mocking**: NSubstitute (`Substitute.For<T>()`, `.Returns()`)
- **Test class naming**: `{ClassName}Tests` (e.g., `StringExtensionsTests`)
- **Test method naming**: `MethodName_ExpectedBehavior_Condition` (e.g., `TryGetIndex_ReturnsExpectedIndex_IfTheValueIsFound`)
- **Shared mocks**: `MockHelper` class with static factory methods

### Integration Tests (`RefDocGen.IntegrationTests`)

- Use **xUnit Collection Fixtures** (`[Collection(...)]`, `ICollectionFixture<T>`) to share expensive doc generation setup
- Generate actual HTML output, then parse with **AngleSharp** to assert on DOM structure
- Helper tools in `Tools/` folder (e.g., `DocumentationTools.GetApiPage()`, `TypePageTools.GetAttributes()`)

## Build & CI

### Building

```bash
dotnet build --no-restore -warnaserror
```

### Running Tests

```bash
dotnet test ./tests/RefDocGen.UnitTests
dotnet test ./tests/RefDocGen.IntegrationTests
```

### Format Check

```bash
dotnet format style --verify-no-changes
```

### CI Pipeline (`.github/workflows/dotnet.yml`)

Triggered on push/PR to `master`:
1. `dotnet restore`
2. `dotnet build --no-restore -warnaserror`
3. Unit tests → Integration tests → Format check
4. Generate reference docs and upload as artifact
5. E2E tests (Playwright)
6. Deploy to GitHub Pages (master only)

## Suppressed Warnings

- `IDE0058` — expression value unused (in tests)
- `CA1707` — underscores in identifiers (allowed in test method names)
- `IDE0072` — switch expression exhaustiveness
- `CA1822` — mark members as static
- `CA1852` — seal internal types
- `IDE0046` — convert to conditional expression
- `CA1848` — logging message template performance
