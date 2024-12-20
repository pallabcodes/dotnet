namespace ConsoleApp1;

public class Circle : Shape
{
    public Circle(double radius)
    {
        Name = "Circle";
        Radius = radius;
    }

    public double Radius { get; set; }

    public override double Area()
    {
        return Math.PI * Math.Pow(Radius, 2.0);
    }

    // You can replace the method using override and public override void GetInfo() this will only the 1st subclass to override so the later subclasses will get an error
    public override void GetInfo()
    {
        // Execute the base version
        base.GetInfo();
        Console.WriteLine($"It has a Radius of {Radius}");
    }
}