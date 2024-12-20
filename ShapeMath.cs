namespace ConsoleApp1;

public class ShapeMath
{
    // This class can only contain static methods and constant values
    public static double GetArea(string shape = "", double length1 = 0, double length2 = 0)
    {
        if (string.Equals("Rectangle",
                shape,
                StringComparison.OrdinalIgnoreCase))
            return length1 * length2;

        if (string.Equals("Triangle",
                shape,
                StringComparison.OrdinalIgnoreCase))
            return length1 * (length2 / 2);

        if (string.Equals("Circle",
                shape,
                StringComparison.OrdinalIgnoreCase))
            return 3.14159 * Math.Pow(length1, 2);

        return -1;
    }
}