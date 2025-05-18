using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the namespaces of a program.
/// </summary>
internal class SearchPageTMCreator
{
    internal static IEnumerable<SearchPageTM> GetFrom(ITypeRegistry typeRegistry)
    {
        var typeTemplateModels = typeRegistry.ObjectTypes.Select(GetFrom);
        var nsTemplateModels = typeRegistry.Namespaces.Select(GetFrom);
        var assemblyTemplateModels = typeRegistry.Assemblies.Select(GetFrom);

        var fieldTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Fields).Select(GetFrom);
        var propertyTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Properties).Select(GetFrom);
        var eventTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Events).Select(GetFrom);

        var constructorTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Constructors.DistinctByName()).Select(GetFrom);
        var methodTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Methods.DistinctByName()).Select(GetFrom);
        var indexerTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Indexers.DistinctByName()).Select(GetFrom);
        var operatorTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Operators.DistinctByName()).Select(GetFrom);

        return [
            .. assemblyTemplateModels,
            .. nsTemplateModels,
            .. typeTemplateModels,
            .. fieldTemplateModels,
            .. propertyTemplateModels,
            .. eventTemplateModels,
            .. constructorTemplateModels,
            .. methodTemplateModels,
            .. indexerTemplateModels,
            .. operatorTemplateModels];
    }

    /// <summary>
    /// Creates a <see cref="SearchPageTM"/> instance from the specified <see cref="NamespaceData"/>.
    /// </summary>
    /// <param name="ns">The namespace data to create the search page model from.</param>
    /// <returns>A <see cref="SearchPageTM"/> instance representing the search result for the given namespace.</returns>
    internal static SearchPageTM GetFrom(NamespaceData ns)
    {
        return new(ns.Name + " namespace", string.Empty, ns.Name);
    }

    internal static SearchPageTM GetFrom(AssemblyData assembly)
    {
        return new(assembly.Name + " assembly", string.Empty, assembly.Name);
    }

    internal static SearchPageTM GetFrom(ITypeDeclaration type)
    {
        return new(CSharpTypeName.Of(type, useFullName: true) + " type", type.SummaryDocComment.Value, type.Id);
    }

    internal static SearchPageTM GetFrom(IMethodData method)
    {
        return GetFrom(method, "method");
    }

    internal static SearchPageTM GetFrom(IFieldData field)
    {
        return GetFrom(field, "field");
    }

    internal static SearchPageTM GetFrom(IPropertyData property)
    {
        return GetFrom(property, "property");
    }

    internal static SearchPageTM GetFrom(IEventData e)
    {
        return GetFrom(e, "event");
    }

    internal static SearchPageTM GetFrom(IConstructorData constructor)
    {
        return GetFrom(constructor, "constructor", false);
    }

    internal static SearchPageTM GetFrom(IIndexerData indexer)
    {
        return GetFrom(indexer, "indexer", false);
    }

    internal static SearchPageTM GetFrom(IOperatorData op)
    {
        return GetFrom(op, "operator");
    }

    /// <summary>
    /// Creates a <see cref="SearchPageTM"/> from the given <paramref name="member"/> instance.
    /// </summary>
    /// <param name="member">The <see cref="IMemberData"/> instance to convert.</param>
    /// <param name="memberKindName">Name of the member kind to be displayed (e.g., 'field').</param>
    /// <param name="showMemberName">Whether the member name should be included in the resulting <see cref="SearchPageTM"/>.</param>
    /// <returns>A <see cref="SearchPageTM"/> instance based on the provided <paramref name="member"/> instance.</returns>
    internal static SearchPageTM GetFrom(IMemberData member, string memberKindName, bool showMemberName = true)
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
