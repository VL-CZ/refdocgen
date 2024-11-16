
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.Tools;

namespace RefDocGen.CodeElements;

internal static class TypeExtensions
{
    internal static ITypeNameData ToITypeNameData(this Type type, IReadOnlyDictionary<string, TypeParameterDeclaration> declaredTypeParameters)
    {
        return type.IsGenericParameter || type.GetBaseElementType().IsGenericParameter
            ? new GenericTypeParameterNameData(type, declaredTypeParameters)
            : new TypeNameData(type, declaredTypeParameters);
    }
}
