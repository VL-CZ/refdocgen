using RefDocGen.MemberData.Abstract;
using RefDocGen.MemberData.Concrete;

namespace RefDocGen.MemberData;

internal static class TypeExtensions
{
    internal static ITypeNameData ToITypeNameData(this Type type, int index = 0)
    {
        return type.IsGenericTypeParameter
            ? new TypeParameterNameData(type, index)
            : new TypeNameData(type);
    }
}
