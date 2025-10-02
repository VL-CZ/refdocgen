## Documentation versioning

You can optionally generate versioned documentation, allowing users to switch between multiple versions.
To do this, it is necessary to use the `--doc-version` option.

The version can be switched using the dropdown in the bottom menu.

#### Examples

Generate two versions of the documentation, using these commands (the output directory needs to be the same):

```bash
refdocgen MyLibrary.dll --doc-version v1.0

# after version 1.1 is published
refdocgen MyLibrary.dll --doc-version v1.1
```

The documentation versions do not necessarily have to match the library versions.
For instance, we may create two documentation versions, one showing the public API, and the other including even private members, as illustrated below:

```
refdocgen MyLibrary.dll --doc-version v1.0-public
refdocgen MyLibrary.dll --doc-version v1.0-private --min-visibility Private
```
