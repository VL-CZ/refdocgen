using RefDocGen.CodeElements.Members;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;
using RefDocGen.TemplateGenerators.Shared.Tools.Keywords;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared;

public enum Language { CSharp }

internal readonly record struct PropertyModifiers(string[] Modifiers, string[] GetterModifiers, string[] SetterModifiers);

internal interface ILanguageSpecificData
{
    string[] GetModifiers(IFieldData field);
    PropertyModifiers GetModifiers(IPropertyData property);
    string[] GetModifiers(IMethodData method);
    string[] GetModifiers(IConstructorData constructor);
    string[] GetModifiers(IEventData eventData);
    string[] GetModifiers(IParameterData parameter);
    PropertyModifiers GetModifiers(IIndexerData indexer);
    string[] GetModifiers(IOperatorData operatorData);
    string[] GetModifiers(IObjectTypeData type);
    string[] GetModifiers(IDelegateTypeData delegateType);
    string[] GetModifiers(IEnumTypeData enumType);
    string[] GetModifiers(ITypeParameterData typeParameter);
    string GetTypeName(ITypeDeclaration type);

}

internal class CSharpLanguageData : ILanguageSpecificData
{
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

    public string[] GetModifiers(IMethodData method)
    {
        return GetCallableMemberModifiers(method).GetStrings();
    }

    public string[] GetModifiers(IConstructorData constructor)
    {
        List<Keyword> modifiers = [constructor.AccessModifier.ToKeyword()];

        if (constructor.IsStatic)
        {
            modifiers.Add(Keyword.Static);
        }

        return modifiers.GetStrings();
    }

    public string[] GetModifiers(IEventData eventData)
    {
        var modifiers = GetCallableMemberModifiers(eventData);
        modifiers.Add(Keyword.Event);

        return modifiers.GetStrings();
    }

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

    public PropertyModifiers GetModifiers(IIndexerData indexer)
    {
        return GetModifiers((IPropertyData)indexer);
    }

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

    public string[] GetModifiers(IDelegateTypeData delegateType)
    {
        List<Keyword> modifiers = [delegateType.AccessModifier.ToKeyword(), Keyword.Delegate];
        return modifiers.GetStrings();
    }

    public string[] GetModifiers(IEnumTypeData enumType)
    {
        List<Keyword> modifiers = [enumType.AccessModifier.ToKeyword(), Keyword.Enum];
        return modifiers.GetStrings();

    }

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

    public string GetTypeName(ITypeDeclaration type)
    {
        return CSharpTypeName.Of(type); // TODO
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

public class LocalizedData<T>
{
    private Dictionary<Language, T> data = new();

    public LocalizedData(Dictionary<Language, T> data)
    {
        this.data = data;
    }

    public T this[Language language] => data[language];
}
