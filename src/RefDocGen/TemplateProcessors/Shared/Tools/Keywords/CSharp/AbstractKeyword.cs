using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types;
using RefDocGen.CodeElements.Types.Abstract;

namespace RefDocGen.TemplateProcessors.Shared.Tools.Keywords.CSharp;

/// <summary>
/// Static class containing additional methods related to the 'abstract' keyword.
/// </summary>
internal static class AbstractKeyword
{
    /// <summary>
    /// Checks whether the 'abstract' keyword is present in the provided type definition.
    /// </summary>
    /// <param name="type">Type that we check for 'abstract' keyword.</param>
    /// <returns>Boolean representing if the 'abstract' keyword is present in the type definition.</returns>
    internal static bool IsPresentIn(IObjectTypeData type)
    {
        return type.IsAbstract && type.Kind == ObjectTypeKind.Class && !type.IsSealed;
    }

    /// <summary>
    /// Checks whether the 'abstract' keyword is present in the provided member signature.
    /// </summary>
    /// <param name="memberData">Member that we check for 'abstract' keyword.</param>
    /// <returns>Boolean representing if the 'abstract' keyword is present in the member signature.</returns>
    internal static bool IsPresentIn(ICallableMemberData memberData)
    {
        if (memberData.ContainingType is IObjectTypeData objectType
            && objectType.Kind == ObjectTypeKind.Interface)
        {
            return false; // for all interface members, return false.
        }

        return memberData.IsAbstract;
    }
}
