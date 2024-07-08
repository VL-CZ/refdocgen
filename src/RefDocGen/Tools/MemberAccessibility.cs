using RefDocGen.Intermed;

namespace RefDocGen.Tools;

internal record MemberAccessibility(bool IsPrivate, bool IsFamily, bool IsAssembly, bool IsPublic, bool IsFamilyAndAssembly, bool IsFamilyOrAssembly)
{
    internal AccessModifier GetAccessModifier()
    {
        return (IsPrivate, IsFamily, IsAssembly, IsFamilyAndAssembly, IsFamilyOrAssembly) switch
        {
            (true, _, _, _, _) => AccessModifier.Private,
            (_, true, _, _, _) => AccessModifier.Protected,
            (_, _, true, _, _) => AccessModifier.Internal,
            (_, _, _, true, _) => AccessModifier.PrivateProtected, // C# private protected
            (_, _, _, _, true) => AccessModifier.ProtectedInternal, // C# protected internal
            _ => AccessModifier.Public
        };
    }
}
