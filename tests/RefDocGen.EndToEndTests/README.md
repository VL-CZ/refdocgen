# End to End tests
This directory contains end-to-end tests written in [Playwright](https://playwright.dev/) framework.

Run these two commands from the root folder to generate test data (i.e. versioned documentation of RefDocGen.ExampleLibrary):

```shell
dotnet run --project ./src/RefDocGen/ -- tests/RefDocGen.ExampleLibrary/RefDocGen.ExampleLibrary.csproj --static-pages-dir tests/RefDocGen.ExampleLibrary/static-pages --doc-version v1.0 -o tests/RefDocGen.EndToEndTests/e2e-tests-docs

dotnet run --project ./src/RefDocGen/ -- tests/RefDocGen.ExampleLibrary/RefDocGen.ExampleLibrary.csproj --static-pages-dir tests/RefDocGen.ExampleLibrary/static-pages --doc-version v1.1 -o tests/RefDocGen.EndToEndTests/e2e-tests-docs
```

Then, run the tests:

```shell
npx playwright test
```
