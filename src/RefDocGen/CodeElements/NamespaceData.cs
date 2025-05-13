using RefDocGen.CodeElements.Abstract.Types;
using RefDocGen.CodeElements.Abstract.Types.Delegate;
using RefDocGen.CodeElements.Abstract.Types.Enum;

namespace RefDocGen.CodeElements;

public record NamespaceData(string Name, IEnumerable<IObjectTypeData> ObjectTypes, IEnumerable<IDelegateTypeData> Delegates, IEnumerable<IEnumTypeData> Enums);

public record AssemblyData(string Name, IEnumerable<NamespaceData> Namespaces);
