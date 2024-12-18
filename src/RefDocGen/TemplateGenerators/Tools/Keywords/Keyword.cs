namespace RefDocGen.TemplateGenerators.Tools.Keywords;

/// <summary>
/// Enum representing various C# keywords and modifiers
/// </summary>
internal enum Keyword
{
    Abstract,
    Async,
    Base,
    Class,
    Const,
    Default,
    Delegate,
    Enum,
    Event,
    Explicit,
    Extern,
    Implicit,
    In,
    Interface,
    Internal,
    Namespace,
    New,
    Null,
    Object,
    Operator,
    Out,
    Override,
    Params,
    Private,
    Protected,
    Public,
    Readonly,
    Record,
    Ref,
    Required,
    Sealed,
    Static,
    Struct,
    Virtual,
    Void,
    Volatile,
    Where,
    PrivateProtected,
    ProtectedInternal
}

/// <summary>
/// A class containing extension methods for <see cref="Keyword"/> enum.
/// </summary>
internal static class KeywordExtensions
{
    /// <summary>
    /// Get string representation of the keyword.
    /// </summary>
    /// <param name="keyword">Keyword to convert.</param>
    /// <returns>String representation (in C# style) of the keyword.</returns>
    internal static string GetString(this Keyword keyword)
    {
        return keyword switch
        {
            Keyword.PrivateProtected => "private protected",
            Keyword.ProtectedInternal => "internal protected",
            _ => keyword.ToString().ToLowerInvariant()
        };
    }
}
