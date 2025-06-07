using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Abstract.Enum;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateProcessors.Shared.DocComments.Html;
using RefDocGen.TemplateProcessors.Shared.Languages;
using RefDocGen.TemplateProcessors.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateProcessors.Shared.TemplateModels.Types;
using RefDocGen.TemplateProcessors.Shared.Tools;

namespace RefDocGen.TemplateProcessors.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the search result template models.
/// </summary>
internal class SearchResultTMCreator : BaseTMCreator
{
    public SearchResultTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
    {
    }


    /// <summary>
    /// Creates an enumerable of <see cref="SearchResultTM"/> instances based on the data stored in the <paramref name="typeRegistry"/>.
    /// </summary>
    /// <param name="typeRegistry">A registry containing the types to be included in the documentation.
    /// </param>
    /// <returns>An enumerable of <see cref="SearchResultTM"/> instances based on the data stored in the <paramref name="typeRegistry"/>.</returns>
    internal IEnumerable<SearchResultTM> GetFrom(ITypeRegistry typeRegistry)
    {
        var nsModels = typeRegistry.Namespaces.Select(GetFrom);
        var assemblyModels = typeRegistry.Assemblies.Select(GetFrom);

        var objectTypeModels = typeRegistry.ObjectTypes.Select(GetFrom);
        var enumModels = typeRegistry.Enums.Select(GetFrom);
        var delegateModels = typeRegistry.Delegates.Select(GetFrom);

        var fieldModels = typeRegistry.ObjectTypes.SelectMany(t => t.Fields).Select(GetFrom);
        var propertyModels = typeRegistry.ObjectTypes.SelectMany(t => t.Properties).Select(GetFrom);
        var eventModels = typeRegistry.ObjectTypes.SelectMany(t => t.Events).Select(GetFrom);

        var constructorModels = typeRegistry.ObjectTypes.SelectMany(t => t.Constructors.DistinctByName()).Select(GetFrom);
        var methodModels = typeRegistry.ObjectTypes.SelectMany(t => t.Methods.DistinctByName()).Select(GetFrom);
        var indexerModels = typeRegistry.ObjectTypes.SelectMany(t => t.Indexers.DistinctByName()).Select(GetFrom);
        var operatorModels = typeRegistry.ObjectTypes.SelectMany(t => t.Operators.DistinctByName()).Select(GetFrom);

        var enumMemberModels = typeRegistry.Enums.SelectMany(e => e.Members.OrderBy(m => m.Value)).Select(GetFrom);

        return [
            .. assemblyModels,
            .. nsModels,
            .. objectTypeModels,
            .. enumModels,
            .. delegateModels,
            .. fieldModels,
            .. propertyModels,
            .. eventModels,
            .. constructorModels,
            .. methodModels,
            .. indexerModels,
            .. operatorModels,
            .. enumMemberModels];
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="NamespaceData"/>.
    /// </summary>
    /// <param name="ns">The namespace data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given namespace.</returns>
    internal SearchResultTM GetFrom(NamespaceData ns)
    {
        var name = GetLanguageSpecificData(_ => $"{ns.Name} namespace");
        return new(name, string.Empty, ns.Name);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="AssemblyData"/>.
    /// </summary>
    /// <param name="assembly">The assembly data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given assembly.</returns>
    internal SearchResultTM GetFrom(AssemblyData assembly)
    {
        var name = GetLanguageSpecificData(_ => $"{assembly.Name} assembly");
        return new(name, string.Empty, assembly.Name + "-DLL");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IObjectTypeData"/>.
    /// </summary>
    /// <param name="type">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given type.</returns>
    internal SearchResultTM GetFrom(IObjectTypeData type)
    {
        return GetFrom(type, TypeKindName.Of(type));
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEnumTypeData"/>.
    /// </summary>
    /// <param name="e">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given enum.</returns>
    internal SearchResultTM GetFrom(IEnumTypeData e)
    {
        return GetFrom(e, TypeKindName.Enum);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IDelegateTypeData"/>.
    /// </summary>
    /// <param name="d">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given delegate.</returns>
    internal SearchResultTM GetFrom(IDelegateTypeData d)
    {
        return GetFrom(d, TypeKindName.Delegate);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IMethodData"/>.
    /// </summary>
    /// <param name="method">The method data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given method.</returns>
    internal SearchResultTM GetFrom(IMethodData method)
    {
        return GetFrom(method, "method");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IFieldData"/>.
    /// </summary>
    /// <param name="field">The field data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given field.</returns>
    internal SearchResultTM GetFrom(IFieldData field)
    {
        return GetFrom(field, "field");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IPropertyData"/>.
    /// </summary>
    /// <param name="property">The property data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given property.</returns>
    internal SearchResultTM GetFrom(IPropertyData property)
    {
        return GetFrom(property, "property");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEventData"/>.
    /// </summary>
    /// <param name="e">The event data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given event.</returns>
    internal SearchResultTM GetFrom(IEventData e)
    {
        return GetFrom(e, "event");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IConstructorData"/>.
    /// </summary>
    /// <param name="constructor">The constructor data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given constructor.</returns>
    internal SearchResultTM GetFrom(IConstructorData constructor)
    {
        return GetFrom(constructor, "constructor", false);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IIndexerData"/>.
    /// </summary>
    /// <param name="indexer">The indexer data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given indexer.</returns>
    internal SearchResultTM GetFrom(IIndexerData indexer)
    {
        return GetFrom(indexer, "indexer", false);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IOperatorData"/>.
    /// </summary>
    /// <param name="op">The operator data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given operator.</returns>
    internal SearchResultTM GetFrom(IOperatorData op)
    {
        return GetFrom(op, "operator");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEnumMemberData"/>.
    /// </summary>
    /// <param name="member">The enum member to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given enum member.</returns>
    internal SearchResultTM GetFrom(IEnumMemberData member)
    {
        return GetFrom(member, "enum member");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> from the given <paramref name="type"/> instance.
    /// </summary>
    /// <param name="type">The <see cref="ITypeDeclaration"/> instance to convert.</param>
    /// <param name="typeKindName">Name of the type kind to be displayed (e.g., 'class').</param>
    /// <returns>A <see cref="SearchResultTM"/> instance based on the provided <paramref name="type"/> instance.</returns>
    internal SearchResultTM GetFrom(ITypeDeclaration type, string typeKindName)
    {
        var localizedNames = GetLanguageSpecificData(lang =>
        {
            string name = lang.GetTypeName(type);
            return $"{type.Namespace}.{name} {typeKindName}";
        });

        string typeId = TemplateId.Of(type);

        return new(localizedNames, ToHtmlOneLineString(type.SummaryDocComment), typeId);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> from the given <paramref name="member"/> instance.
    /// </summary>
    /// <param name="member">The <see cref="IMemberData"/> instance to convert.</param>
    /// <param name="memberKindName">Name of the member kind to be displayed (e.g., 'field').</param>
    /// <param name="showMemberName">Whether the member name should be included in the resulting <see cref="SearchResultTM"/>.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance based on the provided <paramref name="member"/> instance.</returns>
    internal SearchResultTM GetFrom(IMemberData member, string memberKindName, bool showMemberName = true)
    {
        var localizedNames = GetLanguageSpecificData(lang =>
        {
            string typeName = lang.GetTypeName(member.ContainingType);
            string result = $"{member.ContainingType.Namespace}.{typeName}";

            if (showMemberName)
            {
                result += $".{member.Name}";
            }

            result += $" {memberKindName}";
            return result;
        });

        return new(localizedNames,
            ToHtmlOneLineString(member.SummaryDocComment),
            TemplateId.Of(member.ContainingType),
            TemplateId.Of(member));
    }
}
