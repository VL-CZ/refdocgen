using RefDocGen.MemberData.Abstract;

namespace RefDocGen.MemberData.Concrete;

internal class TypeNameData : ITypeNameData
{
    public TypeNameData(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public string ShortName
    {
        get
        {
            string name = Type.Name;

            // remove the backtick and number of generic arguments (if present)
            int backTickIndex = name.IndexOf('`');
            if (backTickIndex >= 0)
            {
                name = name[..backTickIndex];
            }

            // remove the reference suffix (if present)
            if (name.EndsWith('&'))
            {
                name = name[..^1];
            }

            return name;
        }
    }

    public string FullName => Type.Namespace is not null ? $"{Type.Namespace}.{ShortName}" : ShortName;

    public string Id
    {
        get
        {
            string name = FullName;

            if (IsGeneric)
            {
                name = name + '{' + string.Join(",", GenericParameters.Select(p => p.Id)) + '}';
            }

            return name.Replace('&', '@');
        }
    }

    public bool IsGeneric => Type.IsGenericType;

    public string? Namespace => Type.Namespace;

    public IReadOnlyList<ITypeNameData> GenericParameters => Type.GetGenericArguments().Select(t => new TypeNameData(t)).ToArray();

}
