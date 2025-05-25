using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModelCreators.Tools;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual object types.
/// </summary>
internal class ObjectTypeTMCreator : TypeTMCreator
{
    public ObjectTypeTMCreator(IDocCommentTransformer docCommentTransformer, IReadOnlyDictionary<Language, ILanguageSpecificData> languages)
        : base(docCommentTransformer, languages)
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

        var interfaces = type.Interfaces.Select(GetTypeLink).ToArray();

        var modifiers = GetLocalizedData(lang => lang.GetModifiers(type));

        return new ObjectTypeTM(
            Id: type.Id,
            Name: GetTypeName(type),
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
            BaseType: GetTypeLinkOrNull(type.BaseType),
            ImplementedInterfaces: interfaces,
            Attributes: GetTemplateModels(type.Attributes),
            DeclaringType: GetTypeLinkOrNull(type.DeclaringType),
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
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(constructor));

        return new ConstructorTM(
            Id: constructor.Id,
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
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(field));

        return new FieldTM(
            Id: field.Id,
            Name: field.Name,
            Type: GetTypeLink(field.Type),
            Modifiers: modifiers,
            ConstantValue: GetLocalizedDefaultValue(field.ConstantValue),
            Attributes: GetTemplateModels(field.Attributes),
            SummaryDocComment: ToHtmlString(field.SummaryDocComment),
            RemarksDocComment: ToHtmlString(field.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(field.SeeAlsoDocComments),
            InheritedFrom: GetTypeLinkOrNull(field.InheritedFrom));
    }

