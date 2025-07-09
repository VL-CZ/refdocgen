namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

/// <summary>
/// Contains additional checks used in templates.
/// </summary>
public static class Checks
{
    /// <summary>
    /// Determines whether at least one of the provided values is not <c>null</c>.
    /// </summary>
    /// <param name="values">A list of values to check.</param>
    /// <returns><c>true</c> if at least one value is not <c>null</c>; otherwise, <c>false</c>.</returns>
    public static bool AnyNotNull(params object?[] values)
    {
        return values.Any(val => val is not null);
    }

    /// <summary>
    /// Determines whether any of the enumerables contains at least one element.
    /// </summary>
    /// <param name="enumerables">An array of enumerables to check.</param>
    /// <returns><c>true</c> if at least one enumerable contains elements; otherwise, <c>false</c>.</returns>
    public static bool AnyNonEmpty(params IEnumerable<object>[] enumerables)
    {
        return enumerables.Any(en => en.Any());
    }
}
