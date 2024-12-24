```c#
using System;
using System.Collections.Generic; // For List<T>
using System.Collections; // For IEnumerable

namespace ConsoleApp1
{
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
                    for (int i = animalList.Count; i < index; i++)
                    {
                        animalList.Add(null); // Fill in with null until we reach the desired index
                    }
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

    // Define the Animal class
    public class Animal
    {
        public string Name { get; set; }

        public Animal(string name)
        {
            Name = name;
        }

        public override string ToString() => Name; // To string returns the animal's name
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create an AnimalFarm with 4 animals
            AnimalFarm farm = new AnimalFarm();
            
            // Add animals at indices 0, 1, 2, 3
            farm[0] = new Animal("Cow");
            farm[1] = new Animal("Horse");
            farm[2] = new Animal("Sheep");
            farm[3] = new Animal("Chicken");

            // Iterate through the AnimalFarm using foreach
            foreach (var animal in farm)
            {
                Console.WriteLine(animal); // Print each animal
            }
        }
    }
}

```