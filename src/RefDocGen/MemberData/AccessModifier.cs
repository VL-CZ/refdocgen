namespace RefDocGen.MemberData;

public enum AccessModifier { Private, FamilyAndAssembly, Family, Assembly, FamilyOrAssembly, Public } // sorted from the MOST restrictive

internal static class AccessModifierExtensions
{
    internal static string GetString(this AccessModifier accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.Family => "protected",
            AccessModifier.Assembly => "internal",
            AccessModifier.FamilyAndAssembly => "private protected",
            AccessModifier.FamilyOrAssembly => "internal protected",
            _ => accessModifier.ToString().ToLowerInvariant()
        };
    }

    internal static AccessModifier GetTheLeastRestrictive(IEnumerable<AccessModifier> accessModifiers)
    {
        int minIntegerValue = accessModifiers.Max(a => (int)a);
        return (AccessModifier)minIntegerValue;
    }

    internal static AccessModifier GetAccessModifier(bool isPrivate, bool isFamily, bool isAssembly, bool isPublic, bool isFamilyAndAssembly, bool isFamilyOrAssembly)
    {
        return (isPrivate, isFamily, isAssembly, isFamilyAndAssembly, isFamilyOrAssembly, isPublic) switch
        {
            (true, _, _, _, _, _) => AccessModifier.Private,
            (_, true, _, _, _, _) => AccessModifier.Family, // C# protected
            (_, _, true, _, _, _) => AccessModifier.Assembly, // C# internal
            (_, _, _, true, _, _) => AccessModifier.FamilyAndAssembly, // C# private protected
            (_, _, _, _, true, _) => AccessModifier.FamilyOrAssembly, // C# protected internal
            (_, _, _, _, _, true) => AccessModifier.Public,
            _ => throw new ArgumentException() // TODO: custom exception
        };
    }
}
