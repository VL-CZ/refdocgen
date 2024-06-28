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

    private FieldTemplateModel CreateFieldTemplateModel(FieldIntermed fieldIntermed)
    {
        List<string> modifiers = [fieldIntermed.AccessibilityModifier.ToString()];

        if (fieldIntermed.IsStatic)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (fieldIntermed.IsConst)
        {
            modifiers.Add(Placeholder.Const.GetString());
        }
        if (fieldIntermed.IsReadonly)
        {
            modifiers.Add(Placeholder.Readonly.GetString());
        }

        return new FieldTemplateModel(fieldIntermed.Name, fieldIntermed.Type, string.Empty, [.. modifiers]);
    }

    private PropertyTemplateModel CreatePropertyTemplateModel(PropertyIntermed propertyIntermed)
    {
        string[] modifiers = [propertyIntermed.AccessibilityModifier.ToString()];
        return new PropertyTemplateModel(propertyIntermed.Name, propertyIntermed.Type, string.Empty, modifiers, propertyIntermed.Getter is not null, propertyIntermed.Setter is not null, [], []);
    }

    private MethodTemplateModel CreateMethodTemplateModel(MethodIntermed methodIntermed)
    {
        string[] modifiers = [methodIntermed.AccessibilityModifier.ToString()];
        return new MethodTemplateModel(methodIntermed.Name,
            methodIntermed.Parameters.Select(p => new MethodParameterModel(p.Name, p.Type)).ToArray(),
            methodIntermed.ReturnType, string.Empty, modifiers);
    }
}
