using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

internal class EnumValueData : IEnumValueData
{
    public EnumValueData(FieldInfo fieldInfo)
    {
        FieldInfo = fieldInfo;
    }

    public FieldInfo FieldInfo { get; }

    public string Id => Name;

    public string Name => FieldInfo.Name;

    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;
}
