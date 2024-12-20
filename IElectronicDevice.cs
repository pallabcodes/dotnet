namespace ConsoleApp1;

public interface IElectronicDevice
{
    // We want each device to have these capabilities
    void On();
    void Off();
    void VolumeUp();
    void VolumeDown();
}