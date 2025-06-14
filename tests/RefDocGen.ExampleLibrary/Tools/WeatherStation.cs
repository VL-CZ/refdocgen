namespace RefDocGen.ExampleLibrary.Tools;

/// <summary>
/// Weather station class
/// </summary>
internal class WeatherStation
{
    /// <summary>
    /// Location of the weather station
    /// </summary>
    private readonly Point location;

    /// <summary>
    /// Weather station constructor
    /// </summary>
    /// <param name="location">Location of the weather station.</param>
    public WeatherStation(Point location)
    {
        OnTemperatureChange += () => { };
    }

    /// <summary>
    /// Temperature change event.
    /// </summary>
    /// <seealso cref="Action"/>
    /// <seealso cref="Action{T1, T2}"/>
    public event Action OnTemperatureChange;
}
