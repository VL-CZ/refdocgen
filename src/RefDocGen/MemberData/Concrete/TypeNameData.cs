using RefDocGen.MemberData.Abstract;
using System.Text;

namespace RefDocGen.MemberData.Concrete;

internal class TypeNameData : ITypeNameData
{
    public TypeNameData(Type type)
    {
        Type = type;
    }

    public Type Type { get; }

    public string Name => Type.Name;

    public string FullName => Type.FullName ?? Name;

    public string Id
    {
        get
        {
            string name = $"{Type.Namespace}.{Name}";

            if (IsGeneric)
            {
                int backTickIndex = name.IndexOf('`');
                string nameWithoutGenericParams = name[..backTickIndex];

                var sb = new StringBuilder(nameWithoutGenericParams);

                return sb.Append('{')
                    .Append(string.Join(", ", GenericParameters.Select(p => p.Id)))
                    .Append('}')
                    .Replace('&', '@')
                    .ToString();
            }
            else
            {
                return name.Replace('&', '@');
            }
        }
    }

    public bool IsGeneric => Type.IsGenericType;

    public IReadOnlyList<ITypeNameData> GenericParameters => Type.GetGenericArguments().Select(t => new TypeNameData(t)).ToArray();
}
