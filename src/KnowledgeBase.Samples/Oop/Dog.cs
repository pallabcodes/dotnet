namespace KnowledgeBase.Samples.Oop;

/// <summary>Inheritance: extends Animal and overrides a virtual member.</summary>
public sealed class Dog : Animal
{
    public Dog(string name, string sound, string secondSound)
        : base(name, sound)
    {
        SecondSound = secondSound;
    }

    public string SecondSound { get; set; }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says {Sound} and {SecondSound}");
    }
}