using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.InheritDoc;
using System.Xml.Linq;

internal class MemberInheritDocHandler : InheritDocHandler<MemberData>
{
    public MemberInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    protected override List<MemberData> GetParentNodes(MemberData member)
    {
        var parents = GetDeclaredParents(member.DeclaringType);

        var result = new List<MemberData>();

        foreach (var p in parents)
        {
            if (p.AllMembers.TryGetValue(member.Id, out var parentMember))
            {
                result.Add(parentMember);
            }
        }

        return result;
    }

    protected override XElement? GetRawDocumentation(MemberData member)
    {
        return member.RawDocComment;
    }
}

internal class TypeInheritDocHandler : InheritDocHandler<TypeDeclaration>
{
    public TypeInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    protected override List<TypeDeclaration> GetParentNodes(TypeDeclaration node)
    {
        return GetDeclaredParents(node);
    }

    protected override XElement? GetRawDocumentation(TypeDeclaration member)
    {
        return member.RawDocComment;
    }
}
