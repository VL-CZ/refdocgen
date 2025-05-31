using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.Languages;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Links;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual object types.
/// </summary>
internal class ObjectTypeTMCreator : TypeTMCreator
{
    public ObjectTypeTMCreator(IDocCommentTransformer docCommentTransformer, IEnumerable<ILanguageConfiguration> availableLanguages)
        : base(docCommentTransformer, availableLanguages)
    {
    }

    /// <summary>
    /// Creates a <see cref="ObjectTypeTM"/> instance based on the provided <see cref="IObjectTypeData"/> object.
    /// </summary>
    /// <param name="type">The <see cref="IObjectTypeData"/> instance representing the type.</param>
    /// <returns>A <see cref="ObjectTypeTM"/> instance based on the provided <paramref name="type"/>.</returns>
    internal ObjectTypeTM GetFrom(IObjectTypeData type)
    {
        var constructors = type.Constructors.OrderAlphabeticallyAndByParams().Select(GetFrom).ToArray();
        var fields = type.Fields.OrderAlphabetically().Select(GetFrom).ToArray();
        var properties = type.Properties.OrderAlphabetically().Select(GetFrom).ToArray();
        var methods = type.Methods.OrderAlphabeticallyAndByParams().Select(GetFrom).ToArray();
        var operators = type.Operators.OrderAlphabeticallyAndByParams().Select(GetFrom).ToArray();
        var indexers = type.Indexers.OrderAlphabeticallyAndByParams().Select(GetFrom).ToArray();
        var events = type.Events.OrderAlphabetically().Select(GetFrom).ToArray();

        var interfaces = type.Interfaces.Select(GetGenericTypeLink).ToArray();
        var baseType = type.BaseType is not null
            ? GetGenericTypeLink(type.BaseType)
            : null;

        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(type));

        return new ObjectTypeTM(
            Id: type.Id,
            Name: type.ShortName,
            Namespace: type.Namespace,
            Assembly: type.Assembly,
            Modifiers: modifiers,
            Constructors: constructors,
            Fields: fields,
            Properties: properties,
            Methods: methods,
            Operators: operators,
            Indexers: indexers,
            Events: events,
            NestedTypes: GetNestedTypes(type),
            TypeParameters: GetTemplateModels(type.TypeParameters),
            BaseType: baseType,
            ImplementedInterfaces: interfaces,
            Attributes: GetTemplateModels(type.Attributes),
            DeclaringType: GetCodeLinkOrNull(type.DeclaringType),
            SummaryDocComment: ToHtmlString(type.SummaryDocComment),
            RemarksDocComment: ToHtmlString(type.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(type.SeeAlsoDocComments)
        );
    }

