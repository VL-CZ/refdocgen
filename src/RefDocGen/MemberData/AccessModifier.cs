namespace RefDocGen.MemberData;

public enum AccessModifier { Private, PrivateProtected, Protected, Internal, ProtectedInternal, Public } // sorted from the MOST restrictive

internal static class AccessModifierExtensions
{
    internal static string GetString(this AccessModifier accessModifier)
    {
        return accessModifier switch
        {
            AccessModifier.PrivateProtected => "private protected",
            AccessModifier.ProtectedInternal => "internal protected",
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
            (_, true, _, _, _, _) => AccessModifier.Protected,
            (_, _, true, _, _, _) => AccessModifier.Internal,
            (_, _, _, true, _, _) => AccessModifier.PrivateProtected, // C# private protected
            (_, _, _, _, true, _) => AccessModifier.ProtectedInternal, // C# protected internal
            (_, _, _, _, _, true) => AccessModifier.Public,
            _ => throw new ArgumentException() // TODO: custom exception
        };
    }
}
