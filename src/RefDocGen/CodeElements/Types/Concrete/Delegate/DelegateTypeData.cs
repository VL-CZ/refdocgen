using RefDocGen.CodeElements.Members.Abstract;
using RefDocGen.CodeElements.Members.Concrete;
using RefDocGen.CodeElements.Types.Abstract.Attribute;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Exception;
using RefDocGen.CodeElements.Types.Abstract.TypeName;
using RefDocGen.Tools.Xml;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Types.Concrete.Delegate;

/// <summary>
/// Class representing data of a delegate.
/// </summary>
internal class DelegateTypeData : TypeDeclaration, IDelegateTypeData
{
    /// <summary>
    /// Indicates whether the members have already been added.
    /// </summary>
    private bool membersAdded;

    /// <summary>
    /// The method used for delegate invocation (i.e. <c>Invoke</c>).
    /// </summary>
    private MethodData invokeMethod = null!; // the value is set in `AddInvokeMethod`

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateTypeData"/> class.
    /// </summary>
    /// <param name="type"><see cref="Type"/> object representing the delegate.</param>
    /// <param name="typeParameterDeclarations">Collection of the type parameters declared in this delegate; the keys represent type parameter names.</param>
    /// <param name="attributes">Collection of attributes applied to the type.</param>
    public DelegateTypeData(Type type, Dictionary<string, TypeParameterData> typeParameterDeclarations, IReadOnlyList<IAttributeData> attributes)
        : base(type, typeParameterDeclarations, attributes)
    {
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
    /// List of delegate method parameters, the keys represent its names.
    /// </summary>
    public IReadOnlyDictionary<string, ParameterData> Parameters => invokeMethod.Parameters;

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IDelegateTypeData.Parameters => Parameters.Values
        .OrderBy(p => p.Position)
        .ToList();

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> Exceptions { get; internal set; } = [];

    /// <inheritdoc/>
    internal override IReadOnlyDictionary<string, MemberData> AllMembers { get; private protected set; } = new Dictionary<string, MemberData>();

    /// <summary>
    /// Adds the <c>Invoke</c> method to the delegate.
    /// </summary>
    /// <param name="invokeMethod">The <c>Invoke</c> method to add.</param>
    /// <exception cref="InvalidOperationException">Thrown if the <c>Invoke</c> method has already been added.</exception>
    internal void AddInvokeMethod(MethodData invokeMethod)
    {
        if (membersAdded)
        {
            throw new InvalidOperationException($"The invoke method has been already added to {Id} delegate.");
        }

        this.invokeMethod = invokeMethod;

        AllMembers = new Dictionary<string, MemberData>
        {
            [InvokeMethod.Id] = (MemberData)InvokeMethod
        };

        membersAdded = true;
    }
}
