using System.Collections;

namespace KnowledgeBase.Samples.Oop;

/// <summary>
/// A growable, foreach-able collection of animals that owns its invariants:
/// indices are contiguous, out-of-range writes fail loudly, and iteration is
/// delegated to a List&lt;T&gt;. Demonstrates IEnumerable&lt;T&gt; and a custom indexer.
/// </summary>
public sealed class AnimalFarm : IEnumerable<Animal>
{
    private readonly List<Animal> _animals = [];

    public int Count => _animals.Count;

    public Animal this[int index]
    {
        get => _animals[index];
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (index < 0 || index > _animals.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    "Index must map to the next available slot.");
            }

            if (index == _animals.Count)
            {
                _animals.Add(value);
            }
            else
            {
                _animals[index] = value;
            }
        }
    }

    public IEnumerator<Animal> GetEnumerator() => _animals.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}