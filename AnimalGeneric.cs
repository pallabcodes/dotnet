namespace ConsoleApp1;

public class AnimalGeneric(string name = "No Name")
{
    public string Name { get; set; } = name;

    // Used generics to handle multiple types of parameters and return types with a single method, avoiding traditional method overloading
    public static void GetSum<T>(ref T num1, ref T num2)
    {
        var dblX = Convert.ToDouble(num1);
        var dblY = Convert.ToDouble(num2);
        Console.WriteLine($"{dblX} + {dblY} = {dblX + dblY}");
    }
}