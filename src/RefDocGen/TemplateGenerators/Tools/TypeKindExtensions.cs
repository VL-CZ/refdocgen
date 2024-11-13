using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators.Tools;

public static class TypeKindExtensions
{
    public static string GetName(this TypeKind typeKind)
    {
        return typeKind switch
        {
            TypeKind.ValueType => "struct",
            TypeKind.Interface => "interface",
            _ => "class"
        };
    }
}
