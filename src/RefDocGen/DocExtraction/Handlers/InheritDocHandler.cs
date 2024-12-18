using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

record MemberRecord(ObjectTypeData Type, string MemberId, XElement DocComment);

internal class InheritDocHandler
{
    /// <summary>
    /// Registry of the declared types, to which the documentation comments will be added.
    /// </summary>
    private readonly TypeRegistry typeRegistry;
    private List<XElement> toReturn = [];

    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    internal IEnumerable<XElement> Handle(List<MemberRecord> inheritDocs)
    {
        foreach (var inheritDoc in inheritDocs)
        {
            Handle(inheritDoc);
        }

        return toReturn;
    }

    private void Handle(MemberRecord memberRecord)
    {
        var parentTypes = new List<ITypeNameData>();

        var baseType = memberRecord.Type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(memberRecord.Type.Interfaces);

        foreach (var parentType in parentTypes)
        {
            var id = parentType.HasTypeParameters
                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
                : parentType.Id;

            if (memberRecord.Type.AllMembers.TryGetValue(memberRecord.MemberId, out var member)
                && typeRegistry.ObjectTypes.TryGetValue(id, out var parent))
            {
                if (parent.AllMembers.TryGetValue(memberRecord.MemberId, out var parentMember))
                {
                    if (parentMember.RawDocComment is null)
                    {
                        continue;
                    }

                    memberRecord.DocComment.RemoveNodes();
                    memberRecord.DocComment.Add(parentMember.RawDocComment.Nodes());

                    toReturn.Add(memberRecord.DocComment);
                }
            }
        }
    }
}
