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
        List<string> modifiers = [field.AccessibilityModifier.ToString()];

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
        string[] modifiers = [propertyIntermed.AccessibilityModifier.ToString()];
        return new PropertyTemplateModel(propertyIntermed.Name, propertyIntermed.Type, string.Empty, modifiers, propertyIntermed.Getter is not null, propertyIntermed.Setter is not null, [], []);
    }

    private MethodTemplateModel CreateMethodTemplateModel(MethodIntermed methodIntermed)
    {
        List<string> modifiers = [methodIntermed.AccessibilityModifier.ToString()];

        if (methodIntermed.IsStatic)
        {
            modifiers.Add(Placeholder.Static.GetString());
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
            methodIntermed.Parameters.Select(p => new MethodParameterModel(p.Name, p.Type)).ToArray(),
            methodIntermed.ReturnType, string.Empty, [.. modifiers]);
    }
}