    /// <summary>
    /// Creates a <see cref="PropertyTM"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="property">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTM"/> instance based on the provided <paramref name="property"/>.</returns>
    private PropertyTM GetFrom(IPropertyData property)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(property).Modifiers);
        var getterModifiers = GetLocalizedData(lang => lang.GetModifiers(property).GetterModifiers);
        var setterModifiers = GetLocalizedData(lang => lang.GetModifiers(property).SetterModifiers);

        return new PropertyTM(
            Id: property.Id,
            Name: GetCallableMemberName(property),
            Type: GetTypeLink(property.Type),
            HasGetter: property.Getter is not null,
            HasSetter: property.Setter is not null,
            IsSetterInitOnly: property.IsSetterInitOnly,
            Modifiers: modifiers,
            GetterModifiers: getterModifiers,
            SetterModifiers: setterModifiers,
            ConstantValue: GetLocalizedDefaultValue(property.ConstantValue),
            Attributes: GetTemplateModels(property.Attributes),
            SummaryDocComment: ToHtmlString(property.SummaryDocComment),
            RemarksDocComment: ToHtmlString(property.RemarksDocComment),
            ValueDocComment: ToHtmlString(property.ValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(property.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(property.DocumentedExceptions),
            InheritedFrom: GetTypeLinkOrNull(property.InheritedFrom),
            BaseDeclaringType: GetTypeMemberLinkOrNull(property.BaseDeclaringType, property),
            ExplicitInterfaceType: GetTypeMemberLinkOrNull(property.ExplicitInterfaceType, property),
            ImplementedInterfaces: GetInterfacesImplemented(property));
    }

    /// <summary>
    /// Creates an <see cref="IndexerTM"/> instance based on the provided <see cref="IIndexerData"/> object.
    /// </summary>
    /// <param name="indexer">The <see cref="IIndexerData"/> instance representing the indexer.</param>
    /// <returns>An <see cref="IndexerTM"/> instance based on the provided <paramref name="indexer"/>.</returns>
    private IndexerTM GetFrom(IIndexerData indexer)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(indexer).Modifiers);
        var getterModifiers = GetLocalizedData(lang => lang.GetModifiers(indexer).GetterModifiers);
        var setterModifiers = GetLocalizedData(lang => lang.GetModifiers(indexer).SetterModifiers);

        return new IndexerTM(
            Id: indexer.Id,
            Type: GetTypeLink(indexer.Type),
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
            InheritedFrom: GetTypeLinkOrNull(indexer.InheritedFrom),
            BaseDeclaringType: GetTypeMemberLinkOrNull(indexer.BaseDeclaringType, indexer),
            ExplicitInterfaceType: GetTypeMemberLinkOrNull(indexer.ExplicitInterfaceType, indexer),
            ImplementedInterfaces: GetInterfacesImplemented(indexer));
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IMethodData"/> object.
    /// </summary>
    /// <param name="method">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="method"/>.</returns>
    private MethodTM GetFrom(IMethodData method)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(method));

        return new MethodTM(
            Id: method.Id,
            Name: GetCallableMemberName(method),
            Parameters: GetTemplateModels(method.Parameters),
            TypeParameters: GetTemplateModels(method.TypeParameters),
            ReturnType: GetTypeLink(method.ReturnType),
            ReturnsVoid: method.ReturnType.IsVoid,
            Modifiers: modifiers,
            Attributes: GetTemplateModels(method.Attributes),
            SummaryDocComment: ToHtmlString(method.SummaryDocComment),
            RemarksDocComment: ToHtmlString(method.RemarksDocComment),
            ReturnsDocComment: ToHtmlString(method.ReturnValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(method.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(method.DocumentedExceptions),
            InheritedFrom: GetTypeLinkOrNull(method.InheritedFrom),
            BaseDeclaringType: GetTypeMemberLinkOrNull(method.BaseDeclaringType, method),
            ExplicitInterfaceType: GetTypeMemberLinkOrNull(method.ExplicitInterfaceType, method),
            ImplementedInterfaces: GetInterfacesImplemented(method));
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IOperatorData"/> object.
    /// </summary>
    /// <param name="operatorData">The <see cref="IOperatorData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="operatorData"/>.</returns>
    private MethodTM GetFrom(IOperatorData operatorData)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(operatorData));

        string name = CSharpOperatorName.Of(operatorData);

        string returnTypeName = operatorData.IsConversionOperator
            ? "" // for conversion operators, the return type is shown in its name
            : CSharpTypeName.Of(operatorData.ReturnType);

        var returnType = new TypeLinkTM(
            returnTypeName,
            typeUrlResolver.GetUrlOf(operatorData.ReturnType)
        );

        return new MethodTM(
            Id: operatorData.Id,
            Name: name,
            Parameters: GetTemplateModels(operatorData.Parameters),
            TypeParameters: GetTemplateModels(operatorData.TypeParameters),
            ReturnType: returnType,
            ReturnsVoid: operatorData.ReturnType.IsVoid,
            Modifiers: modifiers,
            Attributes: GetTemplateModels(operatorData.Attributes),
            SummaryDocComment: ToHtmlString(operatorData.SummaryDocComment),
            RemarksDocComment: ToHtmlString(operatorData.RemarksDocComment),
            ReturnsDocComment: ToHtmlString(operatorData.ReturnValueDocComment),
            SeeAlsoDocComments: GetHtmlStrings(operatorData.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(operatorData.DocumentedExceptions),
            InheritedFrom: GetTypeLinkOrNull(operatorData.InheritedFrom),
            BaseDeclaringType: GetTypeMemberLinkOrNull(operatorData.BaseDeclaringType, operatorData),
            ExplicitInterfaceType: GetTypeMemberLinkOrNull(operatorData.ExplicitInterfaceType, operatorData),
            ImplementedInterfaces: GetInterfacesImplemented(operatorData));
    }

    /// <summary>
    /// Creates a <see cref="EventTM"/> instance based on the provided <see cref="IEventData"/> object.
    /// </summary>
    /// <param name="eventData">The <see cref="IEventData"/> instance representing the event.</param>
    /// <returns>A <see cref="EventTM"/> instance based on the provided <paramref name="eventData"/>.</returns>
    private EventTM GetFrom(IEventData eventData)
    {
        var modifiers = GetLocalizedData(lang => lang.GetModifiers(eventData));

        return new EventTM(
            Id: eventData.Id,
            Name: GetCallableMemberName(eventData),
            Type: GetTypeLink(eventData.Type),
            Modifiers: modifiers,
            Attributes: GetTemplateModels(eventData.Attributes),
            SummaryDocComment: ToHtmlString(eventData.SummaryDocComment),
            RemarksDocComment: ToHtmlString(eventData.RemarksDocComment),
            SeeAlsoDocComments: GetHtmlStrings(eventData.SeeAlsoDocComments),
            Exceptions: GetTemplateModels(eventData.DocumentedExceptions),
            InheritedFrom: GetTypeLinkOrNull(eventData.InheritedFrom),
            BaseDeclaringType: GetTypeMemberLinkOrNull(eventData.BaseDeclaringType, eventData),
            ExplicitInterfaceType: GetTypeMemberLinkOrNull(eventData.ExplicitInterfaceType, eventData),
            ImplementedInterfaces: GetInterfacesImplemented(eventData));
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
    /// Gets the <see cref="TypeLinkTM"/> of the provided type member.
    /// </summary>
    /// <param name="type">The provided type containing the member.</param>
    /// <param name="member">The member for which the URL is returned.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/> and <paramref name="member"/>.</returns>
    private TypeLinkTM GetTypeMemberLink(ITypeNameData type, IMemberData member)
    {
        return new TypeLinkTM(
            CSharpTypeName.Of(type) + "." + member.Name,
            typeUrlResolver.GetUrlOf(type.TypeDeclarationId, member.Id));
    }

    /// <summary>
    /// Gets the <see cref="TypeLinkTM"/> of the provided type member.
    /// </summary>
    /// <param name="type">The provided type containing the member.</param>
    /// <param name="member">The member for which the URL is returned.</param>
    /// <returns><see cref="TypeLinkTM"/> corresponding to the provided <paramref name="type"/> and <paramref name="member"/>. <see langword="null"/> if the provided <paramref name="type"/> is <see langword="null"/>.</returns>
    private TypeLinkTM? GetTypeMemberLinkOrNull(ITypeNameData? type, IMemberData member)
    {
        if (type is null)
        {
            return null;
        }

        return GetTypeMemberLink(type, member);
    }

    /// <summary>
    /// Returns the types of the interfaces, whose part of contract the member implements.
    /// </summary>
    /// <param name="member">The provided member.</param>
    /// <returns>The types of the interfaces, whose part of contract the member implements.</returns>
    private TypeLinkTM[] GetInterfacesImplemented(ICallableMemberData member)
    {
        return [.. member.ImplementedInterfaces.Select(i => GetTypeMemberLink(i, member))];
    }
}
