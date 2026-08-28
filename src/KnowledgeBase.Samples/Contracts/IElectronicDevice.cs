namespace KnowledgeBase.Samples.Contracts;

/// <summary>
/// Contract every electronic device honours. Observable state (IsOn, Volume)
/// is exposed so behaviour can be asserted without capturing console output.
/// </summary>
public interface IElectronicDevice
{
    bool IsOn { get; }

    int Volume { get; }

    void On();

    void Off();

    void VolumeUp();

    void VolumeDown();
}