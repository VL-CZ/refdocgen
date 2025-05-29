using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Members;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Keywords.CSharp;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.Languages;

/// <summary>
/// Provides syntax-related data the C# language.
/// </summary>
internal class CSharpLanguageConfiguration : ILanguageConfiguration
{
    /// <inheritdoc cref="LanguageId"/>
    public const string languageId = "csharp-lang";

    /// <inheritdoc/>
    public string LanguageName => "C#";

    /// <inheritdoc/>
    public string LanguageId => languageId;

    /// <inheritdoc/>
    public string FormatLiteralValue(object? literalValue)
    {
        return CSharpLiteralValueFormatter.Format(literalValue);
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IFieldData field)
    {
        List<Keyword> modifiers = [field.AccessModifier.ToKeyword()];

        if (field.IsStatic && !field.IsConstant)
        {
            modifiers.Add(Keyword.Static);
        }
        if (field.IsConstant)
        {
            modifiers.Add(Keyword.Const);
        }
        if (field.IsReadonly)
        {
            modifiers.Add(Keyword.Readonly);
        }
        if (field.IsRequired)
        {
            modifiers.Add(Keyword.Required);
        }

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public PropertyModifiers GetModifiers(IPropertyData property)
    {
        var modifiers = GetCallableMemberModifiers(property);

        if (property.IsRequired)
        {
            modifiers.Add(Keyword.Required);
        }

        List<Keyword> getterModifiers = [];
        List<Keyword> setterModifiers = [];

        if (property.Getter is not null && property.Getter.AccessModifier != property.AccessModifier)
        {
            getterModifiers.Add(property.Getter.AccessModifier.ToKeyword());
        }

        if (property.Setter is not null && property.Setter.AccessModifier != property.AccessModifier)
        {
            setterModifiers.Add(property.Setter.AccessModifier.ToKeyword());
        }

        return new(modifiers.GetStrings(), getterModifiers.GetStrings(), setterModifiers.GetStrings());
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IMethodData method)
    {
        return GetCallableMemberModifiers(method).GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IConstructorData constructor)
    {
        List<Keyword> modifiers = [constructor.AccessModifier.ToKeyword()];

        if (constructor.IsStatic)
        {
            modifiers.Add(Keyword.Static);
        }

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IEventData eventData)
    {
        var modifiers = GetCallableMemberModifiers(eventData);
        modifiers.Add(Keyword.Event);

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IParameterData parameter)
    {
        List<Keyword> modifiers = [];

        if (parameter.IsExtensionParameter)
        {
            modifiers.Add(Keyword.This);
        }

        if (parameter.IsInput)
        {
            modifiers.Add(Keyword.In);
        }

        if (parameter.IsOutput)
        {
            modifiers.Add(Keyword.Out);
        }

        if (RefKeyword.IsPresentIn(parameter))
        {
            modifiers.Add(Keyword.Ref);
        }

        if (parameter.IsParamsCollection)
        {
            modifiers.Add(Keyword.Params);
        }

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public PropertyModifiers GetModifiers(IIndexerData indexer)
    {
        return GetModifiers((IPropertyData)indexer);
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IOperatorData operatorData)
    {
        var modifiers = GetCallableMemberModifiers(operatorData);

        if (operatorData.Kind == OperatorKind.ExplicitConversion)
        {
            modifiers.Add(Keyword.Explicit);
        }
        else if (operatorData.Kind == OperatorKind.ImplicitConversion)
        {
            modifiers.Add(Keyword.Implicit);
        }

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IObjectTypeData type)
    {
        List<Keyword> modifiers = [type.AccessModifier.ToKeyword()];

        if (SealedKeyword.IsPresentIn(type))
        {
            modifiers.Add(Keyword.Sealed);
        }

        if (AbstractKeyword.IsPresentIn(type))
        {
            modifiers.Add(Keyword.Abstract);
        }

        if (StaticKeyword.IsPresentIn(type))
        {
            modifiers.Add(Keyword.Static);
        }

        if (type.IsByRefLike)
        {
            modifiers.Add(Keyword.Ref);
        }

        // add 'class', 'struct' or 'interface' modifier
        modifiers.Add(type.Kind.ToKeyword());

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IDelegateTypeData delegateType)
    {
        List<Keyword> modifiers = [delegateType.AccessModifier.ToKeyword(), Keyword.Delegate];
        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string[] GetModifiers(IEnumTypeData enumType)
    {
        List<Keyword> modifiers = [enumType.AccessModifier.ToKeyword(), Keyword.Enum];
        return modifiers.GetStrings();

    }

    /// <inheritdoc/>
    public string[] GetModifiers(ITypeParameterData typeParameter)
    {
        List<Keyword> modifiers = [];

        if (typeParameter.IsCovariant)
        {
            modifiers.Add(Keyword.Out);
        }
        if (typeParameter.IsContravariant)
        {
            modifiers.Add(Keyword.In);
        }

        return modifiers.GetStrings();
    }

    /// <inheritdoc/>
    public string GetOperatorName(IOperatorData operatorData)
    {
        return CSharpOperatorName.Of(operatorData);
    }

    /// <inheritdoc/>
    public string GetSpecialTypeConstraintName(SpecialTypeConstraint constraint)
    {
        return constraint.GetCSharpName();
    }

    /// <inheritdoc/>
    public string GetTypeName(ITypeDeclaration type)
    {
        return CSharpTypeName.Of(type);
    }

    /// <summary>
    /// Gets the list of modifiers for the provided <paramref name="member"/> object.
    /// </summary>
    /// <param name="member">Member, whose modifiers we get.</param>
    /// <returns>A list of modifiers for the provided member.</returns>
    private List<Keyword> GetCallableMemberModifiers(ICallableMemberData member)
    {
        List<Keyword> modifiers = [];

        if (!member.IsExplicitImplementation) // don't add access modifiers for explicitly implemeneted members
        {
            modifiers.Add(member.AccessModifier.ToKeyword());
        }

        if (member.IsStatic)
        {
            modifiers.Add(Keyword.Static);
        }

        if (AbstractKeyword.IsPresentIn(member))
        {
            modifiers.Add(Keyword.Abstract);
        }

        if (VirtualKeyword.IsPresentIn(member))
        {
            modifiers.Add(Keyword.Virtual);
        }

        if (member.OverridesAnotherMember)
        {
            modifiers.Add(Keyword.Override);
        }

        if (member.IsSealed)
        {
            modifiers.Add(Keyword.Sealed);
        }

        if (member.IsAsync)
        {
            modifiers.Add(Keyword.Async);
        }

        return modifiers;
    }
}
