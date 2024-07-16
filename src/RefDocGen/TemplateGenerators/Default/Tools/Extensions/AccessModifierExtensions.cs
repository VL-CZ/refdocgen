using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

internal static class AccessModifierExtensions
{
    internal static string GetPlaceholderString(this AccessModifier accessModifier)
    {
        var placeholder = accessModifier switch
        {
            AccessModifier.Private => Placeholder.Private,
            AccessModifier.Family => Placeholder.Protected,
            AccessModifier.Assembly => Placeholder.Internal,
            AccessModifier.FamilyAndAssembly => Placeholder.PrivateProtected,
            AccessModifier.FamilyOrAssembly => Placeholder.ProtectedInternal,
            AccessModifier.Public => Placeholder.Public,
            _ => throw new ArgumentException()
        };

        return placeholder.GetString();
    }
}
