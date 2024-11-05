using RefDocGen.MemberData.Abstract;
using System.Text;

namespace RefDocGen.TemplateGenerators.Default.Tools;

internal class TypeName
{
    public static string From(ITypeNameData type)
    {
        string name = type.ShortName;

        if (type.HasGenericParameters)
        {
            var sb = new StringBuilder(name);

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

