namespace KnowledgeBase.Samples.Contracts;

/// <summary>
/// A trivial factory: clients depend on the IElectronicDevice abstraction
/// and never construct a concrete Television themselves.
/// </summary>
public static class TvRemote
{
    public static IElectronicDevice GetDevice() => new Television();
}