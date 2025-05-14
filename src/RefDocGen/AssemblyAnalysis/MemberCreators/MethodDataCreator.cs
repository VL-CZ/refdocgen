using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.Tools;
using System.Reflection;

namespace RefDocGen.AssemblyAnalysis.MemberCreators;

/// <summary>
/// Class responsible for creating <see cref="MethodData"/> instances.
/// </summary>
internal static class MethodDataCreator
{
    /// <summary>
    /// Creates a <see cref="MethodData"/> instance from the corresponding <paramref name="methodInfo"/>.
    /// </summary>
    /// <param name="methodInfo"><see cref="MethodInfo"/> object representing the method.</param>
    /// <param name="containingType">Type containing the method.</param>
    /// <param name="availableTypeParameters">A dictionary of available type parameters; the keys represent type parameter names.</param>
    /// <returns>A <see cref="MethodData"/> instance representing the method.</returns>
    internal static MethodData CreateFrom(MethodInfo methodInfo, TypeDeclaration containingType, Dictionary<string, TypeParameterData> availableTypeParameters)
    {
        var declaredTypeParameters = MemberCreatorHelper.CreateTypeParametersDictionary(methodInfo);
        var allTypeParameters = availableTypeParameters.Merge(declaredTypeParameters);

        return new MethodData(
            methodInfo,
            containingType,
            MemberCreatorHelper.CreateParametersDictionary(methodInfo, allTypeParameters),
            declaredTypeParameters,
            allTypeParameters,
            MemberCreatorHelper.GetAttributeData(methodInfo, allTypeParameters));
    }
}
