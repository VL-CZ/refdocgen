using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Types;


internal class EnumData : IEnumData
{
    public EnumData(Type typeObject, IReadOnlyDictionary<string, EnumValueData> values)
    {
        TypeObject = typeObject;
        Values = values;
    }

    /// <inheritdoc/>
    public Type TypeObject { get; }

    /// <inheritdoc/>
    public string Id => FullName;

    /// <inheritdoc/>
    public string ShortName => TypeObject.Name;

    /// <inheritdoc/>
    public string FullName => TypeObject.Namespace is not null ? $"{TypeObject.Namespace}.{ShortName}" : ShortName;

    /// <inheritdoc/>
    public string Namespace => TypeObject.Namespace ?? string.Empty;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;

    public IReadOnlyDictionary<string, EnumValueData> Values { get; }

    /// <inheritdoc/>
    IReadOnlyList<IEnumValueData> IEnumData.Values => Values.Values.ToList();
}
