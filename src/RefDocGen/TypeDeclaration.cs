using RefDocGen.CodeElements.Concrete.Types;

namespace RefDocGen;

internal class TypeDeclarations
{
    public TypeDeclarations(IReadOnlyDictionary<string, TypeData> types, IReadOnlyDictionary<string, EnumData> enums)
    {
        Types = types;
        Enums = enums;
    }

    public IReadOnlyDictionary<string, TypeData> Types { get; }

    public IReadOnlyDictionary<string, EnumData> Enums { get; }
}
