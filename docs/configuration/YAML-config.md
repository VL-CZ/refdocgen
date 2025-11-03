
## YAML configuration
Instead of using command line arguments, it is possible to use a YAML file for configuration. \
Then:
- we don't need to repeat the options every time
- the configuration can be easily shared

The YAML file can be generated automatically using the `--save-config` flag (preferred) or created manually.
It is recommended to name the file `refdocgen.yaml`.

The structure of YAML and command line configuration is very similar:

- all the keys in YAML have the same name as the matching command-line option (without the starting dashes), e.g. the `output-dir` key corresponds to the `--output-dir` option
- the only mandatory key is the `input` (similar to the command-line configuration)
- `save-config` option is not supported, as it does not make sense here
- the default values are the same as in the command-line configuration

#### Examples

The following command results in creating the YAML displayed below:

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