    /// <summary>
    /// Creates a <see cref="ConstructorTM"/> instance based on the provided <see cref="IConstructorData"/> object.
    /// </summary>
    /// <param name="constructor">The <see cref="IConstructorData"/> instance representing the constructor.</param>
    /// <returns>A <see cref="ConstructorTM"/> instance based on the provided <paramref name="constructor"/>.</returns>
    private ConstructorTM GetFrom(IConstructorData constructor)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(constructor));

        return new ConstructorTM(
            Id: constructor.Id,
            TypeName: GetLanguageSpecificData(lang => lang.GetTypeName(constructor.ContainingType)),
            Parameters: GetTemplateModels(constructor.Parameters),
            Modifiers: modifiers,
            Attributes: GetTemplateModels(constructor.Attributes),
            SummaryDocComment: ToHtmlString(constructor.SummaryDocComment),
            RemarksDocComment: ToHtmlString(constructor.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(constructor.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(constructor.DocumentedExceptions));
    }

    /// <summary>
    /// Creates a <see cref="FieldTM"/> instance based on the provided <see cref="IFieldData"/> object.
    /// </summary>
    /// <param name="field">The <see cref="IFieldData"/> instance representing the field.</param>
    /// <returns>A <see cref="FieldTM"/> instance based on the provided <paramref name="field"/>.</returns>
    private FieldTM GetFrom(IFieldData field)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(field));

        return new FieldTM(
            Id: field.Id,
            Name: field.Name,
            Type: GetGenericTypeLink(field.Type),
            Modifiers: modifiers,
            ConstantValue: GetLanguageSpecificDefaultValue(field.ConstantValue),
            Attributes: GetTemplateModels(field.Attributes),
            SummaryDocComment: ToHtmlString(field.SummaryDocComment),
            RemarksDocComment: ToHtmlString(field.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(field.SeeAlsoDocComments),
            InheritedFrom: GetCodeLinkOrNull(field.InheritedFrom));
    }

    /// <summary>
    /// Creates a <see cref="PropertyTM"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="property">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTM"/> instance based on the provided <paramref name="property"/>.</returns>
    private PropertyTM GetFrom(IPropertyData property)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(property).Modifiers);
        var getterModifiers = GetLanguageSpecificData(lang => lang.GetModifiers(property).GetterModifiers);
        var setterModifiers = GetLanguageSpecificData(lang => lang.GetModifiers(property).SetterModifiers);

        return new PropertyTM(
            Id: property.Id,
            Name: property.Name,
            Type: GetGenericTypeLink(property.Type),
            HasGetter: property.Getter is not null,
            HasSetter: property.Setter is not null,
            IsSetterInitOnly: property.IsSetterInitOnly,
            Modifiers: modifiers,
            GetterModifiers: getterModifiers,
            SetterModifiers: setterModifiers,
            ConstantValue: GetLanguageSpecificDefaultValue(property.ConstantValue),
            Attributes: GetTemplateModels(property.Attributes),
            SummaryDocComment: ToHtmlString(property.SummaryDocComment),
            RemarksDocComment: ToHtmlString(property.RemarksDocComment),
            ValueDocComment: ToHtmlString(property.ValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(property.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(property.DocumentedExceptions),
            InheritedFrom: GetCodeLinkOrNull(property.InheritedFrom),
            BaseDeclaringType: GetCodeLink(property.BaseDeclaringType, property),
            ExplicitInterfaceType: GetCodeLink(property.ExplicitInterfaceType, property),
            ImplementedInterfaces: GetInterfacesImplemented(property));
    }

    /// <summary>
    /// Creates an <see cref="IndexerTM"/> instance based on the provided <see cref="IIndexerData"/> object.
    /// </summary>
    /// <param name="indexer">The <see cref="IIndexerData"/> instance representing the indexer.</param>
    /// <returns>An <see cref="IndexerTM"/> instance based on the provided <paramref name="indexer"/>.</returns>
    private IndexerTM GetFrom(IIndexerData indexer)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(indexer).Modifiers);
        var getterModifiers = GetLanguageSpecificData(lang => lang.GetModifiers(indexer).GetterModifiers);
        var setterModifiers = GetLanguageSpecificData(lang => lang.GetModifiers(indexer).SetterModifiers);

        return new IndexerTM(
            Id: indexer.Id,
            Type: GetGenericTypeLink(indexer.Type),
            Parameters: GetTemplateModels(indexer.Parameters),
            HasGetter: indexer.Getter is not null,
            HasSetter: indexer.Setter is not null,
            IsSetterInitOnly: indexer.IsSetterInitOnly,
            Modifiers: modifiers,
            GetterModifiers: getterModifiers,
            SetterModifiers: setterModifiers,
            Attributes: GetTemplateModels(indexer.Attributes),
            SummaryDocComment: ToHtmlString(indexer.SummaryDocComment),
            RemarksDocComment: ToHtmlString(indexer.RemarksDocComment),
            ValueDocComment: ToHtmlString(indexer.ValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(indexer.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(indexer.DocumentedExceptions),
            InheritedFrom: GetCodeLinkOrNull(indexer.InheritedFrom),
            BaseDeclaringType: GetCodeLink(indexer.BaseDeclaringType, indexer),
            ExplicitInterfaceType: GetCodeLink(indexer.ExplicitInterfaceType, indexer),
            ImplementedInterfaces: GetInterfacesImplemented(indexer));
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IMethodData"/> object.
    /// </summary>
    /// <param name="method">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="method"/>.</returns>
    private MethodTM GetFrom(IMethodData method)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(method));

        return new MethodTM(
            Id: method.Id,
            Name: method.Name,
            Parameters: GetTemplateModels(method.Parameters),
            TypeParameters: GetTemplateModels(method.TypeParameters),
            ReturnType: GetGenericTypeLink(method.ReturnType),
            ReturnsVoid: method.ReturnType.IsVoid,
            Modifiers: modifiers,
            Attributes: GetTemplateModels(method.Attributes),
            SummaryDocComment: ToHtmlString(method.SummaryDocComment),
            RemarksDocComment: ToHtmlString(method.RemarksDocComment),
            ReturnsDocComment: ToHtmlString(method.ReturnValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(method.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(method.DocumentedExceptions),
            InheritedFrom: GetCodeLinkOrNull(method.InheritedFrom),
            BaseDeclaringType: GetCodeLink(method.BaseDeclaringType, method),
            ExplicitInterfaceType: GetCodeLink(method.ExplicitInterfaceType, method),
            ImplementedInterfaces: GetInterfacesImplemented(method));
    }

    /// <summary>
    /// Creates a <see cref="OperatorTM"/> instance based on the provided <see cref="IOperatorData"/> object.
    /// </summary>
    /// <param name="operatorData">The <see cref="IOperatorData"/> instance representing the method.</param>
    /// <returns>A <see cref="OperatorTM"/> instance based on the provided <paramref name="operatorData"/>.</returns>
    private OperatorTM GetFrom(IOperatorData operatorData)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(operatorData));
        var name = GetLanguageSpecificData(lang => lang.GetOperatorName(operatorData));

        return new OperatorTM(
            Id: operatorData.Id,
            Name: name,
            Parameters: GetTemplateModels(operatorData.Parameters),
            TypeParameters: GetTemplateModels(operatorData.TypeParameters),
            ReturnType: GetGenericTypeLink(operatorData.ReturnType),
            ReturnsVoid: operatorData.ReturnType.IsVoid,
            IsConversionOperator: operatorData.IsConversionOperator,
            Modifiers: modifiers,
            Attributes: GetTemplateModels(operatorData.Attributes),
            SummaryDocComment: ToHtmlString(operatorData.SummaryDocComment),
            RemarksDocComment: ToHtmlString(operatorData.RemarksDocComment),
            ReturnsDocComment: ToHtmlString(operatorData.ReturnValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(operatorData.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(operatorData.DocumentedExceptions),
            InheritedFrom: GetCodeLinkOrNull(operatorData.InheritedFrom),
            BaseDeclaringType: GetCodeLink(operatorData.BaseDeclaringType, operatorData),
            ExplicitInterfaceType: GetCodeLink(operatorData.ExplicitInterfaceType, operatorData),
            ImplementedInterfaces: GetInterfacesImplemented(operatorData));
    }

    /// <summary>
    /// Creates a <see cref="EventTM"/> instance based on the provided <see cref="IEventData"/> object.
    /// </summary>
    /// <param name="eventData">The <see cref="IEventData"/> instance representing the event.</param>
    /// <returns>A <see cref="EventTM"/> instance based on the provided <paramref name="eventData"/>.</returns>
    private EventTM GetFrom(IEventData eventData)
    {
        var modifiers = GetLanguageSpecificData(lang => lang.GetModifiers(eventData));

        return new EventTM(
            Id: eventData.Id,
            Name: eventData.Name,
            Type: GetGenericTypeLink(eventData.Type),
            Modifiers: modifiers,
            Attributes: GetTemplateModels(eventData.Attributes),
            SummaryDocComment: ToHtmlString(eventData.SummaryDocComment),
            RemarksDocComment: ToHtmlString(eventData.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(eventData.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(eventData.DocumentedExceptions),
            InheritedFrom: GetCodeLinkOrNull(eventData.InheritedFrom),
            BaseDeclaringType: GetCodeLink(eventData.BaseDeclaringType, eventData),
            ExplicitInterfaceType: GetCodeLink(eventData.ExplicitInterfaceType, eventData),
            ImplementedInterfaces: GetInterfacesImplemented(eventData));
    }

    /// <summary>
    /// Gets a collection of nested types contained in the <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns>A collection of nested types contained in the <paramref name="type"/>.</returns>
    private TypeNameTM[] GetNestedTypes(IObjectTypeData type)
    {
        var nestedTypes = new List<TypeNameTM>();

        nestedTypes.AddRange(type.NestedObjectTypes.Select(GetTypeNameFrom));
        nestedTypes.AddRange(type.NestedDelegates.Select(GetTypeNameFrom));
        nestedTypes.AddRange(type.NestedEnums.Select(GetTypeNameFrom));

        return [.. nestedTypes];
    }

    /// <summary>
    /// Returns the types of the interfaces, whose part of contract the member implements.
    /// </summary>
    /// <param name="member">The provided member.</param>
    /// <returns>The types of the interfaces, whose part of contract the member implements.</returns>
    private CodeLinkTM[] GetInterfacesImplemented(ICallableMemberData member)
    {
        return [.. member.ImplementedInterfaces.Select(i => GetCodeLink(i, member))];
    }
}
