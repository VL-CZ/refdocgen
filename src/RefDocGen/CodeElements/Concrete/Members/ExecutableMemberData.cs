using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Tools;
using RefDocGen.Tools;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RefDocGen.CodeElements.Concrete.Members;

/// <summary>
/// Class representing data of an executable member (i.e. method or a constructor).
/// Note that properties are excluded from this definition.
/// </summary>
internal abstract class ExecutableMemberData : MemberData, IExecutableMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="ExecutableMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    /// <param name="declaredTypeParameters">Collection of type parameters declared in the containing type; keys represent type parameter names.</param>
    protected ExecutableMemberData(MethodBase methodBase, IReadOnlyDictionary<string, TypeParameterData> declaredTypeParameters) : base(methodBase)
    {
        this.methodBase = methodBase;

        // add type parameters
        TypeParameterDeclarations = !IsConstructor()
            ? methodBase.GetGenericArguments()
                .Select((ga, i) => new TypeParameterData(ga, i, CodeElementKind.Member))
                .ToDictionary(t => t.Name)
            : [];

        // add the dicitonaries
        var allParams = declaredTypeParameters
            .Merge(TypeParameterDeclarations);

        // add parameters
        Parameters = methodBase.GetParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p, allParams))
            .ToArray();
    }

    /// <inheritdoc/>
    public override string Id => MemberId.Of(this);

    /// <inheritdoc/>
    public abstract bool OverridesAnotherMember { get; }

    /// <inheritdoc/>
    public override AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(methodBase.IsPrivate, methodBase.IsFamily,
        methodBase.IsAssembly, methodBase.IsPublic, methodBase.IsFamilyAndAssembly, methodBase.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public override bool IsStatic => methodBase.IsStatic;

    /// <inheritdoc/>
    public bool IsOverridable => methodBase.IsVirtual && !methodBase.IsFinal;

    /// <inheritdoc/>
    public bool IsAbstract => methodBase.IsAbstract;

    /// <inheritdoc/>
    public bool IsFinal => methodBase.IsFinal;

    /// <inheritdoc/>
    public bool IsAsync => methodBase.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null;

    /// <inheritdoc/>
    public bool IsSealed => OverridesAnotherMember && IsFinal;

    /// <inheritdoc/>
    public bool IsVirtual => methodBase.IsVirtual;

    /// <summary>
    /// Array of method parameters, ordered by their position.
    /// </summary>
    internal IReadOnlyList<ParameterData> Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters;

    /// <inheritdoc/>
    public IEnumerable<IExceptionDocumentation> Exceptions { get; internal set; } = [];

    /// <summary>
    /// Collection of type parameters declared in the member; the keys represent type parameter names.
    /// </summary>
    internal IReadOnlyDictionary<string, TypeParameterData> TypeParameterDeclarations { get; } = new Dictionary<string, TypeParameterData>();

    /// <inheritdoc/>
    IReadOnlyList<ITypeParameterData> IExecutableMemberData.TypeParameters => TypeParameterDeclarations.Values
        .OrderBy(t => t.Index)
        .ToList();

    /// <inheritdoc/>
    public virtual bool IsExplicitImplementation => Name.Contains('.');

    /// <summary>
    /// Checks if the member represents a constructor.
    /// </summary>
    /// <returns><c>true</c> if the member represents a constructor, <c>false</c> otherwise.</returns>
    protected abstract bool IsConstructor();
}
