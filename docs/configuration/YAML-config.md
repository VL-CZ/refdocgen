# YAML Configuration

You can use a YAML file to configure RefDocGen instead of specifying options on the command line. This approach makes it easy to reuse and share your configuration.

The YAML file can be generated automatically using the `--save-config` flag (preferred) or created manually.
It is recommended to name the file `refdocgen.yaml`.

YAML configuration closely mirrors the command-line options:

- Each YAML key matches its corresponding command-line option (without leading dashes). For example, the `output-dir` key in YAML corresponds to the `--output-dir` option.
- The only required key is `input`, just as on the command line.
- The `save-config` option is not supported in YAML, as it doesn't make sense here
- Default values are the same as those used for command-line options.

## Example

The following command generates a YAML configuration file as shown below:

```bash
refdocgen MyLibrary.sln 
    -o custom-folder
    --verbose 
    --force-create 
    --min-visibility Private 
    --exclude-projects MyLibrary.Tests 
    --exclude-namespaces MyLibrary.Internal MyLibrary.Experimental 
    --save-config # save the configuration into YAML
```

`refdocgen.yaml`

```yaml
input: MyLibrary.sln
output-dir: custom-folder
template: Default
verbose: true
force-create: true
min-visibility: Private
inherit-members: NonObject
exclude-projects:
  - MyLibrary.Tests
exclude-namespaces:
  - MyLibrary.Internal
  - MyLibrary.Experimental
```

The next time we want to use the same configuration, we just need to run `refdocgen refdocgen.yaml` and the configuration will be loaded from the YAML.
