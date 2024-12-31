using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Delegate;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.DocExtraction.Tools;

namespace RefDocGen.CodeElements.Concrete;

/// <summary>
/// Represents a registry of declared types.
/// </summary>
/// <param name="ObjectTypes">A collection of the declared value, reference and interface types, indexed by their IDs.</param>
/// <param name="Enums">A collection of the declared enum types, indexed by their IDs.</param>
/// <param name="Delegates">A collection of the declared delegate types, indexed by their IDs.</param>
internal record TypeRegistry(
    IReadOnlyDictionary<string, ObjectTypeData> ObjectTypes,
    IReadOnlyDictionary<string, EnumTypeData> Enums,
    IReadOnlyDictionary<string, DelegateTypeData> Delegates
    ) : ITypeRegistry
{
    /// <inheritdoc/>
    IEnumerable<IObjectTypeData> ITypeRegistry.ObjectTypes => ObjectTypes.Values;

    /// <inheritdoc/>
    IEnumerable<IEnumTypeData> ITypeRegistry.Enums => Enums.Values;

    /// <inheritdoc/>
    IEnumerable<IDelegateTypeData> ITypeRegistry.Delegates => Delegates.Values;


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
}
