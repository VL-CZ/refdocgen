namespace RefDocGen.MemberData.Abstract;

public interface ITypeNameData
{
    Type Type { get; }

    string Id { get; }

    string Name { get; }

    string FullName { get; }

    bool IsGeneric { get; }

    IReadOnlyList<ITypeNameData> GenericParameters { get; }
}
