namespace RefDocGen.Intermed;

public record ClassIntermed(string Name, AccessModifier AccessibilityModifier, FieldIntermed[] Fields, PropertyIntermed[] Properties, MethodIntermed[] Methods)
{
}
