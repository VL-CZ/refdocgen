using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.TemplateGenerators.Tools.Keywords;

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
        return type.IsAbstract && type.Kind == TypeKind.Class && !type.IsSealed;
    }

    /// <summary>
    /// Checks whether the 'abstract' keyword is present in the provided member signature.
    /// </summary>
    /// <param name="memberData">Member that we check for 'abstract' keyword.</param>
    /// <returns>Boolean representing if the 'abstract' keyword is present in the member signature.</returns>
    internal static bool IsPresentIn(ICallableMemberData memberData)
    {
        if (memberData.ContainingType is IObjectTypeData objectType
            && objectType.Kind == TypeKind.Interface)
        {
            return false; // for all interface members, return false.
        }

        return memberData.IsAbstract;
    }
}
