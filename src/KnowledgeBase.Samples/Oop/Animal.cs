namespace KnowledgeBase.Samples.Oop;

/// <summary>
/// Base class demonstrating the core OOP building blocks:
/// encapsulation (private state behind validated properties),
/// composition ("has-a" AnimalIDInfo),
/// polymorphism hooks (virtual members),
/// and static/process-wide state.
/// </summary>
public class Animal
{
    // One shared Random: creating one per instance seeds identically when
    // instances are constructed within the same tick.
    private static readonly Random Rng = new();
    private static int _numOfAnimals;

    private string _name = string.Empty;

    public Animal(string name, string sound)
    {
        Name = name;
        Sound = sound;
        IdNumber = Rng.Next(1, int.MaxValue);
        Interlocked.Increment(ref _numOfAnimals);
    }

    /// <summary>Number of Animal instances created in this process.</summary>
    public static int NumOfAnimals => Volatile.Read(ref _numOfAnimals);

    /// <summary>Random integer identifier; assigned once at construction.</summary>
    public int IdNumber { get; }

    /// <summary>
    /// Validated property: an invalid value fails fast with an exception
    /// rather than being silently coerced to a fallback.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            if (value.Any(char.IsDigit))
            {
                throw new ArgumentException("Names must not contain digits.", nameof(value));
            }

            _name = value;
        }
    }

    public virtual string Sound { get; protected set; }

    /// <summary>Composition: every Animal owns its registration details.</summary>
    public AnimalIDInfo IdInfo { get; set; } = new(0, "Unregistered");

    public virtual void MakeSound()
    {
        Console.WriteLine($"{Name} says {Sound}");
    }

    public override string ToString() => Name;

    /// <summary>
    /// Nested helper: logic that belongs to the domain of its enclosing type
    /// can live beside it without leaking into the public surface.
    /// </summary>
    public sealed class AnimalHealth
    {
        public bool HealthyWeight(double height, double weight) => height / weight is >= 0.18 and <= 0.27;
    }
}