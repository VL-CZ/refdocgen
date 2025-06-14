using RefDocGen.ExampleLibrary.Tools.Collections;

namespace RefDocGen.ExampleLibrary;

/// <summary>
/// Abstract class representing an animal.
/// </summary>
/// <remarks>
/// This class is abstract, use inheritance.
/// </remarks>
public abstract class Animal
{
    /// <summary>
    /// Animal weight.
    /// 
    /// Note that this also applies for <see cref="Dog"/>.
    /// <br/>
    /// 
    /// See also <see cref="User.username"/>
    /// <br/>
    /// 
    /// See also <see cref="MyCollection{T}.Add"/>
    /// <br/>
    /// 
    /// See also <see cref="MyCollection{T}.IsReadOnly"/>
    /// </summary>
    /// <remarks>
    /// The weight is in kilograms (kg).
    /// </remarks>
    private int weight;

    /// <summary>
    /// Dog's owner; NULL if the dog doesn't have any owner.
    /// </summary>
    /// <exception cref="NullReferenceException">User not found.</exception>.
    /// <exception cref="InvalidOperationException">Blah blah blah.</exception>.
    protected User Owner { get; set; }

    /// <summary>
    /// Abstract method to get the animal's sound.
    /// </summary>
    internal abstract string GetSound();

    /// <summary>
    /// Static method returning the average lifespan of an animal.
    /// </summary>
    /// <param name="species">The species of the animal.</param>
    /// <returns>The average lifespan.</returns>
    public static int GetAverageLifespan(string species)
    {
        return species == "Dog" ? 13 : 10;
    }

    /// <summary>
    /// A virtual method to generate an animal profile.
    /// </summary>
    /// <param name="name">Animal's name.</param>
    /// <param name="habitat">Animal's habitat.</param>
    /// <param name="dateOfBirth">Animal's birthdate.</param>
    /// <returns>Profile of the animal as a string.</returns>
    public virtual string GenerateAnimalProfile(string name, string habitat, DateTime dateOfBirth)
    {
        return $"{name} is an animal living in {habitat}, born on {dateOfBirth.ToShortDateString()}.";
    }
}
