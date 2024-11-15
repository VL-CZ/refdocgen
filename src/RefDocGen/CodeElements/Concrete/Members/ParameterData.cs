using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of a method/constructor parameter.
/// </summary>
internal class ParameterData : IParameterData
{
    /// <inheritdoc/>
    public ParameterData(ParameterInfo parameterInfo, IReadOnlyList<TypeParameterDeclaration> declaredTypeParameters)
    {
        ParameterInfo = parameterInfo;
        Type = new TypeNameData(parameterInfo.ParameterType, declaredTypeParameters);
        DocComment = XmlDocElementFactory.EmptyParamWithName(Name);
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
    public bool IsOptional => ParameterInfo.IsOptional;

    /// <inheritdoc/>
    public bool IsInput => ParameterInfo.IsIn;

    /// <inheritdoc/>
    public bool IsOutput => ParameterInfo.IsOut;

    /// <inheritdoc/>
    public bool IsByRef => ParameterInfo.ParameterType.IsByRef;

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; }
}

