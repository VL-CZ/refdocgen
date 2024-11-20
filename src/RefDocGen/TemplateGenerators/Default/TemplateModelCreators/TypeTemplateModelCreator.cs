using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual types.
/// </summary>
internal static class TypeTemplateModelCreator
{
    /// <summary>
    /// Creates a <see cref="TypeTemplateModel"/> instance based on the provided <see cref="ITypeData"/> object.
    /// </summary>
    /// <param name="typeData">The <see cref="ITypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="TypeTemplateModel"/> instance based on the provided <paramref name="typeData"/>.</returns>
    public static TypeTemplateModel GetFrom(ITypeData typeData)
    {
        var constructors = typeData.Constructors.Select(GetFrom).ToArray();
        var fields = typeData.Fields.Select(GetFrom).ToArray();
        var properties = typeData.Properties.Select(GetFrom).ToArray();
        var methods = typeData.Methods.Select(GetFrom).ToArray();

        List<Keyword> modifiers = [typeData.AccessModifier.ToKeyword()];

        if (SealedKeyword.IsPresentIn(typeData))
        {
            modifiers.Add(Keyword.Sealed);
        }

        if (AbstractKeyword.IsPresentIn(typeData))
        {
            modifiers.Add(Keyword.Abstract);
        }

        return new TypeTemplateModel(
            typeData.Id,
            CSharpTypeName.Of(typeData),
            typeData.DocComment.Value,
            typeData.Kind.GetName(),
            modifiers.GetStrings(),
            constructors,
            fields,
            properties,
            methods);
    }

    /// <summary>
    /// Creates a <see cref="ConstructorTemplateModel"/> instance based on the provided <see cref="IConstructorData"/> object.
    /// </summary>
    /// <param name="constructorData">The <see cref="IConstructorData"/> instance representing the constructor.</param>
    /// <returns>A <see cref="ConstructorTemplateModel"/> instance based on the provided <paramref name="constructorData"/>.</returns>
    private static ConstructorTemplateModel GetFrom(IConstructorData constructorData)
    {
        var modifiers = GetCallableMemberModifiers(constructorData);

        return new ConstructorTemplateModel(constructorData.Parameters.Select(GetFrom).ToArray(),
            constructorData.DocComment.Value, modifiers.GetStrings());
    }

    /// <summary>
    /// Creates a <see cref="FieldTemplateModel"/> instance based on the provided <see cref="IFieldData"/> object.
    /// </summary>
    /// <param name="fieldData">The <see cref="IFieldData"/> instance representing the field.</param>
    /// <returns>A <see cref="FieldTemplateModel"/> instance based on the provided <paramref name="fieldData"/>.</returns>
    private static FieldTemplateModel GetFrom(IFieldData fieldData)
    {
        List<Keyword> modifiers = [fieldData.AccessModifier.ToKeyword()];

        if (fieldData.IsStatic && !fieldData.IsConstant)
        {
            modifiers.Add(Keyword.Static);
        }

        if (fieldData.IsConstant)
        {
            modifiers.Add(Keyword.Const);
        }

        if (fieldData.IsReadonly)
        {
            modifiers.Add(Keyword.Readonly);
        }

        return new FieldTemplateModel(fieldData.Name, CSharpTypeName.Of(fieldData.Type), fieldData.DocComment.Value, modifiers.GetStrings());
    }

    /// <summary>
    /// Creates a <see cref="PropertyTemplateModel"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="propertyData">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTemplateModel"/> instance based on the provided <paramref name="propertyData"/>.</returns>
    private static PropertyTemplateModel GetFrom(IPropertyData propertyData)
    {
        var modifiers = GetCallableMemberModifiers(propertyData);

        List<Keyword> getterModifiers = [];
        List<Keyword> setterModifiers = [];

        if (propertyData.Getter is not null && propertyData.Getter.AccessModifier != propertyData.AccessModifier)
        {
            getterModifiers.Add(propertyData.Getter.AccessModifier.ToKeyword());
        }

        if (propertyData.Setter is not null && propertyData.Setter.AccessModifier != propertyData.AccessModifier)
        {
            setterModifiers.Add(propertyData.Setter.AccessModifier.ToKeyword());
        }

        return new PropertyTemplateModel(propertyData.Name, CSharpTypeName.Of(propertyData.Type), propertyData.DocComment.Value,
            modifiers.GetStrings(), propertyData.Getter is not null, propertyData.Setter is not null, getterModifiers.GetStrings(), setterModifiers.GetStrings());
    }

    /// <summary>
    /// Creates a <see cref="MethodTemplateModel"/> instance based on the provided <see cref="IMethodData"/> object.
    /// </summary>
    /// <param name="methodData">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTemplateModel"/> instance based on the provided <paramref name="methodData"/>.</returns>
    private static MethodTemplateModel GetFrom(IMethodData methodData)
    {
        var modifiers = GetCallableMemberModifiers(methodData);

        return new MethodTemplateModel(
            methodData.Name,
            methodData.Parameters.Select(GetFrom).ToArray(),
            CSharpTypeName.Of(methodData.ReturnType),
            methodData.ReturnType.IsVoid,
            methodData.DocComment.Value,
            methodData.ReturnValueDocComment.Value,
            modifiers.GetStrings());
    }

    /// <summary>
    /// Creates a <see cref="ParameterTemplateModel"/> instance based on the provided <see cref="IParameterData"/> object.
    /// </summary>
    /// <param name="parameterData">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTemplateModel"/> instance based on the provided <paramref name="parameterData"/>.</returns>
    private static ParameterTemplateModel GetFrom(IParameterData parameterData)
    {
        List<Keyword> modifiers = [];

        if (parameterData.IsInput)
        {
            modifiers.Add(Keyword.In);
        }

        if (parameterData.IsOutput)
        {
            modifiers.Add(Keyword.Out);
        }

        if (RefKeyword.IsPresentIn(parameterData))
        {
            modifiers.Add(Keyword.Ref);
        }

        if (parameterData.IsParamsCollection)
        {
            modifiers.Add(Keyword.Params);
        }

        return new ParameterTemplateModel(parameterData.Name, CSharpTypeName.Of(parameterData.Type), parameterData.DocComment.Value,
            modifiers.GetStrings(), parameterData.IsOptional);
    }

    /// <summary>
    /// Gets the list of modifiers for the provided <paramref name="memberData"/> object.
    /// </summary>
    /// <param name="memberData">Member, whose modifiers we get.</param>
    /// <returns>A list of modifiers for the provided member.</returns>
    private static List<Keyword> GetCallableMemberModifiers(ICallableMemberData memberData)
    {
        List<Keyword> modifiers = [memberData.AccessModifier.ToKeyword()];

        if (memberData.IsStatic)
        {
            modifiers.Add(Keyword.Static);
        }

        if (memberData.IsAbstract)
        {
            modifiers.Add(Keyword.Abstract);
        }

        if (VirtualKeyword.IsPresentIn(memberData))
        {
            modifiers.Add(Keyword.Virtual);
        }

        if (memberData.OverridesAnotherMember)
        {
            modifiers.Add(Keyword.Override);
        }

        if (memberData.IsSealed)
        {
            modifiers.Add(Keyword.Sealed);
        }

        if (memberData.IsAsync)
        {
            modifiers.Add(Keyword.Async);
        }

        return modifiers;
    }

}