using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateProcessors.Shared.Languages;

namespace RefDocGen;

/// <summary>
/// This class shows an example empty configuration of an 'other' language.
/// </summary>
internal class OtherLanguageConfiguration : ILanguageConfiguration
{
    /// <inheritdoc />
    public string LanguageName => "Other";

    /// <inheritdoc />
    public string LanguageId => "other-lang";

    /// <inheritdoc />
    public string ComponentsFolderName => "Todo";

    /// <inheritdoc />
    public string FormatLiteralValue(object? literalValue)
    {
        return "";
    }

    /// <inheritdoc />
    public string[] GetModifiers(IFieldData field)
    {
        return [];
    }

    /// <inheritdoc />
    public PropertyModifiers GetModifiers(IPropertyData property)
    {
        return new([], [], []);
    }

    /// <inheritdoc />
    public string[] GetModifiers(IMethodData method)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IConstructorData constructor)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IEventData eventData)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IParameterData parameter)
    {
        return [];
    }

    /// <inheritdoc />
    public PropertyModifiers GetModifiers(IIndexerData indexer)
    {
        return new([], [], []);
    }

    /// <inheritdoc />
    public string[] GetModifiers(IOperatorData operatorData)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IObjectTypeData type)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IDelegateTypeData delegateType)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(IEnumTypeData enumType)
    {
        return [];
    }

    /// <inheritdoc />
    public string[] GetModifiers(ITypeParameterData typeParameter)
    {
        return [];
    }

    /// <inheritdoc />
    public string GetOperatorName(IOperatorData operatorData)
    {
        return "";
    }

    /// <inheritdoc />
    public string GetSpecialTypeConstraintName(SpecialTypeConstraint constraint)
    {
        return "";
    }

    /// <inheritdoc />
    public string GetTypeName(ITypeNameData type, bool includeTypeParameters = true)
    {
        return "";
    }
}

