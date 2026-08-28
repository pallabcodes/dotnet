namespace KnowledgeBase.Samples.Polymorphism;

/// <summary>
/// Abstract base type: derived shapes must supply an Area while inheriting a
/// default description. Note the contrast with string-based dispatch (see README) —
/// here the runtime type, not a string argument, drives behaviour.
/// </summary>
public abstract class Shape
{
    protected Shape(string name) => Name = name;

    public string Name { get; }

    public virtual string Describe() => $"This is a {Name}";

    public abstract double Area();
}