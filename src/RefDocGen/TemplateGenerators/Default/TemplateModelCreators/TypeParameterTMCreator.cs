using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual generic type parameter declarations.
/// </summary>
internal class TypeParameterTMCreator
{
    /// <summary>
    /// Creates a <see cref="TypeParameterTM"/> instance based on the provided <see cref="ITypeParameterData"/> object.
    /// </summary>
    /// <param name="typeParameter">The <see cref="ITypeParameterData"/> instance representing the type parameter.</param>
    /// <returns>A <see cref="TypeParameterTM"/> instance based on the provided <paramref name="typeParameter"/>.</returns>
    internal static TypeParameterTM GetFrom(ITypeParameterData typeParameter)
    {
        List<Keyword> modifiers = [];

        if (typeParameter.IsCovariant)
        {
            modifiers.Add(Keyword.Out);
        }
        if (typeParameter.IsContravariant)
        {
            modifiers.Add(Keyword.In);
        }

        var constraints = typeParameter.Constraints.Select(CSharpTypeName.Of);

        return new TypeParameterTM(typeParameter.Name, typeParameter.DocComment.Value, modifiers.GetStrings(), constraints);
    }
}
