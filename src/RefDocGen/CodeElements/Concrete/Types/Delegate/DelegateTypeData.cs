using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;

namespace RefDocGen.CodeElements.Concrete.Types.Delegate;

/// <summary>
/// Class representing data of a delegate.
/// </summary>
internal class DelegateTypeData : TypeDeclaration, IDelegateTypeData
{
    /// <summary>
    /// The method used for delegate invocation (i.e. <c>Invoke</c>).
    /// </summary>
    private readonly MethodData invokeMethod;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the delegate.</param>
    /// <param name="invokeMethod"><c>Invoke</c> method of the delegate.</param>
    /// <param name="typeParameterDeclarations">Collection of the type parameters declared in this delegate; the keys represent type parameter names.</param>
    public DelegateTypeData(Type type, MethodInfo invokeMethod, IReadOnlyDictionary<string, TypeParameterDeclaration> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    {

        this.invokeMethod = new MethodData(invokeMethod, typeParameterDeclarations);
    }

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public ITypeNameData ReturnType => invokeMethod.ReturnType;

    /// <summary>
    /// List of delegate method parameters, indexed by their position.
    /// </summary>
    public IReadOnlyList<ParameterData> Parameters => invokeMethod.Parameters;

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IDelegateTypeData.Parameters => Parameters;

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterDeclaration> ITypeDeclaration.TypeParameterDeclarations =>
        TypeParameterDeclarations.Values.OrderBy(t => t.Index).ToList();
}
