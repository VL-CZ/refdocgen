using RefDocGen.CodeElements.Abstract.Types;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of an executable type member, i.e. method, constructor or an indexer.
/// <para>
/// Note that properties are excluded from this definition.
/// </para>
/// </summary>
public interface IExecutableMemberData : ICallableMemberData
{
    /// <summary>
    /// Readonly list of the member parameters, indexed by their position.
    /// </summary>
    IReadOnlyList<IParameterData> Parameters { get; }

    /// <summary>
    /// Collection of generic type parameters declared in the member.
    /// </summary>
    IReadOnlyList<ITypeParameterData> TypeParameters { get; }

    /// <summary>
    /// Checks if the member represents a constructor.
    /// </summary>
    /// <returns><c>true</c> if the member represents a constructor, <c>false</c> otherwise.</returns>
    bool IsConstructor { get; }
}
