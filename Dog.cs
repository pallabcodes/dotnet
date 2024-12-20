namespace ConsoleApp1;

// The Dog class inherits from Animal class and overrides methods and adds new properties.
internal class Dog : Animal
{
    // Create a constructor that has the base constructor initialize everything except Sound2 (base is the same as calling supper from js)
    public Dog(string name = "No Name", string sound = "No Sound", string sound2 = "No Sound 2") : base(name, sound)
    {
        Sound2 = sound2;
    }

    // Additional property for the second sound
    public string Sound2 { get; set; } = "Grrrrr";

    // Add override so that the correct method is called when a Dog calls the `MakeSound` method
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says {Sound} and {Sound2}");
    }
}