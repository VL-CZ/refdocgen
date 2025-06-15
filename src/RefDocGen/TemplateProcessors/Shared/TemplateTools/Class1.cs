using RefDocGen.TemplateProcessors.Default.Templates.Components.LanguageSpecific;

namespace RefDocGen.TemplateProcessors.Shared.TemplateTools;

class LanguageFragmentType
{
    public static Type Get(string folderName, LanguageSpecificComponent specificComponent)
    {
        var ns = typeof(LanguageFragments).Namespace;
        var componentName = specificComponent.ToString();

        return Type.GetType($"{ns}.{folderName}.{componentName}") ?? throw new Exception("");
    }
}
