using RefDocGen.CodeElements.Types.Abstract;
using RefDocGen.CodeElements.Types.Abstract.Delegate;
using RefDocGen.CodeElements.Types.Abstract.Enum;

namespace RefDocGen.CodeElements;

public record NamespaceData(string Name, IEnumerable<IObjectTypeData> ObjectTypes, IEnumerable<IDelegateTypeData> Delegates, IEnumerable<IEnumTypeData> Enums);

public record AssemblyData(string Name, IEnumerable<NamespaceData> Namespaces);
