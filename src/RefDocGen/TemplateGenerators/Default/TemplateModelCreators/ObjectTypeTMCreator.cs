using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Types;
using RefDocGen.TemplateGenerators.Tools;
using RefDocGen.TemplateGenerators.Tools.Keywords;
using RefDocGen.TemplateGenerators.Tools.TypeName;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

/// <summary>
/// Class responsible for creating template models representing the individual object types.
/// </summary>
internal static class ObjectTypeTMCreator
{
    /// <summary>
    /// Creates a <see cref="ObjectTypeTM"/> instance based on the provided <see cref="IObjectTypeData"/> object.
    /// </summary>
    /// <param name="typeData">The <see cref="IObjectTypeData"/> instance representing the type.</param>
    /// <param name="commentParser">TODO</param>
    /// <returns>A <see cref="ObjectTypeTM"/> instance based on the provided <paramref name="typeData"/>.</returns>
    internal static ObjectTypeTM GetFrom(IObjectTypeData typeData, HtmlCommentParser commentParser)
    {
        var constructors = typeData.Constructors.Select(c => GetFrom(c, commentParser)).ToArray();
        var fields = typeData.Fields.Select(c => GetFrom(c, commentParser)).ToArray();
        var properties = typeData.Properties.Select(GetFrom).ToArray();
        var methods = typeData.Methods.Select(GetFrom).ToArray();
        var operators = typeData.Operators.Select(GetFrom).ToArray();
        var indexers = typeData.Indexers.Select(GetFrom).ToArray();
        var typeParameterDeclarations = typeData.TypeParameterDeclarations.Select(TypeParameterTMCreator.GetFrom).ToArray();

        string? baseType = typeData.BaseType is not null
            ? CSharpTypeName.Of(typeData.BaseType)
            : null;

        var interfaces = typeData.Interfaces.Select(CSharpTypeName.Of);

        List<Keyword> modifiers = [typeData.AccessModifier.ToKeyword()];

        if (SealedKeyword.IsPresentIn(typeData))
        {
            modifiers.Add(Keyword.Sealed);
        }

        if (AbstractKeyword.IsPresentIn(typeData))
        {
            modifiers.Add(Keyword.Abstract);
        }
        string summaryDocComment = commentParser.Parse(typeData.SummaryDocComment);

        return new ObjectTypeTM(
            typeData.Id,
            CSharpTypeName.Of(typeData),
            typeData.Namespace,
            summaryDocComment,
            typeData.RemarksDocComment.Value,
            typeData.Kind.GetName(),
            modifiers.GetStrings(),
            constructors,
            fields,
            properties,
            methods,
            operators,
            indexers,
            typeParameterDeclarations,
            baseType,
            interfaces
            );
    }

    /// <summary>
    /// Creates a <see cref="ConstructorTM"/> instance based on the provided <see cref="IConstructorData"/> object.
    /// </summary>
    /// <param name="constructorData">The <see cref="IConstructorData"/> instance representing the constructor.</param>
    /// <param name="commentParser">TODO</param>
    /// <returns>A <see cref="ConstructorTM"/> instance based on the provided <paramref name="constructorData"/>.</returns>
    private static ConstructorTM GetFrom(IConstructorData constructorData, HtmlCommentParser commentParser)
    {
        var modifiers = GetCallableMemberModifiers(constructorData);
        var exceptionTMs = constructorData.Exceptions.Select(ExceptionTMCreator.GetFrom);

        string summaryDocComment = commentParser.Parse(constructorData.SummaryDocComment);

        return new ConstructorTM(
            constructorData.Parameters.Select(ParameterTMCreator.GetFrom).ToArray(),
            summaryDocComment,
            constructorData.RemarksDocComment.Value,
            modifiers.GetStrings(),
            exceptionTMs);
    }

    /// <summary>
    /// Creates a <see cref="FieldTM"/> instance based on the provided <see cref="IFieldData"/> object.
    /// </summary>
    /// <param name="fieldData">The <see cref="IFieldData"/> instance representing the field.</param>
    /// <param name="commentParser">TODO</param>
    /// <returns>A <see cref="FieldTM"/> instance based on the provided <paramref name="fieldData"/>.</returns>
    private static FieldTM GetFrom(IFieldData fieldData, HtmlCommentParser commentParser)
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

        string docComment = commentParser.Parse(fieldData.SummaryDocComment);

        string[] seeAlsoDocComments = fieldData.SeeAlsoDocComments.Select(commentParser.Parse).ToArray();

