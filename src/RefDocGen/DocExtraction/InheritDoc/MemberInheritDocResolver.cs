using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.TypeRegistry;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.DocExtraction.Tools;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.InheritDoc;

/// <summary>
/// Class responsible for handling the 'inheritdoc' comments provided to type members and replacing them with the actual documentation.
/// </summary>
/// <remarks>
/// This class isn't intended to handle inheritdoc comments with 'cref' attribute (for further info, see <see cref="CrefInheritDocResolver"/>).
/// </remarks>
internal class MemberInheritDocResolver : InheritDocResolver<MemberData>
{
    /// <inheritdoc/>
    public MemberInheritDocResolver(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    /// <inheritdoc/>
    protected override InheritDocKind InheritDocKind => InheritDocKind.NonCref;

    /// <inheritdoc/>
    protected override IReadOnlyList<MemberData> GetParentNodes(MemberData member)
    {
        var type = member.ContainingType;
        var parentTypes = new List<ITypeNameData>();
        var result = new List<MemberData>();

        var baseType = type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType); // add base type
        }

        parentTypes.AddRange(type.Interfaces); // add implemented interfaces

        foreach (var parentType in parentTypes)
        {
            string parentId = parentType.TypeDeclarationId;

            // the parent type is contained in the type registry
            if (typeRegistry.GetDeclaredType(parentId) is TypeDeclaration parentDecl)
            {
                string memberId = member.Id;

                if (member is ICallableMemberData callableMember && callableMember.IsExplicitImplementation) // if the member is an explicit interface implementation, use just the member ID
                {
                    string input = callableMember.Id;
                    int lastIndex = input.LastIndexOf('#');

                    memberId = lastIndex >= 0 && lastIndex < input.Length - 1
                        ? input[(lastIndex + 1)..]
                        : callableMember.Id;
                }

                if (parentDecl.AllMembers.TryGetValue(memberId, out var parentMember)) // the member with the same ID is found in the parent type
                {
                    result.Add(parentMember);
                    continue;
                }

                var foundMembers = parentDecl.AllMembers // finds the member if the parent type is an instantiated generic type (e.g., List<string>), but the original type is generic (e.g., List<T>) - so the member IDs may differ
                    .Where(pair => GetInstantiatedMemberId(pair.Key, parentType) == member.Id)
                    .Select(pair => pair.Value);

                if (foundMembers.Any())
                {
                    result.Add(foundMembers.First());
                }
            }
        }

        return result;
    }

    /// <inheritdoc/>
    protected override XElement? GetRawDocumentation(MemberData node)
    {
        return node.RawDocComment;
    }

    /// <summary>
    /// Gets the member ID in an instantiated generic type.
    /// </summary>
    /// <remarks>
    /// The returned ID may be different, because the <paramref name="instantiatedGenericType"/> uses actual types instead of generic type parameters,
    /// so these placeholders (like "`0") are replaced by the actual type IDs.
    /// </remarks>
    /// <param name="memberId">ID of the member to return.</param>
    /// <param name="instantiatedGenericType">A generic type, possibly having some generic parameters replaced by actual types.</param>
    /// <returns>The ID of the same member in <paramref name="instantiatedGenericType"/>.</returns>
    private string GetInstantiatedMemberId(string memberId, ITypeNameData instantiatedGenericType)
    {
        string id = memberId;
        int index = 0;

        foreach (var typeParam in instantiatedGenericType.TypeParameters)
        {
            id = id.Replace($"`{index}", typeParam.Id);

            index++;
        }

        return id;
    }
}
