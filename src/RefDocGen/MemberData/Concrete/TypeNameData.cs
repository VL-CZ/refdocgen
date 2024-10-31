using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData.Concrete;

internal class TypeNameData : ITypeNameData
{
    public TypeNameData(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public string Name => Type.Name;

    public string FullName => Type.FullName;

    public bool IsGeneric => Type.IsGenericType;

    public IReadOnlyList<ITypeNameData> GenericParameters => Type.GetGenericArguments().Select(t => new TypeNameData(t)).ToArray();
}
