using RefDocGen.CodeElements;
using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.DocComments.Html;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual object types.
/// </summary>
internal class ObjectTypeTMCreator : TypeTMCreator
{
    public ObjectTypeTMCreator(IDocCommentTransformer docCommentTransformer) : base(docCommentTransformer)
    {
    }

    /// <summary>
    /// Creates a <see cref="ObjectTypeTM"/> instance based on the provided <see cref="IObjectTypeData"/> object.
    /// </summary>
    /// <param name="type">The <see cref="IObjectTypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="ObjectTypeTM"/> instance based on the provided <paramref name="type"/>.</returns>
    internal ObjectTypeTM GetFrom(IObjectTypeData type)
    {
        var constructors = type.Constructors.Select(GetFrom).ToArray();
        var fields = type.Fields.Select(GetFrom).ToArray();
        var properties = type.Properties.Select(GetFrom).ToArray();
        var methods = type.Methods.Select(GetFrom).ToArray();
        var operators = type.Operators.Select(GetFrom).ToArray();
        var indexers = type.Indexers.Select(GetFrom).ToArray();
        var events = type.Events.Select(GetFrom).ToArray();

        var baseType = type.BaseType is not null
            ? GetTypeLink(type.BaseType)
            : null;

        var interfaces = type.Interfaces.Select(GetTypeLink);

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

        return new ObjectTypeTM(
            type.Id,
            type.ShortName,
            type.Namespace,
            ToHtmlString(type.SummaryDocComment),
            ToHtmlString(type.RemarksDocComment),
            type.Kind.GetName(),
            modifiers.GetStrings(),
            constructors,
            fields,
            properties,
            methods,
            operators,
            indexers,
            events,
            GetTemplateModels(type.TypeParameterDeclarations),
            baseType,
            interfaces,
            GetHtmlStrings(type.SeeAlsoDocComments)
            );
    }

    /// <summary>
    /// Creates a <see cref="ConstructorTM"/> instance based on the provided <see cref="IConstructorData"/> object.
    /// </summary>
    /// <param name="constructor">The <see cref="IConstructorData"/> instance representing the constructor.</param>
    /// <returns>A <see cref="ConstructorTM"/> instance based on the provided <paramref name="constructor"/>.</returns>
    private ConstructorTM GetFrom(IConstructorData constructor)
    {
        var modifiers = GetCallableMemberModifiers(constructor);

        return new ConstructorTM(
            GetTemplateModels(constructor.Parameters),
            ToHtmlString(constructor.SummaryDocComment),
            ToHtmlString(constructor.RemarksDocComment),
            modifiers.GetStrings(),
            GetHtmlStrings(constructor.SeeAlsoDocComments),
            GetTemplateModels(constructor.DocumentedExceptions));
    }

    /// <summary>
    /// Creates a <see cref="FieldTM"/> instance based on the provided <see cref="IFieldData"/> object.
    /// </summary>
    /// <param name="field">The <see cref="IFieldData"/> instance representing the field.</param>
    /// <returns>A <see cref="FieldTM"/> instance based on the provided <paramref name="field"/>.</returns>
    private FieldTM GetFrom(IFieldData field)
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

        string? constantValue = field.ConstantValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(field.ConstantValue);

