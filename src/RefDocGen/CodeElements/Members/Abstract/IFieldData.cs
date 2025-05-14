using System.Reflection;

namespace RefDocGen.CodeElements.Members.Abstract;

/// <summary>
/// Represents data of a field.
/// </summary>
public interface IFieldData : IValueMemberData
{
    /// <summary>
    /// <see cref="System.Reflection.FieldInfo"/> object representing the field.
    /// </summary>
    FieldInfo FieldInfo { get; }
}
