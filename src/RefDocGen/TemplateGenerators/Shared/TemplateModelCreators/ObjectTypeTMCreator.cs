using RefDocGen.CodeElements.Members;
using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.TemplateGenerators.Shared.DocComments.Html;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Shared.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Shared.Tools;
using RefDocGen.TemplateGenerators.Shared.Tools.Keywords;
using RefDocGen.TemplateGenerators.Shared.Tools.Names;

namespace RefDocGen.TemplateGenerators.Shared.TemplateModelCreators;

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

        var interfaces = type.Interfaces.Select(GetTypeLink).ToArray();

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
            GetTypeName(type),
            type.Namespace,
            type.Assembly,
            type.Kind.GetName(),
            modifiers.GetStrings(),
            constructors,
            fields,
            properties,
            methods,
            operators,
            indexers,
            events,
            GetNestedTypes(type),
            GetTemplateModels(type.TypeParameters),
            GetTypeLinkOrNull(type.BaseType),
            interfaces,
            GetTemplateModels(type.Attributes),
            GetTypeLinkOrNull(type.DeclaringType),
            ToHtmlString(type.SummaryDocComment),
            ToHtmlString(type.RemarksDocComment),
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
        List<Keyword> modifiers = [constructor.AccessModifier.ToKeyword()];

        if (constructor.IsStatic)
        {
            modifiers.Add(Keyword.Static);
        }

        return new ConstructorTM(
            constructor.Id,
            GetTemplateModels(constructor.Parameters),
            modifiers.GetStrings(),
            GetTemplateModels(constructor.Attributes),
            ToHtmlString(constructor.SummaryDocComment),
            ToHtmlString(constructor.RemarksDocComment),
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
        if (field.IsRequired)
        {
            modifiers.Add(Keyword.Required);
        }

        string? constantValue = field.ConstantValue == DBNull.Value
            ? null
            : LiteralValueFormatter.Format(field.ConstantValue);

        return new FieldTM(
            field.Id,
            field.Name,
            GetTypeLink(field.Type),
            modifiers.GetStrings(),
            constantValue,
            GetTemplateModels(field.Attributes),
            ToHtmlString(field.SummaryDocComment),
            ToHtmlString(field.RemarksDocComment),
            GetHtmlStrings(field.SeeAlsoDocComments),
            GetTypeLinkOrNull(field.InheritedFrom));
    }

    /// <summary>
    /// Creates a <see cref="PropertyTM"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="property">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTM"/> instance based on the provided <paramref name="property"/>.</returns>
    private PropertyTM GetFrom(IPropertyData property)
    {
        var modifiers = GetCallableMemberModifiers(property);

        if (property.IsRequired)
        {
            modifiers.Add(Keyword.Required);
        }

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
            property.Id,
            GetCallableMemberName(property),
            GetTypeLink(property.Type),
            property.Getter is not null,
            property.Setter is not null,
            property.IsSetterInitOnly,
            modifiers.GetStrings(),
            getterModifiers.GetStrings(),
            setterModifiers.GetStrings(),
            constantValue,
            GetTemplateModels(property.Attributes),
            ToHtmlString(property.SummaryDocComment),
            ToHtmlString(property.RemarksDocComment),
            ToHtmlString(property.ValueDocComment),
            GetHtmlStrings(property.SeeAlsoDocComments),
            GetTemplateModels(property.DocumentedExceptions),
            GetTypeLinkOrNull(property.InheritedFrom),
            GetTypeMemberLinkOrNull(property.BaseDeclaringType, property),
            GetTypeMemberLinkOrNull(property.ExplicitInterfaceType, property),
            GetInterfacesImplemented(property));
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
            indexer.Id,
            GetTypeLink(indexer.Type),
            GetTemplateModels(indexer.Parameters),
            indexer.Getter is not null,
            indexer.Setter is not null,
            indexer.IsSetterInitOnly,
            modifiers.GetStrings(),
            getterModifiers.GetStrings(),
            setterModifiers.GetStrings(),
            GetTemplateModels(indexer.Attributes),
            ToHtmlString(indexer.SummaryDocComment),
            ToHtmlString(indexer.RemarksDocComment),
            ToHtmlString(indexer.ValueDocComment),
            GetHtmlStrings(indexer.SeeAlsoDocComments),
            GetTemplateModels(indexer.DocumentedExceptions),
            GetTypeLinkOrNull(indexer.InheritedFrom),
            GetTypeMemberLinkOrNull(indexer.BaseDeclaringType, indexer),
            GetTypeMemberLinkOrNull(indexer.ExplicitInterfaceType, indexer),
            GetInterfacesImplemented(indexer));
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
            method.Id,
            GetCallableMemberName(method),
            GetTemplateModels(method.Parameters),
            GetTemplateModels(method.TypeParameters),
            GetTypeLink(method.ReturnType),
            method.ReturnType.IsVoid,
            modifiers.GetStrings(),
            GetTemplateModels(method.Attributes),
            ToHtmlString(method.SummaryDocComment),
            ToHtmlString(method.RemarksDocComment),
            ToHtmlString(method.ReturnValueDocComment),
            GetHtmlStrings(method.SeeAlsoDocComments),
            GetTemplateModels(method.DocumentedExceptions),
            GetTypeLinkOrNull(method.InheritedFrom),
            GetTypeMemberLinkOrNull(method.BaseDeclaringType, method),
            GetTypeMemberLinkOrNull(method.ExplicitInterfaceType, method),
            GetInterfacesImplemented(method));
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
            operatorData.Id,
            name,
            GetTemplateModels(operatorData.Parameters),
            GetTemplateModels(operatorData.TypeParameters),
            returnType,
            operatorData.ReturnType.IsVoid,
            modifiers.GetStrings(),
            GetTemplateModels(operatorData.Attributes),
            ToHtmlString(operatorData.SummaryDocComment),
            ToHtmlString(operatorData.RemarksDocComment),
            ToHtmlString(operatorData.ReturnValueDocComment),
            GetHtmlStrings(operatorData.SeeAlsoDocComments),
            GetTemplateModels(operatorData.DocumentedExceptions),
            GetTypeLinkOrNull(operatorData.InheritedFrom),
            GetTypeMemberLinkOrNull(operatorData.BaseDeclaringType, operatorData),
            GetTypeMemberLinkOrNull(operatorData.ExplicitInterfaceType, operatorData),
            GetInterfacesImplemented(operatorData));
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
            eventData.Id,
            GetCallableMemberName(eventData),
            GetTypeLink(eventData.Type),
            modifiers.GetStrings(),
            GetTemplateModels(eventData.Attributes),
            ToHtmlString(eventData.SummaryDocComment),
            ToHtmlString(eventData.RemarksDocComment),
            GetHtmlStrings(eventData.SeeAlsoDocComments),
            GetTemplateModels(eventData.DocumentedExceptions),
            GetTypeLinkOrNull(eventData.InheritedFrom),
            GetTypeMemberLinkOrNull(eventData.BaseDeclaringType, eventData),
            GetTypeMemberLinkOrNull(eventData.ExplicitInterfaceType, eventData),
            GetInterfacesImplemented(eventData));
    }

    /// <summary>
    /// Gets the list of modifiers for the provided <paramref name="member"/> object.
    /// </summary>
    /// <param name="member">Member, whose modifiers we get.</param>
    /// <returns>A list of modifiers for the provided member.</returns>
    private List<Keyword> GetCallableMemberModifiers(ICallableMemberData member)
    {
        List<Keyword> modifiers = [];

        if (!member.IsExplicitImplementation) // don't add access modifiers for explicitly implemeneted members
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

    /// <summary>
    /// Gets a collection of nested types contained in the <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The provided type.</param>
    /// <returns>A collection of nested types contained in the <paramref name="type"/>.</returns>
    private TypeNameTM[] GetNestedTypes(IObjectTypeData type)
    {
        var nestedTypes = new List<TypeNameTM>();

        nestedTypes.AddRange(type.NestedObjectTypes.Select(t => GetFrom(t, t.Kind.GetName())));
        nestedTypes.AddRange(type.NestedDelegates.Select(d => GetFrom(d, "delegate"))); // TODO: use constants
        nestedTypes.AddRange(type.NestedEnums.Select(d => GetFrom(d, "enum")));

        return [.. nestedTypes];
    }

    /// <summary>
    /// Creates a <see cref="TypeNameTM"/> instance representing a nested type based on the provided <see cref="ITypeDeclaration"/> object.
    /// </summary>
    /// <param name="type">The <see cref="ITypeDeclaration"/> instance representing the nested type.</param>
    /// <param name="typeKindName">Name of the type kind.</param>
    /// <returns>A <see cref="TypeNameTM"/> instance based on the provided <paramref name="type"/>.</returns>
    private TypeNameTM GetFrom(ITypeDeclaration type, string typeKindName)
    {
        return new TypeNameTM(type.Id, typeKindName, CSharpTypeName.Of(type), ToHtmlString(type.SummaryDocComment));
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
