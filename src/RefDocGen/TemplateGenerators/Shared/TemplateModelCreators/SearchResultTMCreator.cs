using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Abstract.Enum;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the search result template models.
/// </summary>
internal class SearchResultTMCreator
{
    /// <summary>
    /// Creates an enumerable of <see cref="SearchResultTM"/> instances based on the data stored in the <paramref name="typeRegistry"/>.
    /// </summary>
    /// <param name="typeRegistry">A registry containing the types to be included in the documentation.
    /// </param>
    /// <returns>An enumerable of <see cref="SearchResultTM"/> instances based on the data stored in the <paramref name="typeRegistry"/>.</returns>
    internal static IEnumerable<SearchResultTM> GetFrom(ITypeRegistry typeRegistry)
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
    internal static SearchResultTM GetFrom(NamespaceData ns)
    {
        return new(ns.Name + " namespace", string.Empty, ns.Name);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="AssemblyData"/>.
    /// </summary>
    /// <param name="assembly">The assembly data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given assembly.</returns>
    internal static SearchResultTM GetFrom(AssemblyData assembly)
    {
        return new(assembly.Name + " assembly", string.Empty, assembly.Name + "-DLL");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IObjectTypeData"/>.
    /// </summary>
    /// <param name="type">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given type.</returns>
    internal static SearchResultTM GetFrom(IObjectTypeData type)
    {
        return GetFrom(type, type.Kind.GetName());
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEnumTypeData"/>.
    /// </summary>
    /// <param name="e">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given enum.</returns>
    internal static SearchResultTM GetFrom(IEnumTypeData e)
    {
        return GetFrom(e, "enum");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IDelegateTypeData"/>.
    /// </summary>
    /// <param name="type">The type declaration to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given delegate.</returns>
    internal static SearchResultTM GetFrom(IDelegateTypeData type)
    {
        return GetFrom(type, "delegate");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IMethodData"/>.
    /// </summary>
    /// <param name="method">The method data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given method.</returns>
    internal static SearchResultTM GetFrom(IMethodData method)
    {
        return GetFrom(method, "method");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IFieldData"/>.
    /// </summary>
    /// <param name="field">The field data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given field.</returns>
    internal static SearchResultTM GetFrom(IFieldData field)
    {
        return GetFrom(field, "field");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IPropertyData"/>.
    /// </summary>
    /// <param name="property">The property data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given property.</returns>
    internal static SearchResultTM GetFrom(IPropertyData property)
    {
        return GetFrom(property, "property");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEventData"/>.
    /// </summary>
    /// <param name="e">The event data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given event.</returns>
    internal static SearchResultTM GetFrom(IEventData e)
    {
        return GetFrom(e, "event");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IConstructorData"/>.
    /// </summary>
    /// <param name="constructor">The constructor data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given constructor.</returns>
    internal static SearchResultTM GetFrom(IConstructorData constructor)
    {
        return GetFrom(constructor, "constructor", false);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IIndexerData"/>.
    /// </summary>
    /// <param name="indexer">The indexer data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given indexer.</returns>
    internal static SearchResultTM GetFrom(IIndexerData indexer)
    {
        return GetFrom(indexer, "indexer", false);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IOperatorData"/>.
    /// </summary>
    /// <param name="op">The operator data to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given operator.</returns>
    internal static SearchResultTM GetFrom(IOperatorData op)
    {
        return GetFrom(op, "operator");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> instance from the specified <see cref="IEnumMemberData"/>.
    /// </summary>
    /// <param name="member">The enum member to create the search page model from.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance representing the search result for the given enum member.</returns>
    internal static SearchResultTM GetFrom(IEnumMemberData member)
    {
        return GetFrom(member, "enum member");
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> from the given <paramref name="type"/> instance.
    /// </summary>
    /// <param name="type">The <see cref="ITypeDeclaration"/> instance to convert.</param>
    /// <param name="typeKindName">Name of the type kind to be displayed (e.g., 'class').</param>
    /// <returns>A <see cref="SearchResultTM"/> instance based on the provided <paramref name="type"/> instance.</returns>
    internal static SearchResultTM GetFrom(ITypeDeclaration type, string typeKindName)
    {
        return new(CSharpTypeName.Of(type, useFullName: true) + " " + typeKindName, type.SummaryDocComment.Value, type.Id);
    }

    /// <summary>
    /// Creates a <see cref="SearchResultTM"/> from the given <paramref name="member"/> instance.
    /// </summary>
    /// <param name="member">The <see cref="IMemberData"/> instance to convert.</param>
    /// <param name="memberKindName">Name of the member kind to be displayed (e.g., 'field').</param>
    /// <param name="showMemberName">Whether the member name should be included in the resulting <see cref="SearchResultTM"/>.</param>
    /// <returns>A <see cref="SearchResultTM"/> instance based on the provided <paramref name="member"/> instance.</returns>
    internal static SearchResultTM GetFrom(IMemberData member, string memberKindName, bool showMemberName = true)
    {
        string name = CSharpTypeName.Of(member.ContainingType, useFullName: true);

        if (showMemberName)
        {
            name += $".{member.Name}";
        }

        name += $" {memberKindName}";

        return new(name,
            member.SummaryDocComment.Value,
            member.ContainingType.Id,
            member.Id);
    }
}
