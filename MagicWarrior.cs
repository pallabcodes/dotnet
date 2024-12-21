namespace ConsoleApp1;

// Now we will inherit from Warrior and add on the additonal ability of teleporting using our Teleports interface
public class MagicWarrior : Warrior
{
    // The bigger the number, the more likely the chance of successfully teleporting (100 Max Value)
    private readonly int teleportChance;

    // Add interface functionality
    private readonly CanTeleport teleportType = new();

    public MagicWarrior(string name = "Warrior",
        double health = 0,
        double attkMax = 0,
        double blockMax = 0,
        int teleportChance = 0)
        : base(name, health, attkMax, blockMax)
    {
        this.teleportChance = teleportChance;
    }

    // We'll inherit all properties and methods in the Warrior class, but we'll override the block
    public override double Block()
    {
        // Generate a random value from 1 to 100
        var rnd = new Random();
        var rndDodge = rnd.Next(1, 100);

        // Decide if teleport works based on percent assigned to teleportChance
        if (rndDodge < teleportChance)
        {
            Console.WriteLine($"{Name} {teleportType.teleport()}");
            return 10000;
        }

        // Call the block method in the super class
        return base.Block();
    }
}