using System.Collections;

// For List<T>

// For IEnumerable

namespace ConsoleApp1;

// IEnumerable provides for iteration over a collection
internal class AnimalFarm : IEnumerable<Animal>
{
    // Holds list of Animals
    private readonly List<Animal> animalList;

    // Constructor that accepts a list of Animals
    public AnimalFarm(List<Animal> animalList)
    {
        this.animalList = animalList;
    }

    // Parameterless constructor initializes an empty list
    public AnimalFarm()
    {
        animalList = new List<Animal>();
    }

    // Indexer for AnimalFarm created with this[]
    public Animal this[int index]
    {
        get => animalList[index]; // Directly return the animal at the index
        set
        {
            if (index < animalList.Count)
            {
                animalList[index] = value; // Replace the value at the given index
            }
            else
            {
                // If index is out of bounds, add new animal(s)
                for (var i = animalList.Count;
                     i < index;
                     i++) animalList.Add(null); // Fill in with null until we reach the desired index

                animalList.Add(value); // Add the new animal at the correct index
            }
        }
    }

    // Returns the number of values in the collection
    public int Count => animalList.Count;

    // Returns an enumerator that is used to iterate through the collection
    public IEnumerator<Animal> GetEnumerator()
    {
        return animalList.GetEnumerator();
    }

    // Explicit non-generic GetEnumerator for IEnumerable interface
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator(); // Call the generic version
    }
}