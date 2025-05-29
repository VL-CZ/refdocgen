using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Namespaces;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the namespaces of a program.
/// </summary>
internal class NamespaceTMCreator : BaseTMCreator
{

    public NamespaceTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
    {
    }

    /// <summary>
    /// Creates an enumerable of <see cref="NamespaceTM"/> instances based on the provided <see cref="NamespaceData"/>.
    /// </summary>
    /// <param name="namespaceData">The <see cref="NamespaceData"/> instance representing the namespace.</param>
    /// <returns>A <see cref="NamespaceTM"/> instance based on the provided <paramref name="namespaceData"/>.</returns>
    internal NamespaceTM GetFrom(NamespaceData namespaceData)
    {
        Dictionary<ObjectTypeKind, IEnumerable<TypeNameTM>> namespaceTypes = new()
        {
            [ObjectTypeKind.Class] = [],
            [ObjectTypeKind.ValueType] = [],
            [ObjectTypeKind.Interface] = [],
        };

        // get namespace classes, value types and interfaces
        foreach (var typeKind in namespaceTypes.Keys)
        {
            namespaceTypes[typeKind] = namespaceData.ObjectTypes // select the types of the given kind, ordered by their name
                .Where(t => t.Kind == typeKind)
                .Select(GetTypeNameFrom)
                .OrderBy(t => t.Name.CSharpData);
        }

        // get namespace enums
        var namespaceEnums = namespaceData.Enums
            .Select(GetTypeNameFrom)
            .OrderBy(e => e.Name.CSharpData);

        // get namespace delegates
        var namespaceDelegates = namespaceData.Delegates
            .Select(GetTypeNameFrom)
            .OrderBy(d => d.Name.CSharpData);

        return new NamespaceTM(
            namespaceData.Name,
            namespaceTypes[ObjectTypeKind.Class],
            namespaceTypes[ObjectTypeKind.ValueType],
            namespaceTypes[ObjectTypeKind.Interface],
            namespaceEnums,
            namespaceDelegates
            );
    }
}
