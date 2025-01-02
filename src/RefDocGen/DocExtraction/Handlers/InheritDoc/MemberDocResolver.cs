using RefDocGen.CodeElements.Abstract;
using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.DocExtraction.Handlers.InheritDoc;
using RefDocGen.DocExtraction.Tools;
using RefDocGen.Tools.Xml;
using System;
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

internal class CrefInheritDocHandler : InheritDocHandler<XElement>
{
    public CrefInheritDocHandler(TypeRegistry typeRegistry) : base(typeRegistry)
    {
    }

    protected override List<XElement> GetNestedInheritDocs(XElement? rawDocComment)
    {
        return rawDocComment?.Descendants(XmlDocIdentifiers.InheritDoc)
            .Where(e => e.Attribute(XmlDocIdentifiers.Cref) is not null)
            .ToList() ?? [];
    }

    protected override List<XElement> GetParentNodes(XElement member)
    {
        var nestedDocs = member.Descendants(XmlDocIdentifiers.InheritDoc);

        var neighbours = new List<XElement>();

        foreach (var nestedDoc in nestedDocs)
        {
            if (nestedDoc.Attribute(XmlDocIdentifiers.Cref) is XAttribute crefAttr)
            {
                string[] splitMemberName = crefAttr.Value.Split(':');
                (string objectIdentifier, string fullObjectName) = (splitMemberName[0], splitMemberName[1]);

                var neighbour = objectIdentifier == MemberTypeId.Type
                    ? (typeRegistry.GetDeclaredType(fullObjectName)?.RawDocComment)
                    : (typeRegistry.GetMember(fullObjectName)?.RawDocComment);

                if (neighbour is not null)
                {
                    neighbours.Add(neighbour);
                }
            }
        }

        return neighbours;
    }

    protected override XElement? GetRawDocumentation(XElement member)
    {
        return member;
    }

    protected override bool IsLiteralDoc(XElement? rawDocComment)
    {
        return GetNestedInheritDocs(rawDocComment).Count == 0;
    }
}
