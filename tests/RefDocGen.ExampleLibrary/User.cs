using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace RefDocGen.ExampleLibrary;

/// <summary>
/// Class representing an user of our app.
/// </summary>
[Serializable]
[JsonSerializable(typeof(User), GenerationMode = JsonSourceGenerationMode.Serialization)]
public class User
{
    /// <summary>
    /// Maximum age of the user.
    /// </summary>
    private const int MaxAge = 150;

    /// <summary>
    /// Name of the user. <c>var name = user.username</c>
    ///
    /// <code>
    /// var name = user.username;
    /// var userValidator = new UserValidator();
    ///
    /// var isValid = userValidator.Validate(name);
    /// Console.WriteLine($"Valid: {isValid}");
    /// </code>
    /// <para>
    /// Paragraph
    /// </para>
    /// <list type="bullet">
    ///   <item>ABC</item>
    ///   <item>
    ///     <term>ddd</term>
    ///     <description>DEF</description>
    ///   </item>
    /// </list>
    /// <list type="number">
    ///   <item>First</item>
    ///   <item>Second</item>
    /// </list>
    ///
    /// <list type="table">
    /// <listheader>
    /// <term>name</term>
    /// <term>age</term>
    /// </listheader>
    /// <item>
    /// <term>John Smith</term>
    /// <description>10</description>
    /// </item>
    /// <item>
    /// <term>Jane Anderson</term>
    /// <description>15</description>
    /// </item>
    /// </list>
    /// <see href="http://www.google.com">Google</see>
    /// <see langword="true"/>
    /// </summary>
    /// <seealso href="http://www.google.com"/>
    /// <seealso cref="MaxAge">max age constant</seealso>
    /// <seealso cref="System.Reflection.FieldInfo.IsLiteral" />
    /// <seealso cref="RefDocGen.TestingLibrary.Tools.Point"/>
    /// <seealso cref="notFound"/>
    protected readonly string username;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public User()
    {
        username = "DefaultUser";
    }

    /// <summary>
    /// Initializes a new user using the provided <paramref name="username"/> and <paramref name="age"/>.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="age">Age of the user.</param>
    public User(string username, int age)
    {
        this.username = username;
        Age = age;
    }

    /// <summary>
    /// Gets the age of the user (nullable).
    /// </summary>
    /// <value>The age of the user.</value>
    public int? Age { get; }

    /// <summary>
    /// First name of the user.
    /// </summary>
    public string FirstName { get; internal set; }

    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string LastName { get; init; }

    /// <summary>
    /// List of owned animals.
    /// </summary>
    public List<Animal> Animals { get; } = [];

    /// <summary>
    /// Checks if the user is adult.
    /// </summary>
    /// <returns>True if adult, false otherwise.</returns>
    public bool IsAdult()
    {
        return Age >= 18;
    }

    /// <summary>
    /// Add animals to the user.
    /// </summary>
    /// <param name="animals">Animals to add.</param>
    public void AddAnimals(params Animal[] animals)
    {
        Animals.AddRange(animals);
    }

    /// <summary>
    /// A method with ref, in, and out parameters for testing purposes.
    /// </summary>
    /// <param name="inValue">An input value.</param>
    /// <param name="refValue">A reference value.</param>
    /// <param name="outValue">An output value.</param>
    internal static void ProcessValues(in int inValue, ref int refValue, string s1, out int outValue, double d2 = User.MaxAge)
    {
        outValue = inValue + refValue;
    }

    /// <summary>
    /// Print user's profile.
    /// </summary>
    public void PrintProfile()
    {
    }

    /// <summary>
    /// Print user's profile in the given format
    /// </summary>
    /// <param name="format">Format used for printing the profile.</param>
    [Obsolete]
    public void PrintProfile([NotNull] string format = "json")
    {

    }

    /// <summary>
    /// Print user's profile in the given format
    /// </summary>
    /// <param name="format">Format used for printing the profile.</param>
    /// <param name="keywords">List of keywords.</param>
    public void PrintProfile(string format, List<string>? keywords = null)
    {

    }

    /// <summary>
    /// Get dictionary of animals, whose keys are animal type and values are the animals of given type.
    /// </summary>
    /// <returns>Dictionary of animals, indexed by their type.</returns>
    public Dictionary<string, List<Animal>> GetAnimalsByType()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Make the animals user-owned.
    /// </summary>
    /// <param name="animals">Animals to add. Key: animal type, Value: list of animals of the given type.</param>
    public void AddAnimalsByType(Dictionary<string, List<Animal>> animals)
    {

    }
}
