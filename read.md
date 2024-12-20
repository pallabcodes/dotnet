> c# interface is to be used to hold contracts i.e. methods

```c#
// C# Struct Example
public struct Person
{
    public string Name;
    public int Age;

    public void Introduce()
    {
        Console.WriteLine($"Hello, my name is {Name}, and I'm {Age} years old.");
    }
}

// C# Interface Example
public interface ISpeaker
{
    void Speak();
}


```

> c# struct is to be used to contain fields/properties only