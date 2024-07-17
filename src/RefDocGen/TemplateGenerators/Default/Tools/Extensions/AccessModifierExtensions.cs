using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Default.Tools.Extensions;

internal static class AccessModifierExtensions
{
    internal static string GetPlaceholderString(this AccessModifier accessModifier)
    {
        var placeholder = accessModifier switch
        {
            AccessModifier.Private => Keyword.Private,
            AccessModifier.Family => Keyword.Protected,
            AccessModifier.Assembly => Keyword.Internal,
            AccessModifier.FamilyAndAssembly => Keyword.PrivateProtected,
            AccessModifier.FamilyOrAssembly => Keyword.ProtectedInternal,
            AccessModifier.Public => Keyword.Public,
            _ => throw new ArgumentException()
        };

        return placeholder.GetString();
    }
}
