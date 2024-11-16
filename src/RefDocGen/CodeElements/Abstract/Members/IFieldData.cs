using System.Reflection;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a field.
/// </summary>
public interface IFieldData : IVariableMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the field.
    /// </summary>
    FieldInfo FieldInfo { get; }
}
