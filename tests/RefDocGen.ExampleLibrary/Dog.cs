namespace RefDocGen.ExampleLibrary;

/// <summary>
/// Represents a dog; i.e. a specific type of animal.
/// </summary>
/// <seealso cref="Animal"/>
public class Dog : Animal
{
    /// <summary>
    /// Average weight of the dog in kg.
    /// </summary>
    public const double AverageWeight = 5.43;

    /// <summary>
    /// Dog's breed.
    /// </summary>
    public string Breed { get; set; } = "Unknown";

    /// <summary>
    /// Override the GetSound method to return the dog's sound.
    /// </summary>
    /// <returns>The dog's sound.</returns>
    internal override string GetSound() => "Bark";

    /// <inheritdoc />
    public sealed override string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth)
    {
        return $"{name} is a Dog ({Breed}) living in {habitat}, born on {dateOfBirth.ToShortDateString()}.";
    }

    /// <inheritdoc cref="Animal.GetAverageLifespan(string)" path="/returns"/>
    /// <inheritdoc cref="Animal.GenerateAnimalProfile(string, string, DateTime)" path="//summary"/>
    public string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth, string[] keywords)
    {
        return $"{name} is a Dog ({Breed}) living in {habitat}, born on {dateOfBirth.ToShortDateString()}.";
    }

    /// <summary>
    /// Asynchronous method for barking.
    /// </summary>
    private async Task BarkAsync()
    {
        await Task.Delay(100);
        Console.WriteLine("Bark!");
    }
}
