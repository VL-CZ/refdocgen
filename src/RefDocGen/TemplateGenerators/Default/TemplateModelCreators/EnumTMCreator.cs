using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

internal static class EnumTMCreator
{
    internal static EnumTM GetFrom(IEnumData enumData)
    {
        var valueTMs = enumData.Values.Select(GetFrom);
        return new EnumTM(enumData.Id, enumData.ShortName, enumData.Namespace, enumData.DocComment.Value, [], valueTMs);
    }

    internal static EnumValueTM GetFrom(IEnumValueData enumValue)
    {
        return new EnumValueTM(enumValue.Name, enumValue.DocComment.Value);
    }
}
