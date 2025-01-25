namespace RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

public record AttributeTM(
    string Name,
    string? Url,
    string?[] ConstructorArguments,
    (TypeLinkTM, string?)[] NamedArguments);
