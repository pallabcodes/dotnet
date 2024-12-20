namespace ConsoleApp1;

public class Animal
{
    // Constant that holds a shelter name, available to all instances of Animal
    public const string SHELTER = "Derek's Home for Animals";

    // Static field to track the number of animals. It's shared by all instances of Animal
    private static int numOfAnimals;

    // Read-only field, set only once at runtime (through constructor)
    public readonly int idNum;

    // Private fields for storing the name and sound of the animal
    private string name;
    private string sound;

    // Default constructor. If no parameters are passed, it calls the next constructor with default values.
    public Animal() : this("No Name", "No Sound")
    {
    }

    // Constructor that takes only a name. If only the name is provided, the sound is set to "No Sound"
    public Animal(string name = "No Name") : this(name, "No Sound")
    {
    }

    // Constructor that takes both name and sound to initialize an Animal
    public Animal(string name, string sound)
    {
        // Call the SetName method to ensure the name is valid
        SetName(name);

        // Use the property to assign the sound (it includes validation logic)
        Sound = sound;

        // Increment the number of animals when a new one is created
        NumOfAnimals = 1;

        // Generate a random ID for the animal
        var rnd = new Random();
        idNum = rnd.Next(1, 2147483640); // Generate a random ID between 1 and 2 billion
    }

    // Property for Sound that includes validation logic
    public string Sound
    {
        // => syntax is a shorthand syntax for defining read-only properties, e.g., so it is perfect for a getter i.e. readonly 
        get => sound; // Getter: Returns the value of the private field
        set
        {
            // If the sound is longer than 10 characters, it's set to "No Sound"
            if (value.Length > 10)
            {
                sound = "No Sound";
                Console.WriteLine("Sound is too long");
            }
            else
            {
                sound = value; // Otherwise, assign the sound
            }
        }
    }

    // Automatic property with a default value, used to store the owner of the animal

    /**
     * This is what below code does
     * private string _owner = "No Owner";  // Backing field
     * public string Owner
     * { get { return _owner; } set { _owner = value; } }
     * so below code is just a shorthand so not to manually assign the _owner property and then access it through owner publicly
     */
    public string Owner { get; set; } = "No Owner";

    // Static property for tracking the number of animals
    public static int NumOfAnimals
    {
        get => numOfAnimals; // Return the current number of animals
        set => numOfAnimals += value; // Increment the total number of animals
    }

    // Method to set the name of the animal while validating it
    public void SetName(string name)
    {
        // Check if the name contains any digits, which is not allowed
        if (!name.Any(char.IsDigit))
        {
            this.name = name; // Set the name if it's valid
        }
        else
        {
            this.name = "No Name"; // Default to "No Name" if invalid
            Console.WriteLine("Name can't contain numbers");
        }
    }

    // Getter method for the animal's name
    public string GetName()
    {
        return name; // Return the current name of the animal
    }

    // A method that prints the sound the animal makes
    public void MakeSound()
    {
        Console.WriteLine("{0} says {1}", name, sound); // Print the animal's name and sound
    }
}