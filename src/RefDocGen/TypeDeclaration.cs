using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen;

public interface ITypeDeclarations
{
    IEnumerable<ITypeData> Types { get; }

    IEnumerable<IEnumData> Enums { get; }
}

internal class TypeDeclarations : ITypeDeclarations
{
    public TypeDeclarations(IReadOnlyDictionary<string, TypeData> types, IReadOnlyDictionary<string, EnumData> enums)
    {
        Types = types;
        Enums = enums;
    }

    public IReadOnlyDictionary<string, TypeData> Types { get; }

    public IReadOnlyDictionary<string, EnumData> Enums { get; }

    IEnumerable<ITypeData> ITypeDeclarations.Types => Types.Values;

    IEnumerable<IEnumData> ITypeDeclarations.Enums => Enums.Values;
}
