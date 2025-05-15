using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="OperatorData"/> instances.
/// </summary>
internal static class OperatorDataCreator
{
    /// <summary>
    /// Creates a <see cref="OperatorData"/> instance from the corresponding <paramref name="methodInfo"/>.
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/> object representing the operator.</param>
    /// <param name="containingType">Type containing the operator.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="OperatorData"/> instance representing the operator.</returns>
    internal static OperatorData CreateFrom(MethodInfo methodInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        return new OperatorData(
            methodInfo,
            containingType,
            MemberCreatorHelper.CreateParametersDictionary(methodInfo, availableTypeParameters),
            MemberCreatorHelper.CreateTypeParametersDictionary(methodInfo),
            availableTypeParameters,
            MemberCreatorHelper.GetAttributeData(methodInfo, availableTypeParameters));
    }
}
