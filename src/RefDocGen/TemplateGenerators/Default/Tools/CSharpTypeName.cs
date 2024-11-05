using RefDocGen.MemberData.Abstract;
using System.Text;

namespace RefDocGen.TemplateGenerators.Default.Tools;

internal class CSharpTypeName
{
    public static string From(ITypeNameData type, string genericParamBegin = "<", string genericParamEnd = ">", string genericParamDelimiter = ", ")
    {
        string name = BuiltInTypeName.Get(type);

        if (type.HasGenericParameters)
        {
            var sb = new StringBuilder(name);

            return sb.Append(genericParamBegin)
                .Append(string.Join(genericParamDelimiter, type.GenericParameters.Select(p => From(p, genericParamBegin, genericParamEnd, genericParamDelimiter))))
                .Append(genericParamEnd)
                .ToString();
        }
        else
        {
            return name;
        }
    }
}

