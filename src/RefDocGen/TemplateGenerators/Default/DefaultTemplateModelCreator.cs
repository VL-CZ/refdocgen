using RefDocGen.MemberData.Abstract;
using RefDocGen.TemplateGenerators.Default.TemplateModels;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default;

/// <summary>
/// Class responsible for creating template models for the default template generator.
/// </summary>
internal static class DefaultTemplateModelCreator
{
    /// <summary>
    /// Transforms the provided <see cref="ITypeData"/> instance into a corresponding <see cref="TypeTemplateModel"/>.
    /// </summary>
    /// <param name="typeData">The <see cref="ITypeData"/> instance representing the class.</param>
    /// <returns>A <see cref="TypeTemplateModel"/> instance based on the provided <paramref name="typeData"/>.</returns>
    public static TypeTemplateModel TransformToTemplateModel(ITypeData typeData)
    {
        var constructors = typeData.Constructors.Select(TransformToTemplateModel).ToArray();
        var fields = typeData.Fields.Select(TransformToTemplateModel).ToArray();
        var properties = typeData.Properties.Select(TransformToTemplateModel).ToArray();
        var methods = typeData.Methods.Select(TransformToTemplateModel).ToArray();

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
    /// Transforms the provided <see cref="IConstructorData"/> instance into a corresponding <see cref="ConstructorTemplateModel"/>.
    /// </summary>
    /// <param name="constructorData">The <see cref="IConstructorData"/> instance representing the constructor.</param>
    /// <returns>A <see cref="ConstructorTemplateModel"/> instance based on the provided <paramref name="constructorData"/>.</returns>
    private static ConstructorTemplateModel TransformToTemplateModel(IConstructorData constructorData)
    {
        var modifiers = GetCallableMemberModifiers(constructorData);

        return new ConstructorTemplateModel(constructorData.Parameters.Select(TransformToTemplateModel).ToArray(),
            constructorData.DocComment.Value, modifiers.GetStrings());
    }

    /// <summary>
    /// Transforms the provided <see cref="IFieldData"/> instance into a corresponding <see cref="FieldTemplateModel"/>.
    /// </summary>
    /// <param name="fieldData">The <see cref="IFieldData"/> instance representing the field.</param>
    /// <returns>A <see cref="FieldTemplateModel"/> instance based on the provided <paramref name="fieldData"/>.</returns>
    private static FieldTemplateModel TransformToTemplateModel(IFieldData fieldData)
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
    /// Transforms the provided <see cref="IPropertyData"/> instance into a corresponding <see cref="PropertyTemplateModel"/>.
    /// </summary>
    /// <param name="propertyData">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTemplateModel"/> instance based on the provided <paramref name="propertyData"/>.</returns>
    private static PropertyTemplateModel TransformToTemplateModel(IPropertyData propertyData)
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
    /// Transforms the provided <see cref="IMethodData"/> instance into a corresponding <see cref="MethodTemplateModel"/>.
    /// </summary>
    /// <param name="methodData">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTemplateModel"/> instance based on the provided <paramref name="methodData"/>.</returns>
    private static MethodTemplateModel TransformToTemplateModel(IMethodData methodData)
    {
        var modifiers = GetCallableMemberModifiers(methodData);

        return new MethodTemplateModel(
            methodData.Name,
            methodData.Parameters.Select(TransformToTemplateModel).ToArray(),
            CSharpTypeName.Of(methodData.ReturnType),
            methodData.ReturnType.IsVoid,
            methodData.DocComment.Value,
            methodData.ReturnValueDocComment.Value,
            modifiers.GetStrings());
    }

    /// <summary>
    /// Transforms the provided <see cref="IParameterData"/> instance into a corresponding <see cref="ParameterTemplateModel"/>.
    /// </summary>
    /// <param name="parameterData">The <see cref="IParameterData"/> instance representing the parameter.</param>
    /// <returns>A <see cref="ParameterTemplateModel"/> instance based on the provided <paramref name="parameterData"/>.</returns>
    private static ParameterTemplateModel TransformToTemplateModel(IParameterData parameterData)
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
    /// Get modifiers for the provided <paramref name="memberData"/> object.
    /// </summary>
    /// <param name="memberData">Member, whose modifiers we get.</param>
    /// <returns>List of modifiers for the provided member.</returns>
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
