namespace ConsoleApp1;

public class Animal
{
    // Constant that holds a shelter name, available to all instances of Animal
    public const string SHELTER = "Derek's Home for Animals";

    // Static field to track the number of animals. It's shared by all instances of Animal
    private static int numOfAnimals;

    // Read-only field, set only once at runtime (through constructor)
    public readonly int idNum;


    /**
     * This means that whenever an instance of the Animal class is created, then an AnimalIDInfo object will also be created and associated with that particular instance.
     * This is an example of composition or "Has-a" relationship, where an Animal "has-a" AnimalIDInfo object.
     * Every time you create a new Animal, the animalIDInfo member (which holds the AnimalIDInfo object) will be instantiated automatically.
     * ---------------------------------------------------------------------------------------------------------------------------------
     * The AnimalIDInfo object will not exist or instantiate independently outside the context of the Animal instance, but will be directly tied to the specific instance of Animal.
     * ---------------------------------------------------------------------------------------------------------------------------------
     * summary: Animal "has-a" AnimalIDInfo relationship, where Animal creates and owns an AnimalIDInfo instance or AnimalIDInfo can't be instantiated outside Animal
     * Animal "has-a" AnimalIDInfo relationship (composition), while inheritance represents an "is-a" relationship.
     */
    protected AnimalIDInfo animalIDInfo = new(); // same as new AnimalDIInfo();

    // Private fields for storing the name and sound of the animal
    private string name;
    protected string sound;

    // Default constructor. If no parameters are passed, it calls the next constructor with default values.
    // N.B: this takes 2 arguments and then this calls the constructor that takes 2 arguments and then if any constructor has 2 params and no value provided, then the default value will be provided from here
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
        // Use the property to assign the name (it includes validation logic)
        Name = name;

        // Use the property to assign the sound (it includes validation logic)
        Sound = sound;

        // Increment the number of animals when a new one is created
        NumOfAnimals = 1;

        // Generate a random ID for the animal
        var rnd = new Random();
        idNum = rnd.Next(1, 2147483640); // Generate a random ID between 1 and 2 billion
    }

    // This is how to write a getter a setter together
    public string Name
    {
        get => name; // Getter: Returns the value of the private field
        set
        {
            if (!value.Any(char.IsDigit))
            {
                name = value; // Assign the value if it's valid
            }
            else
            {
                name = "No Name"; // Default to "No Name" if invalid
                Console.WriteLine("Name can't contain numbers");
            }
        }
    }

    // Property for Sound that includes validation logic
    public string Sound
    {
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
    public string Owner { get; set; } = "No Owner";

    // Static property for tracking the number of animals
    public static int NumOfAnimals
    {
        get => numOfAnimals; // Return the current number of animals
        set => numOfAnimals += value; // Increment the total number of animals
    }

    // Method to set the animal's ID information
    public void SetAnimalIDInfo(int idNum, string owner)
    {
        animalIDInfo.IDNum = idNum;
        animalIDInfo.Owner = owner;
    }

    // Method to display the animal's ID information
    public void GetAnimalIDInfo()
    {
        Console.WriteLine($"{Name} has the ID of {animalIDInfo.IDNum} and is owned by {animalIDInfo.Owner}");
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
        return name;
        // Return the current name of the animal
    }

    // Added virtual so that this method cam be overridden by subclasses e.g. Dog
    public virtual void MakeSound()
    {
        Console.WriteLine($"{Name} says {Sound}"); // Print the animal's name and sound
    }

    public override string ToString()
    {
        return Name;
        // ToString returns the animal's name
    }

    // Inner class for health-related calculations
    public class AnimalHealth
    {
        public bool HealthyWeight(double height, double weight)
        {
            // Check if the weight-to-height ratio is within a healthy range
            var calc = height / weight;
            return calc is >= .18 and <= .27;
        }
    }
}