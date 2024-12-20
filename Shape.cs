namespace ConsoleApp1;

public abstract class Shape
{
    public string Name { get; set; }

    // Purpose: A virtual method in a class allows derived classes to override it, but the derived class is not required to do so. It provides a default implementation, but subclasses can choose to override it.
    public virtual void GetInfo()
    {
        Console.WriteLine($"This is a {Name}");
    }

    // We want subclasses to override
    // this method, so mark it as abstract
    // You can only make abstract methods 
    // in abstract class
    public abstract double Area();
}