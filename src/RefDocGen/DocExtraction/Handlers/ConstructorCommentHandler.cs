using RefDocGen.DocExtraction.Handlers.Abstract;
using RefDocGen.MemberData;
using RefDocGen.MemberData.Abstract;

namespace RefDocGen.DocExtraction.Handlers;

internal class ConstructorCommentHandler : InvokableMemberCommentHandler
{
    protected override InvokableMemberData[] GetMemberCollection(ClassData type)
    {
        return type.Constructors;
    }
}
