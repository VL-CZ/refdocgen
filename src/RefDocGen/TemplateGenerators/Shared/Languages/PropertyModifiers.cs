namespace RefDocGen.TemplateGenerators.Shared.Languages;

/// <summary>
/// Represents a set of property modifiers.
/// </summary>
/// <param name="Modifiers">Modifiers applied to the property.</param>
/// <param name="GetterModifiers">Modifiers applied to the property getter.</param>
/// <param name="SetterModifiers">Modifiers applied to the property getter.</param>
internal readonly record struct PropertyModifiers(string[] Modifiers, string[] GetterModifiers, string[] SetterModifiers);
