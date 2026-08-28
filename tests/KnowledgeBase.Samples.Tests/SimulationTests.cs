using KnowledgeBase.Samples.Simulation;

namespace KnowledgeBase.Samples.Tests;

public sealed class SimulationTests
{
    [Fact]
    public void Warrior_attack_and_block_come_from_the_rng()
    {
        var warrior = new Warrior("Test", 100, 100, 100, new ScriptedRandom([42]));

        Assert.Equal(42, warrior.Attack());
        Assert.Equal(42, warrior.Block());
    }

    [Fact]
    public void Warrior_health_caps_at_zero_when_over_damaged()
    {
        var warrior = new Warrior("Test", 10, 0, 0, new ScriptedRandom());
        warrior.ReceiveDamage(200);

        Assert.Equal(0, warrior.Health);
        Assert.False(warrior.IsAlive);
    }

    [Fact]
    public void Fight_ends_with_a_victor_when_one_side_is_outgunned()
    {
        // A always attacks for 20 and blocks 20; B attacks/blocks for 0.
        var a = new Warrior("A", 100, 100, 100, new ScriptedRandom([20]));
        var b = new Warrior("B", 40, 1, 1, new ScriptedRandom([0]));

        var fight = BattleArena.Fight(a, b);

        Assert.True(a.IsAlive);
        Assert.False(b.IsAlive);
        Assert.Equal(100, a.Health); // B never landed a hit

        // Rounds: A deals 20 (B at 20), B deals 0, A deals 20 (B at 0).
        Assert.Equal(3, fight.Count);
        Assert.Equal("B", fight.Last().Defender);
        Assert.Equal(0, fight.Last().DefenderHealthRemaining);
    }

    [Fact]
    public void Fight_alternates_turns_and_stops_when_the_defender_dies()
    {
        var a = new Warrior("A", 50, 100, 10, new ScriptedRandom([20]));
        var b = new Warrior("B", 40, 100, 10, new ScriptedRandom([0]));

        var fight = BattleArena.Fight(a, b);

        Assert.False(b.IsAlive);
        Assert.Equal(new[] { "A", "B", "A" }, fight.Select(round => round.Attacker));
        Assert.Equal(new[] { "B", "A", "B" }, fight.Select(round => round.Defender));
        Assert.Equal(0, fight.Last().DefenderHealthRemaining);
    }

    [Fact]
    public void Fight_is_bounded_when_no_one_can_score()
    {
        var a = new Warrior("A", 100, 0, 0, new ScriptedRandom([0]));
        var b = new Warrior("B", 100, 0, 0, new ScriptedRandom([0]));

        var fight = BattleArena.Fight(a, b, maxRounds: 25);

        Assert.Equal(25, fight.Count);
        Assert.True(a.IsAlive);
        Assert.True(b.IsAlive);
    }

    [Fact]
    public void MagicWarrior_dodges_when_the_roll_beats_the_chance()
    {
        var rng = new ScriptedRandom([5]); // 5 < 100 -> dodge
        var mage = new MagicWarrior("Loki", 75, 20, 10, 100, rng, new CanTeleport());

        Assert.True(mage.CanDodge);
        Assert.Equal(double.MaxValue, mage.Block());
        Assert.Equal("Teleports away!", mage.TryTeleport());
    }

    [Fact]
    public void MagicWarrior_with_zero_chance_never_dodges()
    {
        var rng = new ScriptedRandom([99]); // 99 < 0 is false
        var mage = new MagicWarrior("Loki", 75, 20, 10, 0, rng, new CantTeleport());

        Assert.False(mage.CanDodge);
        Assert.Equal("Teleport failed.", mage.TryTeleport());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void MagicWarrior_rejects_out_of_range_teleport_chance(int chance)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MagicWarrior("Loki", 75, 20, 10, chance, new ScriptedRandom(), new CanTeleport()));
    }

    [Fact]
    public void Dodging_warrior_takes_no_damage_from_a_hit()
    {
        var attacker = new Warrior("A", 50, 100, 0, new ScriptedRandom([20]));
        var mage = new MagicWarrior("Mage", 50, 20, 10, 100, new ScriptedRandom([5]), new CanTeleport());

        var damage = Math.Max(0, attacker.Attack() - mage.Block());
        mage.ReceiveDamage(damage);

        Assert.Equal(0, damage);
        Assert.Equal(50, mage.Health);
        Assert.True(mage.IsAlive);
    }

    private sealed class ScriptedRandom : IRandomGenerator
    {
        private readonly int[] _values;
        private int _index;

        public ScriptedRandom(params int[] values) => _values = values.Length > 0 ? values : [0];

        public int Next(int minInclusive, int maxExclusive)
        {
            var value = _values[_index % _values.Length];
            _index++;
            return value;
        }
    }
}