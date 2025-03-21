using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements.Concrete;

/// <inheritdoc/>
internal class TypeRegistry : ITypeRegistry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistry"/> class.
    /// </summary>
    /// <param name="objectTypes"><inheritdoc cref="ObjectTypes"/></param>
    /// <param name="enums"><inheritdoc cref="Enums"/></param>
    /// <param name="delegates"><inheritdoc cref="Delegates"/></param>
    public TypeRegistry(
        IReadOnlyDictionary<string, ObjectTypeData> objectTypes,
        IReadOnlyDictionary<string, EnumTypeData> enums,
        IReadOnlyDictionary<string, DelegateTypeData> delegates)
    {
        ObjectTypes = objectTypes;
        Enums = enums;
        Delegates = delegates;

        AllTypes = ObjectTypes.ToParent<string, ObjectTypeData, TypeDeclaration>()
            .Merge(Enums.ToParent<string, EnumTypeData, TypeDeclaration>())
            .Merge(Delegates.ToParent<string, DelegateTypeData, TypeDeclaration>());
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

}
