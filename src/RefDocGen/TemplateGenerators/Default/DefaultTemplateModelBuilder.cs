using RefDocGen.MemberData;
using RefDocGen.MemberData.Interfaces;
using RefDocGen.TemplateGenerators.Default.TemplateModels;
using RefDocGen.TemplateGenerators.Default.Tools;
using RefDocGen.TemplateGenerators.Default.Tools.Extensions;

namespace RefDocGen.TemplateGenerators.Default;

internal class DefaultTemplateModelBuilder
{
    public ClassTemplateModel CreateClassTemplateModel(ClassData classData)
    {
        var fields = classData.Fields.Select(CreateFieldTemplateModel).ToArray();
        var properties = classData.Properties.Select(CreatePropertyTemplateModel).ToArray();
        var methods = classData.Methods.Select(CreateMethodTemplateModel).ToArray();

        return new ClassTemplateModel(classData.Name, classData.DocComment.Value, [classData.AccessModifier.GetPlaceholderString()], fields, properties, methods);
    }

    private FieldTemplateModel CreateFieldTemplateModel(FieldData fieldData)
    {
        List<string> modifiers = [fieldData.AccessModifier.GetPlaceholderString()];

        if (fieldData.IsStatic && !fieldData.IsConstant)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (fieldData.IsConstant)
        {
            modifiers.Add(Placeholder.Const.GetString());
        }
        if (fieldData.IsReadonly)
        {
            modifiers.Add(Placeholder.Readonly.GetString());
        }

        return new FieldTemplateModel(fieldData.Name, fieldData.Type, fieldData.DocComment.Value, modifiers);
    }

    private PropertyTemplateModel CreatePropertyTemplateModel(PropertyData propertyData)
    {
        List<string> modifiers = [propertyData.AccessModifier.GetPlaceholderString()];

        modifiers.AddRange(GetCallableMemberModifiers(propertyData));

        List<string> getterModifiers = [];
        List<string> setterModifiers = [];

        if (propertyData.Getter is not null && propertyData.Getter.AccessModifier != propertyData.AccessModifier)
        {
            getterModifiers.Add(propertyData.Getter.AccessModifier.GetPlaceholderString());
        }
        if (propertyData.Setter is not null && propertyData.Setter.AccessModifier != propertyData.AccessModifier)
        {
            setterModifiers.Add(propertyData.Setter.AccessModifier.GetPlaceholderString());
        }

        return new PropertyTemplateModel(propertyData.Name, propertyData.Type, propertyData.DocComment.Value, modifiers, propertyData.Getter is not null, propertyData.Setter is not null, [.. getterModifiers], [.. setterModifiers]);
    }

    private MethodTemplateModel CreateMethodTemplateModel(MethodData methodData)
    {
        List<string> modifiers = [methodData.AccessModifier.GetPlaceholderString()];

        modifiers.AddRange(GetCallableMemberModifiers(methodData));

        return new MethodTemplateModel(methodData.Name,
            methodData.GetParameters().Select(CreateMethodParameterModel).ToArray(),
            methodData.ReturnType, methodData.DocComment.Value, modifiers);
    }

    private MethodParameterTemplateModel CreateMethodParameterModel(MethodParameterData parameterData)
    {
        var modifiers = new List<string>();

        if (parameterData.IsInput)
        {
            modifiers.Add(Placeholder.In.GetString());
        }
        if (parameterData.IsOutput)
        {
            modifiers.Add(Placeholder.Out.GetString());
        }
        if (parameterData.HasRefKeyword())
        {
            modifiers.Add(Placeholder.Ref.GetString());
        }
        if (parameterData.IsParamsCollection)
        {
            modifiers.Add(Placeholder.Params.GetString());
        }
        if (parameterData.IsOptional)
        {
            modifiers.Add("optional"); // TODO: add default value
        }

        return new MethodParameterTemplateModel(parameterData.Name, parameterData.Type, parameterData.DocComment.Value, modifiers);
    }

    private List<string> GetCallableMemberModifiers(ICallableMemberData memberData)
    {
        var modifiers = new List<string>();
        if (memberData.IsStatic)
        {
            modifiers.Add(Placeholder.Static.GetString());
        }
        if (memberData.IsAbstract)
        {
            modifiers.Add(Placeholder.Abstract.GetString());
        }
        if (memberData.HasVirtualKeyword())
        {
            modifiers.Add(Placeholder.Virtual.GetString());
        }
        if (memberData.OverridesAnotherMember)
        {
            modifiers.Add(Placeholder.Override.GetString());
        }
        if (memberData.IsSealed)
        {
            modifiers.Add(Placeholder.Sealed.GetString());
        }
        if (memberData.IsAsync)
        {
            modifiers.Add(Placeholder.Async.GetString());
        }

        return modifiers;
    }
}
