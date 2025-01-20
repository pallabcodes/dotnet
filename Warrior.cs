namespace ConsoleApp1;

public class Warrior
{
    // Always create a single Random instance and reuse it, or you will get the same value over and over
    private readonly Random rnd = new();

    // Constructor initializes the warrior
    public Warrior(string name = "Warrior",
        double health = 0,
        double attkMax = 0,
        double blockMax = 0)
    {
        Name = name;
        Health = health;
        AttkMax = attkMax;
        BlockMax = blockMax;
    }

    // Define the Warriors properties
    public string Name { get; set; } = "Warrior";
    public double Health { get; set; }
    public double AttkMax { get; set; }
    public double BlockMax { get; set; }

    // Generate a random atack value from 1
    // to the warriors maximum attack value
    public double Attack()
    {
        return rnd.Next(1, (int)AttkMax);
    }

    // Generate a random block value from 1 to the warriors maximum block
    public virtual double Block()
    {
        return rnd.Next(1, (int)BlockMax);
    }
}