using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.CodeElements.Types.Abstract.TypeName;

namespace RefDocGen.TemplateProcessors.Shared.Languages;

/// <summary>
/// A configuration containing selected syntax-related data of a programming language.
/// </summary>
internal interface ILanguageConfiguration
{
    /// <summary>
    /// Gets the modifiers for the given <paramref name="field"/>.
    /// </summary>
    /// <param name="field">The field, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the field.</returns>
    string[] GetModifiers(IFieldData field);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="property"/>.
    /// </summary>
    /// <param name="property">The property, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the property.</returns>
    PropertyModifiers GetModifiers(IPropertyData property);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="method"/>.
    /// </summary>
    /// <param name="method">The method, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the method.</returns>
    string[] GetModifiers(IMethodData method);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="constructor"/>.
    /// </summary>
    /// <param name="constructor">The constructor, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the constructor.</returns>
    string[] GetModifiers(IConstructorData constructor);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="eventData"/>.
    /// </summary>
    /// <param name="eventData">The event, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the event.</returns>
    string[] GetModifiers(IEventData eventData);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="parameter"/>.
    /// </summary>
    /// <param name="parameter">The parameter, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the parameter.</returns>
    string[] GetModifiers(IParameterData parameter);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="indexer"/>.
    /// </summary>
    /// <param name="indexer">The indexer, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the indexer.</returns>
    PropertyModifiers GetModifiers(IIndexerData indexer);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="operatorData"/>.
    /// </summary>
    /// <param name="operatorData">The operator, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the operator.</returns>
    string[] GetModifiers(IOperatorData operatorData);

    /// <summary>
    /// Gets the modifiers for the given object type.
    /// </summary>
    /// <param name="type">The object type, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the object type.</returns>
    string[] GetModifiers(IObjectTypeData type);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="delegateType"/>.
    /// </summary>
    /// <param name="delegateType">The delegate type, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the delegate type.</returns>
    string[] GetModifiers(IDelegateTypeData delegateType);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="enumType"/>.
    /// </summary>
    /// <param name="enumType">The enum type, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the enum type.</returns>
    string[] GetModifiers(IEnumTypeData enumType);

    /// <summary>
    /// Gets the modifiers for the given <paramref name="typeParameter"/>.
    /// </summary>
    /// <param name="typeParameter">The type parameter, whose modifiers are returned.</param>
    /// <returns>The modifiers applied to the type parameter.</returns>
    string[] GetModifiers(ITypeParameterData typeParameter);

    /// <summary>
    /// Gets the type name of the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type, whose name is returned.</param>
    /// <param name="includeTypeParameters">Specifies whether the type paramters should be included in the type name.</param>
    /// <returns>The name of the type. No modifiers should be included in the name.</returns>
    string GetTypeName(ITypeNameData type, bool includeTypeParameters = true);

    /// <summary>
    /// Formats the given <paramref name="literalValue"/> as a language-specific literal.
    /// </summary>
    /// <param name="literalValue">The literal value, which is formatted.</param>
    /// <returns>The formatted literal value.</returns>
    string FormatLiteralValue(object? literalValue);

    /// <summary>
    /// Gets the name of the given special type constraint in the target language.
    /// </summary>
    /// <param name="constraint">The special type constraint, whose name is returned.</param>
    /// <returns>The name of the constraint.</returns>
    string GetSpecialTypeConstraintName(SpecialTypeConstraint constraint);

    /// <summary>
    /// Gets name of the operator.
    /// </summary>
    /// <param name="operatorData">The operator, whose name is returned.</param>
    /// <returns>Name of the operator. No modifiers or parameters should be included.</returns>
    string GetOperatorName(IOperatorData operatorData);

    /// <summary>
    /// The language name to be displayed in the documentation.
    /// </summary>
    string LanguageName { get; }

    /// <summary>
    /// A unique identifier of the language.
    /// </summary>
    string LanguageId { get; }

    /// <summary>
    /// Name of the folder inside the <c>TemplateProcessors/Default/Templates/Components/LanguageSpecific</c> directory that contains the language-specific components.
    /// </summary>
    string ComponentsFolderName { get; }
}
