namespace RefDocGen.ExampleLibrary.CyclicDoc;

/// <inheritdoc cref="Cycle2"/>
class Cycle1 { }

/// <inheritdoc cref="Cycle1"/>
class Cycle2 { }

/// <inheritdoc cref="Cycle1"/>
class CycleReference { }
