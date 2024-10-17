using RefDocGen.DocExtraction;
using RefDocGen.MemberData.Interfaces;
using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.MemberData;

/// <summary>
/// Represents data of a constructor.
/// </summary>
public record ConstructorData : ICallableMemberData
{
    /// <summary>
    /// Create new <see cref="ConstructorData"/> instance.
    /// </summary>
    /// <param name="constructorInfo"><see cref="System.Reflection.ConstructorInfo"/> object representing the field.</param>
    public ConstructorData(ConstructorInfo constructorInfo)
    {
        ConstructorInfo = constructorInfo;
        Parameters = ConstructorInfo.GetParameters().OrderBy(p => p.Position).Select(p => new MethodParameterData(p)).ToArray();
    }

    /// <summary>
    /// <see cref="System.Reflection.ConstructorInfo"/> object representing the field.
    /// </summary>
    public ConstructorInfo ConstructorInfo { get; }

    /// <inheritdoc/>
    public string Name => string.Empty;

    /// <inheritdoc/>
    public AccessModifier AccessModifier => AccessModifierExtensions.GetAccessModifier(ConstructorInfo.IsPrivate, ConstructorInfo.IsFamily,
        ConstructorInfo.IsAssembly, ConstructorInfo.IsPublic, ConstructorInfo.IsFamilyAndAssembly, ConstructorInfo.IsFamilyOrAssembly);

    /// <inheritdoc/>
    public bool IsStatic => ConstructorInfo.IsStatic;

    /// <inheritdoc/>
    public bool IsOverridable => false;

    /// <inheritdoc/>
    public bool OverridesAnotherMember => false;

    /// <inheritdoc/>
    public bool IsAbstract => false;

    /// <inheritdoc/>
    public bool IsFinal => false;

    /// <inheritdoc/>
    public bool IsSealed => false;

    /// <inheritdoc/>
    public bool IsVirtual => false;

    /// <inheritdoc/>
    public bool IsAsync => false;

    /// <summary>
    /// XML doc comment for the constructor.
    /// </summary>
    public XElement DocComment { get; init; } = DocCommentTools.EmptySummaryNode;

    /// <summary>
    /// Gets the constuctor parameters represented as <see cref="MethodParameterData"/> objects, ordered by their position.
    /// </summary>
    public MethodParameterData[] Parameters { get; }

}

