using RefDocGen.Intermed;
using RefDocGen.Tools;

namespace RefDocGen.TemplateModels.Builders;

internal class CSharpTemplateModelBuilder : ITemplateModelBuilder
{
    public ClassTemplateModel CreateClassTemplateModel(ClassIntermed classIntermed)
    {
        var fields = classIntermed.Fields.Select(CreateFieldTemplateModel).ToArray();
        var properties = classIntermed.Properties.Select(CreatePropertyTemplateModel).ToArray();
        var methods = classIntermed.Methods.Select(CreateMethodTemplateModel).ToArray();

        return new ClassTemplateModel(classIntermed.Name, string.Empty, [classIntermed.AccessibilityModifier.ToString()], fields, properties, methods);
    }

    private FieldTemplateModel CreateFieldTemplateModel(FieldIntermed field)
    {
        List<string> modifiers = [field.AccessibilityModifier.GetString()];

        if (field.IsStatic && !field.IsConstant)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (field.IsConstant)
        {
            modifiers.Add(Placeholder.Const.GetString());
        }
        if (field.IsReadonly)
        {
            modifiers.Add(Placeholder.Readonly.GetString());
        }

        return new FieldTemplateModel(field.Name, field.Type, string.Empty, [.. modifiers]);
    }

    private PropertyTemplateModel CreatePropertyTemplateModel(PropertyIntermed propertyIntermed)
    {
        List<string> modifiers = [propertyIntermed.AccessModifier.GetString()];

        if (propertyIntermed.IsStatic)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (propertyIntermed.HasVirtualKeyword)
        {
            modifiers.Add(Placeholder.Virtual.GetString());
        }
        if (propertyIntermed.OverridesAnotherProperty)
        {
            modifiers.Add(Placeholder.Override.GetString());
        }
        if (propertyIntermed.IsSealed)
        {
            modifiers.Add(Placeholder.Sealed.GetString());
        }

        List<string> getterModifiers = [];
        List<string> setterModifiers = [];

        if (propertyIntermed.Getter is not null && propertyIntermed.Getter.AccessModifier != propertyIntermed.AccessModifier)
        {
            getterModifiers.Add(propertyIntermed.Getter.AccessModifier.GetString());
        }
        if (propertyIntermed.Setter is not null && propertyIntermed.Setter.AccessModifier != propertyIntermed.AccessModifier)
        {
            setterModifiers.Add(propertyIntermed.Setter.AccessModifier.GetString());
        }

        return new PropertyTemplateModel(propertyIntermed.Name, propertyIntermed.Type, string.Empty, [.. modifiers], propertyIntermed.Getter is not null, propertyIntermed.Setter is not null, [.. getterModifiers], [.. setterModifiers]);
    }

    private MethodTemplateModel CreateMethodTemplateModel(MethodIntermed methodIntermed)
    {
        List<string> modifiers = [methodIntermed.AccessModifier.GetString()];

        if (methodIntermed.IsStatic)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (methodIntermed.IsAbstract)
        {
            modifiers.Add(Placeholder.Abstract.GetString());
        }
        if (methodIntermed.HasVirtualKeyword)
        {
            modifiers.Add(Placeholder.Virtual.GetString());
        }
        if (methodIntermed.OverridesAnotherMethod)
        {
            modifiers.Add(Placeholder.Override.GetString());
        }
        if (methodIntermed.IsSealed)
        {
            modifiers.Add(Placeholder.Sealed.GetString());
        }
        if (methodIntermed.IsAsync)
        {
            modifiers.Add(Placeholder.Async.GetString());
        }

        return new MethodTemplateModel(methodIntermed.Name,
            methodIntermed.GetParameters().Select(CreateMethodParameterModel).ToArray(),
            methodIntermed.ReturnType, string.Empty, [.. modifiers]);
    }

    private MethodParameterModel CreateMethodParameterModel(MethodParameter methodParameter)
    {
        var modifiers = new List<string>();

        if (methodParameter.IsInput)
        {
            modifiers.Add(Placeholder.In.GetString());
        }
        if (methodParameter.IsOutput)
        {
            modifiers.Add(Placeholder.Out.GetString());
        }
        if (methodParameter.HasRefKeyword)
        {
            modifiers.Add(Placeholder.Ref.GetString());
        }
        if (methodParameter.IsParamsArray)
        {
            modifiers.Add(Placeholder.Params.GetString());
        }
        if (methodParameter.IsOptional)
        {
            modifiers.Add("optional"); // TODO: add default value
        }

        return new MethodParameterModel(methodParameter.Name, methodParameter.Type, modifiers);
    }
}
