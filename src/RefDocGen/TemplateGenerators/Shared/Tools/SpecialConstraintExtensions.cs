using RefDocGen.CodeElements;

namespace RefDocGen.TemplateGenerators.Shared.Tools;

/// <summary>
/// Class containing extension methods for <see cref="SpecialTypeConstraint"/> enum.
/// </summary>
internal static class SpecialConstraintExtensions
{
    /// <summary>
    /// Get C# name of the special constraint.
    /// </summary>
    /// <param name="specialConstraint">The selected special constraint.</param>
    /// <returns>C# name of the constraint.</returns>
    internal static string GetCSharpName(this SpecialTypeConstraint specialConstraint)
    {
        return specialConstraint switch
        {
            SpecialTypeConstraint.DefaultConstructor => "new()",
            SpecialTypeConstraint.NotNullableValueType => "struct",
            SpecialTypeConstraint.ReferenceType => "class",
            _ => throw new ArgumentException("") // TODO: add message
        };
    }
}
