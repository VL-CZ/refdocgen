using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Types;
using System.Xml.Linq;

namespace RefDocGen.DocExtraction.Handlers;

record MemberRecord(ObjectTypeData Type, string MemberId, XElement DocComment);

record Node(ObjectTypeData Type, string MemberId);

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
        var node = new Node(memberRecord.Type, memberRecord.MemberId);
        var docComment = Handle(node);

        if (docComment is null)
        {
            return;
        }

        memberRecord.DocComment.RemoveNodes();
        memberRecord.DocComment.Add(docComment.Nodes());

        toReturn.Add(memberRecord.DocComment);
    }

    private XElement? Handle(Node node)
    {
        var parentTypes = GetParentTypes(node.Type);

        foreach (var parentType in parentTypes)
        {
            var parentId = parentType.HasTypeParameters
                ? $"{parentType.FullName}`{parentType.TypeParameters.Count}"
                : parentType.Id;

            if (node.Type.AllMembers.TryGetValue(node.MemberId, out var member)
                && typeRegistry.ObjectTypes.TryGetValue(parentId, out var parent))
            {
                // desired member found
                if (parent.AllMembers.TryGetValue(node.MemberId, out var parentMember))
                {
                    if (parentMember.RawDocComment is null)
                    {
                        var parentNode = new Node(parent, node.MemberId);
                        var parentDocComment = Handle(parentNode);

                        if (parentDocComment is not null)
                        {
                            return parentDocComment;
                        }
                    }
                    else
                    {
                        return parentMember.RawDocComment;
                    }
                }
            }
        }

        return null;
    }

    private IEnumerable<ITypeNameData> GetParentTypes(ObjectTypeData type)
    {
        var parentTypes = new List<ITypeNameData>();

        var baseType = type.BaseType;

        if (baseType is not null)
        {
            parentTypes.Add(baseType);
        }

        parentTypes.AddRange(type.Interfaces);

        return parentTypes;
    }
}
