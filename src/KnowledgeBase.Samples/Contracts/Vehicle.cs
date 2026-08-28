namespace KnowledgeBase.Samples.Contracts;

/// <summary>
/// Interface implementation. State is encapsulated and mutated only through
/// behaviour (<see cref="Move"/>/<see cref="Stop"/>); the type performs no I/O,
/// which keeps it trivially testable.
/// </summary>
public sealed class Vehicle : IDrivable
{
    public Vehicle(string brand, int wheels, double maxSpeed)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);
        if (wheels <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(wheels), "A vehicle must have at least one wheel.");
        }

        if (maxSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSpeed), "Max speed must be positive.");
        }

        Brand = brand;
        Wheels = wheels;
        MaxSpeed = maxSpeed;
    }

    public string Brand { get; }

    public int Wheels { get; set; }

    public double Speed { get; set; }

    public double MaxSpeed { get; }

    public void Move() => Speed = MaxSpeed;

    public void Stop() => Speed = 0;

    public override string ToString() => $"{Brand} at {Speed} mph ({Wheels} wheels)";
}