using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract;
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
        var constructorTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Constructors).Select(GetFrom);
        var methodTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Methods).Select(GetFrom);
        var indexerTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Indexers).Select(GetFrom);
        var operatorTemplateModels = typeRegistry.ObjectTypes.SelectMany(t => t.Operators).Select(GetFrom);

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

    internal static SearchPageTM GetFrom(NamespaceData namespaceData)
    {
        return new(namespaceData.Name + " namespace", string.Empty, namespaceData.Name);
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
        return new(CSharpTypeName.Of(method.ContainingType, useFullName: true) + "." + method.Name + " method",
            method.SummaryDocComment.Value,
            method.ContainingType.Id,
            method.Id);
    }

    internal static SearchPageTM GetFrom(IFieldData field)
    {
        return new(CSharpTypeName.Of(field.ContainingType, useFullName: true) + "." + field.Name + " field",
            field.SummaryDocComment.Value,
            field.ContainingType.Id,
            field.Id);
    }

    internal static SearchPageTM GetFrom(IPropertyData property)
    {
        return new(CSharpTypeName.Of(property.ContainingType, useFullName: true) + "." + property.Name + " property",
            property.SummaryDocComment.Value,
            property.ContainingType.Id,
            property.Id);
    }

    internal static SearchPageTM GetFrom(IEventData e)
    {
        return new(CSharpTypeName.Of(e.ContainingType, useFullName: true) + "." + e.Name + " event",
            e.SummaryDocComment.Value,
            e.ContainingType.Id,
            e.Id);
    }

    internal static SearchPageTM GetFrom(IConstructorData constructor)
    {
        return new(CSharpTypeName.Of(constructor.ContainingType, useFullName: true) + " constructor",
            constructor.SummaryDocComment.Value,
            constructor.ContainingType.Id,
            constructor.Id);
    }

    internal static SearchPageTM GetFrom(IIndexerData indexer)
    {
        return new(CSharpTypeName.Of(indexer.ContainingType, useFullName: true) + " indexer",
            indexer.SummaryDocComment.Value,
            indexer.ContainingType.Id,
            indexer.Id);
    }

    internal static SearchPageTM GetFrom(IOperatorData op)
    {
        return new(CSharpTypeName.Of(op.ContainingType, useFullName: true) + "." + op.Name + " operator",
            op.SummaryDocComment.Value,
            op.ContainingType.Id,
            op.Id);
    }
}
