namespace KnowledgeBase.Samples.Oop;

/// <summary>
/// Value object modelling an animal's registration identity.
/// Immutable records give value semantics (equality by data) for free.
/// </summary>
public sealed record AnimalIDInfo(int IdNumber, string Owner);