namespace KnowledgeBase.Samples.Contracts;

/// <summary>Concrete receiver used by the Command pattern sample.</summary>
public sealed class Television : IElectronicDevice
{
    private const int MaxVolume = 100;

    public bool IsOn { get; private set; }

    public int Volume { get; private set; }

    public void On() => IsOn = true;

    public void Off() => IsOn = false;

    public void VolumeUp() => Volume = Math.Min(Volume + 1, MaxVolume);

    public void VolumeDown() => Volume = Math.Max(Volume - 1, 0);
}