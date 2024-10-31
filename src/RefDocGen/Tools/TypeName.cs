using RefDocGen.MemberData.Abstract;
using System.Text;

namespace RefDocGen.Tools;

internal class TypeName
{
    public static string From(ITypeNameData type)
    {
        string name = type.Name;

        if (type.IsGeneric)
        {
            int backTickIndex = name.IndexOf('`');
            string nameWithoutGenericParams = name[..backTickIndex];

            var sb = new StringBuilder(nameWithoutGenericParams);

            return sb.Append('<')
                .Append(string.Join(", ", type.GenericParameters.Select(From)))
                .Append('>')
                .ToString();
        }
        else
        {
            return name;
        }
    }
}

