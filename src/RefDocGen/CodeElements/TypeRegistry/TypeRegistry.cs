using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.CodeElements.Types.Concrete.Delegate;
using RefDocGen.CodeElements.Types.Concrete.Enum;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.TypeRegistry;

/// <inheritdoc/>
internal class TypeRegistry : ITypeRegistry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistry"/> class.
    /// </summary>
    /// <param name="objectTypes"><inheritdoc cref="ITypeRegistry.ObjectTypes"/></param>
    /// <param name="enums"><inheritdoc cref="ITypeRegistry.Enums"/></param>
    /// <param name="delegates"><inheritdoc cref="ITypeRegistry.Delegates"/></param>
    public TypeRegistry(
        IEnumerable<ObjectTypeData> objectTypes,
        IEnumerable<EnumTypeData> enums,
        IEnumerable<DelegateTypeData> delegates)
    {
        ObjectTypes = objectTypes.ToIdDictionary();
        Enums = enums.ToIdDictionary();
        Delegates = delegates.ToIdDictionary();

        AllTypes = ObjectTypes.ToParent<string, ObjectTypeData, TypeDeclaration>()
            .Merge(Enums.ToParent<string, EnumTypeData, TypeDeclaration>())
            .Merge(Delegates.ToParent<string, DelegateTypeData, TypeDeclaration>());

        Assemblies = GetAssemblyData();
        Namespaces = GetNamespaces();
    }

    /// <summary>
    /// A collection of the declared value, reference, and interface types, indexed by their IDs.
    /// </summary>
    public IReadOnlyDictionary<string, ObjectTypeData> ObjectTypes { get; }

    /// <summary>
    /// A collection of the declared enum types, indexed by their IDs.
    /// </summary>
    public IReadOnlyDictionary<string, EnumTypeData> Enums { get; }

    /// <summary>
    /// A collection of the declared delegate types, indexed by their IDs.
    /// </summary>
    public IReadOnlyDictionary<string, DelegateTypeData> Delegates { get; }

    /// <summary>
    /// A collection of the all declared types (object types, enums and delegates), indexed by their IDs.
    /// </summary>
    public IReadOnlyDictionary<string, TypeDeclaration> AllTypes { get; }

    /// <inheritdoc/>
    IEnumerable<IObjectTypeData> ITypeRegistry.ObjectTypes => ObjectTypes.Values;

    /// <inheritdoc/>
    IEnumerable<IEnumTypeData> ITypeRegistry.Enums => Enums.Values;

    /// <inheritdoc/>
    IEnumerable<IDelegateTypeData> ITypeRegistry.Delegates => Delegates.Values;

    /// <inheritdoc/>
    public IEnumerable<AssemblyData> Assemblies { get; }

    /// <inheritdoc/>
    public IEnumerable<NamespaceData> Namespaces { get; }

    /// <summary>
    /// Gets the member by its type ID and member ID.
    /// </summary>
    /// <param name="typeMemberId">Type ID + Member ID concatenated with '.' (i.e. <c>$"{typeId}.{memberId}"</c>).</param>
    /// <returns>Member with the given ID contained in the given type given ID. <c>null</c> if no such member exists.</returns>
    internal MemberData? GetMember(string typeMemberId)
    {
        (string typeId, string memberName, string paramsString) = MemberSignatureParser.Parse(typeMemberId);
        string memberId = memberName + paramsString;

        var foundType = GetDeclaredType(typeId);

        return foundType?.AllMembers.GetValueOrDefault(memberId);
    }

    /// <inheritdoc cref="ITypeRegistry.GetDeclaredType(string)"/>
    internal TypeDeclaration? GetDeclaredType(string typeId)
    {
        if (ObjectTypes.TryGetValue(typeId, out var objectType))
        {
            return objectType;
        }
        else if (Enums.TryGetValue(typeId, out var enumType))
        {
            return enumType;
        }
        else if (Delegates.TryGetValue(typeId, out var delegateType))
        {
            return delegateType;
        }

        return null;
    }

    /// <inheritdoc/>
    ITypeDeclaration? ITypeRegistry.GetDeclaredType(string typeId)
    {
        return GetDeclaredType(typeId);
    }

    /// <summary>
    /// Get list of declared base types (parent class and implemented interfaces) of the given type.
    /// </summary>
    /// <remarks>
    /// This method only selects types declared in any of the analyzed assemblies.
    /// </remarks>
    /// <param name="type">Type whose base types are retrieved.</param>
    /// <returns>A list of declared base types (parent class and implemented interfaces) of the given type.</returns>
    internal IReadOnlyList<TypeDeclaration> GetDeclaredBaseTypes(TypeDeclaration type)
    {
        var parentTypes = new List<ITypeNameData>();
        var result = new List<TypeDeclaration>();

        var baseType = type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(type.Interfaces);

        foreach (var parentType in parentTypes)
        {
            // convert the ID: TODO refactor
            string parentId = parentType.TypeDeclarationId;

            // the parent type is contained in the type registry
            if (GetDeclaredType(parentId) is TypeDeclaration parent)
            {
                result.Add(parent);
            }
        }

        return result;
    }

    /// <summary>
    /// Get the data describing the assemblies.
    /// </summary>
    /// <returns>A list of objects describing the assemblies.</returns>
    private List<AssemblyData> GetAssemblyData()
    {
        var groupedTypes = ObjectTypes.Values.ToLookup(t => t.Assembly);
        var groupedEnums = Enums.Values.ToLookup(e => e.Assembly);
        var groupedDelegates = Delegates.Values.ToLookup(e => e.Assembly);

        var allAssemblies = AllTypes.Values.Select(t => t.Assembly).Distinct();
        List<AssemblyData> assemblies = [];

        foreach (string assembly in allAssemblies)
        {
            var assemblyTypes = groupedTypes[assembly].ToLookup(t => t.Namespace);
            var assemblyEnums = groupedEnums[assembly].ToLookup(e => e.Namespace);
            var assemblyDelegates = groupedDelegates[assembly].ToLookup(d => d.Namespace);

            var assemblyNamespaces = assemblyTypes.Select(t => t.Key)
                .Concat(assemblyEnums.Select(e => e.Key))
                .Concat(assemblyDelegates.Select(d => d.Key))
                .Distinct();

            List<NamespaceData> namespaces = [];

            foreach (string nsName in assemblyNamespaces)
            {
                var nsObjectTypes = assemblyTypes[nsName];
                var nsEnums = assemblyEnums[nsName];
                var nsDelegates = assemblyDelegates[nsName];

                namespaces.Add(new(nsName, nsObjectTypes, nsDelegates, nsEnums));
            }

            assemblies.Add(new(assembly, namespaces));
        }

        return assemblies;
    }

    /// <summary>
    /// Get the data describing the namespaces.
    /// </summary>
    /// <returns>A list of objects describing the namespaces.</returns>
    private IEnumerable<NamespaceData> GetNamespaces()
    {
        return GetAssemblyData().SelectMany(a => a.Namespaces);
    }
}
