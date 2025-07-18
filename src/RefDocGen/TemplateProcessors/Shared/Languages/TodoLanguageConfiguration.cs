using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Languages;

/// <summary>
/// This class shows an example empty configuration of a 'TODO' language.
/// </summary>
internal class TodoLanguageConfiguration : ILanguageConfiguration
{
    /// <inheritdoc />
    public string LanguageName => "TODO";

    /// <inheritdoc />
    public string LanguageId => "todo-lang";

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

