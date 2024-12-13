using RefDocGen.CodeElements;

namespace RefDocGen.TemplateGenerators.Tools;

internal static class SpecialConstraintExtensions
{
    internal static string GetName(this SpecialConstraint specialConstraint)
    {
        return specialConstraint switch
        {
            SpecialConstraint.DefaultConstructor => "new()",
            SpecialConstraint.NotNullableValueType => "struct",
            SpecialConstraint.ReferenceType => "class",
            _ => throw new ArgumentException() // TODO
        };
    }
}
