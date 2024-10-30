using System.Text;

namespace RefDocGen.Tools;

internal class TypeName
{
    public static string From(Type type)
    {
        var sb = new StringBuilder();
        string name = type.Name;

        if (!type.IsGenericType)
        {
            return name;
        }

        sb.Append(name[..name.IndexOf('`')]);
        sb.Append('<');
        sb.Append(string.Join(", ", type.GetGenericArguments().Select(From)));
        sb.Append('>');
        return sb.ToString();
    }
}

