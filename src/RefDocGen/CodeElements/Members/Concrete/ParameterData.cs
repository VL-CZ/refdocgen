using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Shared;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.CodeElements.Types.Concrete;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Members.Concrete;

/// <summary>
/// Class representing data of a method/constructor parameter.
/// </summary>
internal class ParameterData : IParameterData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterData"/> class.
    /// </summary>
    /// <param name="parameterInfo"><see cref="System.Reflection.ParameterInfo"/> object representing the parameter.</param>
    /// <param name="availableTypeParameters">Collection of type parameters declared in the containing type; the keys represent type parameter names.</param>
    /// <param name="isExtensionParameter">
    /// <inheritdoc cref="IsExtensionParameter" path="/summary"/>
    /// </param>
    /// /// <param name="attributes">Collection of attributes applied to the parameter.</param>
    public ParameterData(
        ParameterInfo parameterInfo,
        IReadOnlyDictionary<string, TypeParameterData> availableTypeParameters,
        IReadOnlyList<IAttributeData> attributes,
        bool isExtensionParameter = false)
    {
        ParameterInfo = parameterInfo;
        Type = parameterInfo.ParameterType.GetTypeNameData(availableTypeParameters);
        DocComment = XmlDocElements.EmptyParamWithName(Name);
        IsExtensionParameter = isExtensionParameter;
        Attributes = attributes;
    }

    /// <inheritdoc/>
    public ParameterInfo ParameterInfo { get; }

    /// <inheritdoc/>
    public string Name => ParameterInfo.Name ?? string.Empty;

    /// <inheritdoc/>
    public ITypeNameData Type { get; }

    /// <inheritdoc/>
    public bool IsParamsCollection => ParameterInfo.GetCustomAttribute(typeof(ParamArrayAttribute)) != null;

    /// <inheritdoc/>
    public bool IsInput => ParameterInfo.IsIn;

    /// <inheritdoc/>
    public bool IsOutput => ParameterInfo.IsOut;

    /// <inheritdoc/>
    public bool IsByRef => ParameterInfo.ParameterType.IsByRef;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; }

    /// <inheritdoc/>
    public int Position => ParameterInfo.Position;

    /// <inheritdoc/>
    public object? DefaultValue => ParameterInfo.RawDefaultValue;

    /// <inheritdoc/>
    public bool IsExtensionParameter { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IAttributeData> Attributes { get; }
}

