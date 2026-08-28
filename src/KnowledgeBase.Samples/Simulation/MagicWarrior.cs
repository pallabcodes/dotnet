namespace KnowledgeBase.Samples.Simulation;

/// <summary>
/// Warrior subclass that can dodge via a teleport ability. Demonstrates
/// inheritance plus composition over a strategy interface (ITeleport), rather
/// than baking the strategy into the subclass.
/// </summary>
public sealed class MagicWarrior : Warrior
{
    private readonly IRandomGenerator _rng;
    private readonly ITeleport _teleport;
    private readonly int _teleportChance;

    public MagicWarrior(
        string name,
        double health,
        double maxAttack,
        double maxBlock,
        int teleportChance,
        IRandomGenerator rng,
        ITeleport teleport)
        : base(name, health, maxAttack, maxBlock, rng)
    {
        if (teleportChance is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(teleportChance), "Teleport chance must be 0-100.");
        }

        _teleportChance = teleportChance;
        _rng = rng;
        _teleport = teleport;
    }

    public bool CanDodge => _rng.Next(0, 100) < _teleportChance;

    public string TryTeleport() => CanDodge ? _teleport.Activate() : "Teleport failed.";

    public override double Block() => CanDodge ? double.MaxValue : base.Block();
}