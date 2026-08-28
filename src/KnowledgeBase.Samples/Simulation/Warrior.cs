namespace KnowledgeBase.Samples.Simulation;

/// <summary>
/// Encapsulated combatant. Health is mutated only through behaviour
/// (<see cref="ReceiveDamage"/>); Attack/Block are hooks (virtual) and a
/// MagicWarrior overrides them, demonstrating polymorphism.
/// Randomness is injected for testability.
/// </summary>
public class Warrior
{
    private readonly IRandomGenerator _rng;

    public Warrior(string name, double health, double maxAttack, double maxBlock, IRandomGenerator rng)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(rng);

        Name = name;
        Health = health;
        MaxAttack = maxAttack;
        MaxBlock = maxBlock;
        _rng = rng;
    }

    public string Name { get; }

    public double Health { get; private set; }

    public bool IsAlive => Health > 0;

    public double MaxAttack { get; }

    public double MaxBlock { get; }

    public virtual double Attack() => _rng.Next(1, (int)MaxAttack);

    public virtual double Block() => _rng.Next(1, (int)MaxBlock);

    public void ReceiveDamage(double damage) => Health = Math.Max(0, Health - damage);
}