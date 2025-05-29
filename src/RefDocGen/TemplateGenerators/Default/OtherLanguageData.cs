
#pragma warning disable IDE0005 // add the namespace containing the Razor templates
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements;
using RefDocGen.TemplateGenerators.Shared.Languages;
#pragma warning restore IDE0005

namespace RefDocGen.TemplateGenerators.Default;

internal class OtherLanguageData : ILanguageConfiguration
{
    public string LanguageName => "Other";

    public string LanguageId => "other-lang";

    public string FormatLiteralValue(object? literalValue)
    {
        return "";
    }

    public string[] GetModifiers(IFieldData field)
    {
        return [];
    }

    public PropertyModifiers GetModifiers(IPropertyData property)
    {
        return new([], [], []);
    }

    public string[] GetModifiers(IMethodData method)
    {
        return [];
    }

    public string[] GetModifiers(IConstructorData constructor)
    {
        return [];
    }

    public string[] GetModifiers(IEventData eventData)
    {
        return [];
    }

    public string[] GetModifiers(IParameterData parameter)
    {
        return [];
    }

    public PropertyModifiers GetModifiers(IIndexerData indexer)
    {
        return new([], [], []);
    }

    public string[] GetModifiers(IOperatorData operatorData)
    {
        return [];
    }

    public string[] GetModifiers(IObjectTypeData type)
    {
        return [];
    }

    public string[] GetModifiers(IDelegateTypeData delegateType)
    {
        return [];
    }

    public string[] GetModifiers(IEnumTypeData enumType)
    {
        return [];
    }

    public string[] GetModifiers(ITypeParameterData typeParameter)
    {
        return [];
    }

    public string GetSpecialTypeConstraintName(SpecialTypeConstraint constraint)
    {
        return "";
    }

    public string GetTypeName(ITypeDeclaration type)
    {
        return "";
    }
}
