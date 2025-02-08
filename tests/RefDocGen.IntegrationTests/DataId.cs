namespace RefDocGen.IntegrationTests;

internal enum DataId
{
    MemberName,
    AttributeData,
    ReturnTypeName,
    ParameterElement,
    ParameterName,
    ParameterDoc,
    TypeParameterElement,
    TypeParameterName,
    TypeParameterDoc,
    SummaryDoc,
    RemarksDoc,
    ReturnsDoc,
    ValueDoc,
    SeeAlsoDocs,
    TypeParamConstraints,
    BaseType,
    ImplementedInterfaces,
    TypeNamespace,
    ExceptionData,
    ExceptionType,
    ExceptionDoc,
    DelegateMethod,
    NamespaceClasses,
    NamespaceInterfaces,
    NamespaceDelegates,
    NamespaceEnums,
    NamespaceStructs,
    TypeRowElement,
    TypeRowName,
    TypeRowDoc,
    NamespaceName,
    DeclaredTypeSignature,
    TypeDataSection
}

internal static class XmlDocElementExtensions
{
    internal static string GetString(this DataId dataId)
    {
        return dataId switch
        {
            DataId.MemberName => "member-name",
            DataId.AttributeData => "attribute-data",
            DataId.ReturnTypeName => "return-type-name",
            DataId.ParameterElement => "parameter-data",
            DataId.ParameterName => "parameter-name",
            DataId.ParameterDoc => "parameter-doc",
            DataId.TypeParameterElement => "type-parameter-data",
            DataId.TypeParameterName => "type-parameter-name",
            DataId.TypeParameterDoc => "type-parameter-doc",
            DataId.SummaryDoc => "summary-doc",
            DataId.RemarksDoc => "remarks-doc",
            DataId.ReturnsDoc => "returns-doc",
            DataId.ValueDoc => "value-doc",
            DataId.SeeAlsoDocs => "seealso-item",
            DataId.TypeParamConstraints => "type-param-constraints",
            DataId.BaseType => "base-type",
            DataId.ImplementedInterfaces => "implemented-interfaces",
            DataId.TypeNamespace => "type-namespace",
            DataId.ExceptionData => "exception-data",
            DataId.ExceptionType => "exception-type",
            DataId.ExceptionDoc => "exception-doc",
            DataId.DelegateMethod => "delegate-method",
            DataId.NamespaceClasses => "namespace-classes",
            DataId.NamespaceInterfaces => "namespace-interfaces",
            DataId.NamespaceDelegates => "namespace-delegates",
            DataId.NamespaceEnums => "namespace-enums",
            DataId.NamespaceStructs => "namespace-structs",
            DataId.TypeRowElement => "type-row-element",
            DataId.TypeRowName => "type-row-name",
            DataId.TypeRowDoc => "type-row-doc",
            DataId.NamespaceName => "namespace-name",
            DataId.DeclaredTypeSignature => "type-name-title",
            DataId.TypeDataSection => "declared-type-data",
            _ => throw new ArgumentOutOfRangeException(nameof(dataId), dataId, null)
        };
    }
}
