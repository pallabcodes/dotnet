namespace KnowledgeBase.Samples.Contracts;

/// <summary>
/// Command: wraps an action (and its inverse) behind a single object.
/// The invoker has no knowledge of the receiver; it only asks the command
/// to Execute or Undo. This is what makes actions composable and undoable.
/// </summary>
public sealed class PowerButton : ICommand
{
    private readonly IElectronicDevice _device;

    public PowerButton(IElectronicDevice device) => _device = device;

    public void Execute() => _device.On();

    public void Undo() => _device.Off();
}