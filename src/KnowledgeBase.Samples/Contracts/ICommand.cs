namespace KnowledgeBase.Samples.Contracts;

/// <summary>
/// Command pattern contract: an action that can be executed and undone.
/// The invoker depends only on this abstraction, not on a concrete device.
/// </summary>
public interface ICommand
{
    void Execute();

    void Undo();
}