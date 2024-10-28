using RefDocGen.MemberData.Abstract;
using RefDocGen.Tools.Xml;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace RefDocGen.MemberData.Concrete;

/// <summary>
/// Represents data of an executable member (i.e. method or a constructor).
/// </summary>
internal abstract class ExecutableMemberData : IExecutableMemberData
{
    /// <summary>
    /// <see cref="MethodBase"/> object representing the member.
    /// </summary>
    private readonly MethodBase methodBase;

    /// <summary>
    /// Create new <see cref="ExecutableMemberData"/> instance.
    /// </summary>
    /// <param name="methodBase"><see cref="MethodBase"/> object representing the member.</param>
    protected ExecutableMemberData(MethodBase methodBase)
    {
        this.methodBase = methodBase;

        // add parameters
        Parameters = methodBase.GetParameters()
            .OrderBy(p => p.Position)
            .Select(p => new ParameterData(p))
            .ToArray();
    }

    /// <inheritdoc/>
    public string Id
    {
        get
        {
            if (Parameters.Length == 0) // no params -> return the Name
            {
                return Name;
            }
            else
            {
                // Get the parameters in the format: System.String, System.Int32, etc.
                var parameterNames = Parameters.Select(
                            p => p.ParameterInfo.ParameterType.FullName
                                ?.Replace('&', '@') // denotes params passed by reference
                        );

                return Name + "(" + string.Join(",", parameterNames) + ")";
            }
        }
    }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract bool OverridesAnotherMember { get; }

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(methodBase.IsPrivate, methodBase.IsFamily,
        methodBase.IsAssembly, methodBase.IsPublic, methodBase.IsFamilyAndAssembly, methodBase.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => methodBase.IsStatic;

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

    /// <inheritdoc/>
    public XElement DocComment { get; internal set; } = XmlDocElementFactory.EmptySummary;

    /// <summary>
    /// Array of method parameters, ordered by their position.
    /// </summary>
    public ParameterData[] Parameters { get; }

    /// <inheritdoc/>
    IReadOnlyList<IParameterData> IExecutableMemberData.Parameters => Parameters;
}
