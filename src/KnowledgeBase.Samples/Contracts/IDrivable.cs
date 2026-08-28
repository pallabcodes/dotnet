namespace KnowledgeBase.Samples.Contracts;

/// <summary>Contract for anything that can be driven.</summary>
public interface IDrivable
{
    int Wheels { get; set; }

    double Speed { get; set; }

    double MaxSpeed { get; }

    void Move();

    void Stop();
}