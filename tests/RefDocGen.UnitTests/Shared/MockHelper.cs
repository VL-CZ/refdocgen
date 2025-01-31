using NSubstitute;

namespace RefDocGen.UnitTests.Shared;

/// <summary>
/// Class containing helper methods for mocking types and members.
/// </summary>
internal static class MockHelper
{
    /// <summary>
    /// Mock <see cref="Type"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="typeNamespace">Namespace containing the type.</param>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="genericParameters">Generic parameters of the type.</param>
    /// <returns>Mocked <see cref="Type"/> instance.</returns>
    internal static Type MockType(string typeNamespace, string typeName, Type[] genericParameters)
    {
        var type = Substitute.For<Type>();

        type.Name.Returns(typeName);
        type.Namespace.Returns(typeNamespace);
        type.GetGenericArguments().Returns(genericParameters);
        type.IsGenericType.Returns(genericParameters.Length > 0);

        return type;
    }

    /// <summary>
    /// Mock non-generic <see cref="Type"/> instance and initialize it with the provided data.
    /// </summary>
    /// <param name="typeNamespace">Namespace of the type</param>
    /// <param name="typeName">Name of the type.</param>
    /// <returns>Mocked <see cref="Type"/> instance.</returns>
    internal static Type MockNonGenericType(string typeNamespace, string typeName)
    {
        return MockType(typeNamespace, typeName, []);
    }
}
