using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Enum;
using RefDocGen.CodeElements.Concrete.Types;
using RefDocGen.CodeElements.Concrete.Types.Enum;

namespace RefDocGen.CodeElements;

public interface ITypeDeclarations
{
    IEnumerable<IObjectTypeData> Types { get; }

    IEnumerable<IEnumTypeData> Enums { get; }
}

internal class TypeDeclarations : ITypeDeclarations
{
    public TypeDeclarations(IReadOnlyDictionary<string, ObjectTypeData> types, IReadOnlyDictionary<string, EnumTypeData> enums)
    {
        Types = types;
        Enums = enums;
    }

    public IReadOnlyDictionary<string, ObjectTypeData> Types { get; }

    public IReadOnlyDictionary<string, EnumTypeData> Enums { get; }

    IEnumerable<IObjectTypeData> ITypeDeclarations.Types => Types.Values;

    IEnumerable<IEnumTypeData> ITypeDeclarations.Enums => Enums.Values;
}
