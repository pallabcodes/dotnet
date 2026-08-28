namespace KnowledgeBase.Samples.Simulation;

/// <summary>
/// The turn-based combat simulation keeps randomness behind an abstraction so
/// the domain logic is deterministic under test (a scripted generator can be
/// injected) — the same reason real systems inject time and randomness.
/// </summary>
public interface IRandomGenerator
{
    int Next(int minInclusive, int maxExclusive);
}

/// <summary>Default random generator; a single Random instance is reused.</summary>
public sealed class RandomGenerator : IRandomGenerator
{
    private readonly Random _rng = new();

    public int Next(int minInclusive, int maxExclusive) => _rng.Next(minInclusive, maxExclusive);
}

/// <summary>Strategy contract for a teleport ability.</summary>
public interface ITeleport
{
    string Activate();
}

/// <summary>A teleport that works.</summary>
public sealed class CanTeleport : ITeleport
{
    public string Activate() => "Teleports away!";
}

/// <summary>A teleport that does not work.</summary>
public sealed class CantTeleport : ITeleport
{
    public string Activate() => "Fails to teleport.";
}