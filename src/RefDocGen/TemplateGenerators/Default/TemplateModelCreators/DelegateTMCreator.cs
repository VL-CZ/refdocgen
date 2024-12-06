using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual delegates.
/// </summary>
internal static class DelegateTMCreator
{
    /// <summary>
    /// Creates a <see cref="DelegateTypeTM"/> instance based on the provided <see cref="IDelegateTypeData"/> object.
    /// </summary>
    /// <param name="delegateTypeData">The <see cref="IDelegateTypeData"/> instance representing the delegate.</param>
    /// <returns>A <see cref="DelegateTypeTM"/> instance based on the provided <paramref name="delegateTypeData"/>.</returns>
    internal static DelegateTypeTM GetFrom(IDelegateTypeData delegateTypeData)
    {
        List<Keyword> modifiers = [delegateTypeData.AccessModifier.ToKeyword()];

        var parameters = delegateTypeData.Parameters.Select(ParameterTMCreator.GetFrom).ToArray();
        var typeParameterDeclarations = delegateTypeData.TypeParameterDeclarations.Select(TypeParameterTMCreator.GetFrom).ToArray();

        return new DelegateTypeTM(
            delegateTypeData.Id,
            CSharpTypeName.Of(delegateTypeData),
            delegateTypeData.Namespace,
            delegateTypeData.DocComment.Value,
            modifiers.GetStrings(),
            delegateTypeData.ReturnValueDocComment.Value,
            CSharpTypeName.Of(delegateTypeData.ReturnType),
            delegateTypeData.ReturnType.IsVoid,
            parameters,
            typeParameterDeclarations);
    }
}
