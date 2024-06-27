using RefDocGen.Intermed;

namespace RefDocGen.TemplateModels.Builders;

internal class CSharpTemplateModelBuilder : ITemplateModelBuilder
{
    public ClassTemplateModel CreateClassTemplateModel(ClassIntermed classIntermed)
    {
        var fields = classIntermed.Fields.Select(CreateFieldTemplateModels).ToArray();
        var methods = classIntermed.Methods.Select(CreateMethodTemplateModels).ToArray();

        return new ClassTemplateModel(classIntermed.Name, string.Empty, [classIntermed.AccessibilityModifier.ToString()], fields, methods);
    }

    private FieldTemplateModel CreateFieldTemplateModels(FieldIntermed fieldIntermed)
    {
        string[] modifiers = [fieldIntermed.AccessibilityModifier.ToString()];
        return new FieldTemplateModel(fieldIntermed.Name, fieldIntermed.Type, string.Empty, modifiers);
    }

    private MethodTemplateModel CreateMethodTemplateModels(MethodIntermed methodIntermed)
    {
        string[] modifiers = [methodIntermed.AccessibilityModifier.ToString()];
        return new MethodTemplateModel(methodIntermed.Name,
            methodIntermed.Parameters.Select(p => new MethodParameterModel(p.Name, p.Type)).ToArray(),
            methodIntermed.ReturnType, string.Empty, modifiers);
    }
}