        return new FieldTM(
            field.Name,
            GetTypeLink(field.Type),
            ToHtmlString(field.SummaryDocComment),
            ToHtmlString(field.RemarksDocComment),
            modifiers.GetStrings(),
            GetHtmlStrings(field.SeeAlsoDocComments),
            constantValue);
    }

    /// <summary>
    /// Creates a <see cref="PropertyTM"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="property">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTM"/> instance based on the provided <paramref name="property"/>.</returns>
    private PropertyTM GetFrom(IPropertyData property)
    {
        var modifiers = GetCallableMemberModifiers(property);

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

        string? constantValue = property.ConstantValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(property.ConstantValue);

        return new PropertyTM(
            GetCallableMemberName(property),
            GetTypeLink(property.Type),
            ToHtmlString(property.SummaryDocComment),
            ToHtmlString(property.RemarksDocComment),
            ToHtmlString(property.ValueDocComment),
            modifiers.GetStrings(),
            property.Getter is not null,
            property.Setter is not null,
            getterModifiers.GetStrings().ToArray(),
            setterModifiers.GetStrings().ToArray(),
            GetHtmlStrings(property.SeeAlsoDocComments),
            GetTemplateModels(property.DocumentedExceptions),
            constantValue);
    }

    /// <summary>
    /// Creates an <see cref="IndexerTM"/> instance based on the provided <see cref="IIndexerData"/> object.
    /// </summary>
    /// <param name="indexer">The <see cref="IIndexerData"/> instance representing the indexer.</param>
    /// <returns>An <see cref="IndexerTM"/> instance based on the provided <paramref name="indexer"/>.</returns>
    private IndexerTM GetFrom(IIndexerData indexer)
    {
        var modifiers = GetCallableMemberModifiers(indexer);

        List<Keyword> getterModifiers = [];
        List<Keyword> setterModifiers = [];

        if (indexer.Getter is not null && indexer.Getter.AccessModifier != indexer.AccessModifier)
        {
            getterModifiers.Add(indexer.Getter.AccessModifier.ToKeyword());
        }

        if (indexer.Setter is not null && indexer.Setter.AccessModifier != indexer.AccessModifier)
        {
            setterModifiers.Add(indexer.Setter.AccessModifier.ToKeyword());
        }

        return new IndexerTM(
            GetTemplateModels(indexer.Parameters),
            GetTypeLink(indexer.Type),
            ToHtmlString(indexer.SummaryDocComment),
            ToHtmlString(indexer.RemarksDocComment),
            ToHtmlString(indexer.ValueDocComment),
            modifiers.GetStrings(),
            indexer.Getter is not null,
            indexer.Setter is not null,
            getterModifiers.GetStrings().ToArray(),
            setterModifiers.GetStrings().ToArray(),
            GetHtmlStrings(indexer.SeeAlsoDocComments),
            GetTemplateModels(indexer.DocumentedExceptions));
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IMethodData"/> object.
    /// </summary>
    /// <param name="method">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="method"/>.</returns>
    private MethodTM GetFrom(IMethodData method)
    {
        var modifiers = GetCallableMemberModifiers(method);

        return new MethodTM(
            GetCallableMemberName(method),
            GetTemplateModels(method.Parameters),
            GetTemplateModels(method.TypeParameters),
            GetTypeLink(method.ReturnType),
            method.ReturnType.IsVoid,
            ToHtmlString(method.SummaryDocComment),
            ToHtmlString(method.RemarksDocComment),
            ToHtmlString(method.ReturnValueDocComment),
            modifiers.GetStrings(),
            GetHtmlStrings(method.SeeAlsoDocComments),
            GetTemplateModels(method.DocumentedExceptions));
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IOperatorData"/> object.
    /// </summary>
    /// <param name="operatorData">The <see cref="IOperatorData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="operatorData"/>.</returns>
    private MethodTM GetFrom(IOperatorData operatorData)
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

        string name = CSharpOperatorName.Of(operatorData);

        string returnTypeName = operatorData.IsConversionOperator
            ? "" // for conversion operators, the return type is shown in its name
            : CSharpTypeName.Of(operatorData.ReturnType);

        var returnType = new TypeLinkTM(
            returnTypeName,
            typeUrlResolver.GetUrlOf(operatorData.ReturnType)
        );

        return new MethodTM(
            name,
            GetTemplateModels(operatorData.Parameters),
            GetTemplateModels(operatorData.TypeParameters),
            returnType,
            operatorData.ReturnType.IsVoid,
            ToHtmlString(operatorData.SummaryDocComment),
            ToHtmlString(operatorData.RemarksDocComment),
            ToHtmlString(operatorData.ReturnValueDocComment),
            modifiers.GetStrings(),
            GetHtmlStrings(operatorData.SeeAlsoDocComments),
            GetTemplateModels(operatorData.DocumentedExceptions));
    }

    /// <summary>
    /// Creates a <see cref="EventTM"/> instance based on the provided <see cref="IEventData"/> object.
    /// </summary>
    /// <param name="eventData">The <see cref="IEventData"/> instance representing the event.</param>
    /// <returns>A <see cref="EventTM"/> instance based on the provided <paramref name="eventData"/>.</returns>
    private EventTM GetFrom(IEventData eventData)
    {
        var modifiers = GetCallableMemberModifiers(eventData);
        modifiers.Add(Keyword.Event);

        return new EventTM(
            GetCallableMemberName(eventData),
            GetTypeLink(eventData.Type),
            ToHtmlString(eventData.SummaryDocComment),
            ToHtmlString(eventData.RemarksDocComment),
            modifiers.GetStrings(),
            GetHtmlStrings(eventData.SeeAlsoDocComments),
            GetTemplateModels(eventData.DocumentedExceptions)
            );
    }

    /// <summary>
    /// Gets the list of modifiers for the provided <paramref name="member"/> object.
    /// </summary>
    /// <param name="member">Member, whose modifiers we get.</param>
    /// <returns>A list of modifiers for the provided member.</returns>
    private List<Keyword> GetCallableMemberModifiers(ICallableMemberData member)
    {
        List<Keyword> modifiers = [];

        if (!member.IsExplicitImplementation)
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

    /// <summary>
    /// Gets the C# name of <see cref="ICallableMemberData"/>.
    /// </summary>
    /// <param name="member">The provided member.</param>
    /// <returns>C# name of the provided member.</returns>
    private string GetCallableMemberName(ICallableMemberData member)
    {
        if (member.ExplicitInterfaceType is not null)
        {
            return CSharpTypeName.Of(member.ExplicitInterfaceType) + "." + member.Name;
        }
        else
        {
            return member.Name;
        }
    }
}
