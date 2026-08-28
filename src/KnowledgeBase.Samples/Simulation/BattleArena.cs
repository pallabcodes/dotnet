namespace KnowledgeBase.Samples.Simulation;

/// <summary>The result of a single attacking turn.</summary>
public sealed record CombatRound(
    string Attacker,
    string Defender,
    double DamageDealt,
    double DefenderHealthRemaining);

/// <summary>
/// Simulates a turn-based fight. Kept pure: it returns a transcript of rounds
/// instead of printing, so outcomes are verifiable and the caller decides how
/// to present them.
/// </summary>
public static class BattleArena
{
    public static IReadOnlyList<CombatRound> Fight(Warrior contenderA, Warrior contenderB, int maxRounds = 100)
    {
        ArgumentNullException.ThrowIfNull(contenderA);
        ArgumentNullException.ThrowIfNull(contenderB);

        var transcript = new List<CombatRound>();

        while (contenderA.IsAlive && contenderB.IsAlive && transcript.Count < maxRounds)
        {
            transcript.Add(ResolveRound(contenderA, contenderB));

            if (!contenderA.IsAlive || !contenderB.IsAlive || transcript.Count >= maxRounds)
            {
                break;
            }

            transcript.Add(ResolveRound(contenderB, contenderA));
        }

        return transcript;
    }

    private static CombatRound ResolveRound(Warrior attacker, Warrior defender)
    {
        var damage = Math.Max(0, attacker.Attack() - defender.Block());
        defender.ReceiveDamage(damage);
        return new CombatRound(attacker.Name, defender.Name, damage, defender.Health);
    }
}