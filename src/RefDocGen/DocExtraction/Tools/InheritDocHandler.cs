using RefDocGen.CodeElements.Concrete;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen.DocExtraction.Tools;

internal class InheritDocHandler
{
    /// <summary>
    /// Registry of the declared types, to which the documentation comments will be added.
    /// </summary>
    private readonly TypeRegistry typeRegistry;

    public InheritDocHandler(TypeRegistry typeRegistry)
    {
        this.typeRegistry = typeRegistry;
    }

    internal void AddFromParent(ObjectTypeData type, MethodData method)
    {
        var parentTypeName = type.BaseType;

        if (parentTypeName is null)
        {
            return;
        }

        if (typeRegistry.ObjectTypes.TryGetValue(parentTypeName.Id, out var parent))
        {
            if (parent.Methods.TryGetValue(method.Id, out var parentMethod))
            {
                method.SummaryDocComment = parentMethod.SummaryDocComment;
                method.ReturnValueDocComment = parentMethod.ReturnValueDocComment;
                method.RemarksDocComment = parentMethod.RemarksDocComment;
                method.Exceptions = parentMethod.Exceptions;

                for (int i = 0; i < parentMethod.Parameters.Count; i++)
                {
                    method.Parameters[i].DocComment = parentMethod.Parameters[i].DocComment;
                }
            }
        }
    }
}
