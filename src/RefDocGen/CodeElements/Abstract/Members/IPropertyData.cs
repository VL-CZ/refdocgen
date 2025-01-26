using System.Reflection;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Members;

/// <summary>
/// Represents data of a property.
/// </summary>
public interface IPropertyData : IValueMemberData, ICallableMemberData
{
    /// <summary>
    /// Gets the <see cref="System.Reflection.PropertyInfo"/> object representing the property.
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Getter method represented as a <see cref="IMethodData"/> object, or <c>null</c> if no getter exists.
    /// </summary>
    IMethodData? Getter { get; }

    /// <summary>
    /// Setter method represented as a <see cref="IMethodData"/> object, or <c>null</c> if no setter exists.
    /// </summary>
    IMethodData? Setter { get; }

    /// <summary>
    /// Gets the declared accessors (getter and setter) of the property represented as <see cref="IMethodData"/> objects.
    /// </summary>
    IEnumerable<IMethodData> Accessors { get; }

    /// <summary>
    /// Gets the access modifier of the getter, or <c>null</c> if no getter exists.
    /// </summary>
    AccessModifier? GetterAccessModifier { get; }

    /// <summary>
    /// Gets the access modifier of the setter, or <c>null</c> if no setter exists.
    /// </summary>
    AccessModifier? SetterAccessModifier { get; }

    /// <summary>
    /// 'value' doc comment for the property.
    /// </summary>
    XElement ValueDocComment { get; }
}
