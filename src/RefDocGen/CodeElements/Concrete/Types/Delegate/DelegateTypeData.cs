using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Xml.Linq;
using RefDocGen.CodeElements.Concrete.Members;
using RefDocGen.CodeElements.Abstract.Types.TypeName;
using RefDocGen.CodeElements.Abstract.Types.Exception;

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
    public DelegateTypeData(Type type, MethodInfo invokeMethod, Dictionary<string, TypeParameterData> typeParameterDeclarations)
        : base(type, typeParameterDeclarations)
    {
        this.invokeMethod = new MethodData(invokeMethod, typeParameterDeclarations);

        AllMembers = new Dictionary<string, MemberData>
        {
            [InvokeMethod.Id] = (MemberData)InvokeMethod
        };
    }

    /// <inheritdoc/>
    public XElement ReturnValueDocComment { get; internal set; } = XmlDocElements.EmptySummary;

    /// <inheritdoc/>
    public ITypeNameData ReturnType => invokeMethod.ReturnType;

    /// <inheritdoc/>
    public IMethodData InvokeMethod
    {
        get
        {
            // copy doc comments to the method
            // (no need to copy param comments, because they're shared)
            invokeMethod.SummaryDocComment = SummaryDocComment;
            invokeMethod.ReturnValueDocComment = ReturnValueDocComment;

            return invokeMethod;
        }
    }

    /// <summary>
    /// List of delegate method parameters, indexed by their position.
    /// </summary>
    public IReadOnlyList<ParameterData> Parameters => invokeMethod.Parameters;

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IDelegateTypeData.Parameters => Parameters;

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> ITypeDeclaration.TypeParameterDeclarations =>
        TypeParameterDeclarations.Values.OrderBy(t => t.Index).ToList();

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> Exceptions { get; internal set; } = [];

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; }
}
