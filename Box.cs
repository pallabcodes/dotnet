namespace ConsoleApp1;

internal class Box
{
    // Default constructor that initializes the box with 1x1x1 dimensions
    public Box() : this(1, 1, 1)
    {
    }

    // Constructor that allows setting custom dimensions for the box
    public Box(double l, double w, double b)
    {
        Length = l; // Assigns the provided length to the Length property
        Width = w; // Assigns the provided width to the Width property
        Breadth = b; // Assigns the provided breadth to the Breadth property
    }

    public double Length { get; set; } // Property for the length of the box
    public double Width { get; set; } // Property for the width of the box
    public double Breadth { get; set; } // Property for the breadth of the box


    // operator overloading with +, -, ==, !=, 

    // However, using + operator to define how two objects (box1, box2) should be added together.

    // Overloads the + operator to add two Box objects
    public static Box operator +(Box box1, Box box2)
    {
        // Creates a new Box where each dimension is the sum of the respective dimensions of box1 and box2
        var newBox = new Box
        {
            Length = box1.Length + box2.Length,
            Width = box1.Width + box2.Width,
            Breadth = box1.Breadth + box2.Breadth
        };
        return newBox; // Returns the new Box
    }

    // Overloads the - operator to subtract one Box from another
    public static Box operator -(Box box1, Box box2)
    {
        // Creates a new Box where each dimension is the difference of the respective dimensions of box1 and box2
        var newBox = new Box
        {
            Length = box1.Length - box2.Length,
            Width = box1.Width - box2.Width,
            Breadth = box1.Breadth - box2.Breadth
        };
        return newBox; // Returns the new Box
    }

    // Overloads the == operator to check if two boxes are equal
    public static bool operator ==(Box box1, Box box2)
    {
        return box1.Length == box2.Length &&
               box1.Width == box2.Width &&
               box1.Breadth == box2.Breadth; // Returns true if all dimensions are equal
    }

    // Overloads the != operator to check if two boxes are different
    public static bool operator !=(Box box1, Box box2)
    {
        return !(box1 == box2); // Uses the overloaded == operator to check if the boxes are different
    }

    // Overrides the default ToString method to provide a custom string representation of the Box
    public override string ToString()
    {
        return string.Format("Box with Length: {0}, Width: {1}, and Breadth: {2}",
            Length, Width, Breadth); // Returns a formatted string with the dimensions of the box
    }

    // Explicitly converts a Box to an int by averaging its dimensions
    public static explicit operator int(Box b)
    {
        return
            (int)(b.Length + b.Width + b.Breadth) / 3; // Sums the dimensions and divides by 3, then casts to an integer
    }

    // Implicitly converts an int to a Box where all dimensions are set to the int value
    public static implicit operator Box(int i)
    {
        return new Box(i, i, i); // Returns a new Box with equal dimensions set to the passed integer value
    }

    // Overrides Equals to ensure proper comparison when using == or !=
    public override bool Equals(object obj)
    {
        if (obj is Box box) return this == box; // Uses the overloaded == operator to compare
        return false; // Returns false if obj is not a Box
    }

    // Overrides GetHashCode to provide a consistent hash code based on the box dimensions
    public override int GetHashCode()
    {
        return Length.GetHashCode() ^ Width.GetHashCode() ^
               Breadth.GetHashCode(); // Creates a hash code based on the dimensions
    }
}