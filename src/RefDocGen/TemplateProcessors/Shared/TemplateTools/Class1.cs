using RefDocGen.TemplateProcessors.Default.Templates.Components.LanguageSpecific;

namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

class LanguageFragmentType
{
    public static Type Get(string folderName, LanguageSpecificComponent specificComponent)
    {
        var ns = typeof(LanguageFragments).Namespace;
        var componentName = specificComponent.ToString();

        var fullName = $"{ns}.{folderName}.{folderName}{componentName}";

        return Type.GetType(fullName) ?? throw new Exception(fullName);
    }
}
