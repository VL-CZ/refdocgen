using RefDocGen.MemberData.Abstract;
using System.Text;

namespace RefDocGen.TemplateGenerators.Default.Tools.TypeName;

internal class CSharpTypeName
{
    public static string From(ITypeNameData type)
    {
        string name = BuiltInTypeName.Get(type);

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

