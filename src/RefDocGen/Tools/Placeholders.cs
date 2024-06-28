namespace RefDocGen.Tools;

internal enum Placeholder
{
    Abstract, Class, Const, Delegate, Enum, Event, Explicit, Extern, Implicit,
    In, Interface, Internal, Namespace, New, Null, Object, Out, Override, Params,
    Private, Protected, Public, Readonly, Ref, Sealed, Static, Struct, Virtual,
    Void, Volatile, PrivateProtected, ProtectedInternal
}

internal static class PlaceholderExtensions
{
    internal static string GetString(this Placeholder placeholder)
    {
        return placeholder switch
        {
            Placeholder.PrivateProtected => "private protected",
            Placeholder.ProtectedInternal => "internal protected",
            _ => placeholder.ToString().ToLowerInvariant()
        };
    }
}
