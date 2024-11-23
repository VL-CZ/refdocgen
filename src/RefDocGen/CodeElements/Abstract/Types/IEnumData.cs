using RefDocGen.CodeElements.Abstract.Members;
using System.Xml.Linq;

namespace RefDocGen.CodeElements.Abstract.Types;

#pragma warning disable CA1716

/// <summary>
/// Represents data of an enum.
/// </summary>
public interface IEnumData
{
    /// <summary>
    /// <see cref="Type"/> object representing the type.
    /// </summary>
    Type TypeObject { get; }

    /// <summary>
    /// Identifier of the enum.
    /// <para>
    /// The format is described here: <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d42-id-string-format"/>
    /// </para>
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Short name of the enum; doesn't include its namespace.
    /// </summary>
    string ShortName { get; }

    /// <summary>
    /// Full name of the enum, including its namespace.
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Gets the namespace of the enum.
    /// </summary>
    string Namespace { get; }

    /// <summary>
    /// Documentation comment provided to the enum.
    /// </summary>
    XElement DocComment { get; }

    /// <summary>
    /// Collection of enum values.
    /// </summary>
    IReadOnlyList<IEnumValueData> Values { get; }
}

#pragma warning restore CA1716
