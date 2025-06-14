namespace RefDocGen.ExampleLibrary.Tools;

[Flags]
internal enum HarvestingSeason
{
    Spring = 1,
    Summer = 2,
    Autumn = 4,
    Winter = 8,
    All = Spring | Summer | Autumn | Winter
}