        return new FieldTM(
            fieldData.Name,
            CSharpTypeName.Of(fieldData.Type),
            docComment,
            fieldData.RemarksDocComment.Value,
            modifiers.GetStrings(),
            seeAlsoDocComments);
    }

    /// <summary>
    /// Creates a <see cref="PropertyTM"/> instance based on the provided <see cref="IPropertyData"/> object.
    /// </summary>
    /// <param name="propertyData">The <see cref="IPropertyData"/> instance representing the property.</param>
    /// <returns>A <see cref="PropertyTM"/> instance based on the provided <paramref name="propertyData"/>.</returns>
    private static PropertyTM GetFrom(IPropertyData propertyData)
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

        var exceptionTMs = propertyData.Exceptions.Select(ExceptionTMCreator.GetFrom);

        return new PropertyTM(
            propertyData.Name,
            CSharpTypeName.Of(propertyData.Type),
            propertyData.SummaryDocComment.Value,
            propertyData.RemarksDocComment.Value,
            propertyData.ValueDocComment.Value,
            modifiers.GetStrings(),
            propertyData.Getter is not null,
            propertyData.Setter is not null,
            getterModifiers.GetStrings(),
            setterModifiers.GetStrings(),
            exceptionTMs);
    }

    /// <summary>
    /// Creates an <see cref="IndexerTM"/> instance based on the provided <see cref="IIndexerData"/> object.
    /// </summary>
    /// <param name="indexer">The <see cref="IIndexerData"/> instance representing the indexer.</param>
    /// <returns>An <see cref="IndexerTM"/> instance based on the provided <paramref name="indexer"/>.</returns>
    private static IndexerTM GetFrom(IIndexerData indexer)
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

        var exceptionTMs = indexer.Exceptions.Select(ExceptionTMCreator.GetFrom);

        return new IndexerTM(
            indexer.Parameters.Select(ParameterTMCreator.GetFrom).ToArray(),
            CSharpTypeName.Of(indexer.Type),
            indexer.SummaryDocComment.Value,
            indexer.RemarksDocComment.Value,
            indexer.ValueDocComment.Value,
            modifiers.GetStrings(),
            indexer.Getter is not null,
            indexer.Setter is not null,
            getterModifiers.GetStrings(),
            setterModifiers.GetStrings(),
            exceptionTMs);
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IMethodData"/> object.
    /// </summary>
    /// <param name="methodData">The <see cref="IMethodData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="methodData"/>.</returns>
    private static MethodTM GetFrom(IMethodData methodData)
    {
        var modifiers = GetCallableMemberModifiers(methodData);
        var exceptionTMs = methodData.Exceptions.Select(ExceptionTMCreator.GetFrom);

        return new MethodTM(
            methodData.Name,
            methodData.Parameters.Select(ParameterTMCreator.GetFrom).ToArray(),
            CSharpTypeName.Of(methodData.ReturnType),
            methodData.ReturnType.IsVoid,
            methodData.SummaryDocComment.Value,
            methodData.RemarksDocComment.Value,
            methodData.ReturnValueDocComment.Value,
            modifiers.GetStrings(),
            methodData.TypeParameters.Select(TypeParameterTMCreator.GetFrom).ToArray(),
            exceptionTMs);
    }

    /// <summary>
    /// Creates a <see cref="MethodTM"/> instance based on the provided <see cref="IOperatorData"/> object.
    /// </summary>
    /// <param name="operatorData">The <see cref="IOperatorData"/> instance representing the method.</param>
    /// <returns>A <see cref="MethodTM"/> instance based on the provided <paramref name="operatorData"/>.</returns>
    private static MethodTM GetFrom(IOperatorData operatorData)
    {
        var modifiers = GetCallableMemberModifiers(operatorData);
        var exceptionTMs = operatorData.Exceptions.Select(ExceptionTMCreator.GetFrom);

        return new MethodTM(
            operatorData.Kind.GetName(),
            operatorData.Parameters.Select(ParameterTMCreator.GetFrom).ToArray(),
            CSharpTypeName.Of(operatorData.ReturnType),
            operatorData.ReturnType.IsVoid,
            operatorData.SummaryDocComment.Value,
            operatorData.RemarksDocComment.Value,
            operatorData.ReturnValueDocComment.Value,
            modifiers.GetStrings(),
            operatorData.TypeParameters.Select(TypeParameterTMCreator.GetFrom).ToArray(),
            exceptionTMs);
    }

    /// <summary>
    /// Gets the list of modifiers for the provided <paramref name="memberData"/> object.
    /// </summary>
    /// <param name="memberData">Member, whose modifiers we get.</param>
    /// <returns>A list of modifiers for the provided member.</returns>
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
