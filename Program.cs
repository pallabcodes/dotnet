using System.Globalization;
using System.Text;

namespace ConsoleApp1;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        // Console.ForegroundColor = ConsoleColor.Black;
        // Console.BackgroundColor = ConsoleColor.White;
        // Console.Clear();

        // Console.WriteLine("What is your name ?");
        // var name = Console.ReadLine();
        // Console.WriteLine($"Hello {name}!"); // so, instead of backticks as JS it has like this

        // VARIABLES
        const bool isPrime = true;
        Console.WriteLine(isPrime);

        // INTEGERS: Integers are 32-bit signed integers
        Console.WriteLine("Biggest integer : {0}", int.MaxValue);
        Console.WriteLine("Smallest integer : {0}", int.MinValue);

        Console.WriteLine("Biggest long : {0}", long.MaxValue);
        Console.WriteLine("Smallest long : {0}", long.MinValue);

        // DECIMALS
        // Decimals store 128-bit precise decimal values
        // It is accurate to 28 digits
        var decPiVal = 3.1415926535897932384626433832M;
        var decBigNum = 3.00000000000000000000000000011M;
        Console.WriteLine("DEC : PI + bigNum = {0}", decPiVal + decBigNum);

        Console.WriteLine("Biggest Decimal : {0}", decimal.MaxValue);

        // DOUBLES
        // Doubles are 64-bit float types
        Console.WriteLine("Biggest Double : {0}", double.MaxValue);

        // It is precise to 14 digits
        var dblPiVal = 3.14159265358979;
        var dblBigNum = 3.00000000000002;
        Console.WriteLine("DBL : PI + bigNum = {0}", dblPiVal + dblBigNum);

        Console.WriteLine("Biggest decimal : {0}", decimal.MaxValue);
        Console.WriteLine("Smallest decimal : {0}", decimal.MinValue);

        // FLOATS
        // Floats are 32-bit float types
        Console.WriteLine("Biggest Float : {0}", float.MaxValue.ToString("#"));
        Console.WriteLine("Smallest Float : {0}", float.MinValue.ToString("#"));

        // decimal will be exactly 6 digits
        var fltPiVal = 3.141592F;
        var fltBigNum = 3.000002F;
        Console.WriteLine("FLT : PI + bigNum = {0}", fltPiVal + fltBigNum);

        // Other Data Types
        // byte : 8-bit unsigned int 0 to 255
        // char : 16-bit unicode character
        // sbyte : 8-bit signed int 128 to 127
        // short : 16-bit signed int -32,768 to 32,767
        // uint : 32-bit unsigned int 0 to 4,294,967,295
        // ulong : 64-bit unsigned int 0 to 18,446,744,073,709,551,615
        // ushort : 16-bit unsigned int 0 to 65,535

        // ---------- DATA TYPE CONVERSION ----------
        // You can convert from string to other types with Parse
        var boolFromStr = bool.Parse("True");
        Console.WriteLine(boolFromStr);
        var intFromStr = int.Parse("100");
        Console.WriteLine(intFromStr);
        var dblFromStr = double.Parse("1.234");
        Console.WriteLine(dblFromStr);

        // Convert double into a string
        var strVal = dblFromStr.ToString(CultureInfo.InvariantCulture);
        Console.WriteLine(strVal);

        // Get the new data type
        Console.WriteLine($"Data type : {strVal.GetType()}");

        // Cast double into integer (Explicit Conversion)
        // Put the data type to convert into between ()
        var dblNum = 12.345;
        Console.WriteLine($"Integer : {(int)dblNum}");

        // Cast integer into long (Implicit Conversion)
        // smaller size type to a larger size
        var intNum = 10;
        long longNum = intNum;
        Console.WriteLine(longNum);

        // ---------- FORMATTING OUTPUT ----------

        // Format output for currency
        Console.WriteLine("Currency : {0:c}", 23.455);

        // Pad with zeroes
        Console.WriteLine("Pad with 0s : {0:d4}", 23);

        // Define decimals
        Console.WriteLine("3 Decimals : {0:f3}", 23.4555);

        // Add commas and decimals
        Console.WriteLine("Commas : {0:n4}", 2300);

        // ---------- STRINGS ----------
        // Strings store a series of characters
        var randString = "This is a string";

        string? nullableString = null;
        Console.WriteLine(nullableString ?? "String is null"); // Output: String is null
        Console.WriteLine(randString.Remove(5)); // Output: This


        var parts = randString.Split(" ");
        Console.WriteLine("Split: ${parts[0]}"); // Output: This
        Console.WriteLine(randString.Substring(5, 2)); // Output: is
        Console.WriteLine(string.Concat("Hello", " ", "World")); // Output: Hello World

        // Get number of characters in string
        Console.WriteLine("String Length : {0}", randString.Length);

        // Check if string contains other string
        Console.WriteLine("String Contains is : {0}",
            randString.Contains("is"));

        // Index of string match
        Console.WriteLine("Index of is : {0}",
            randString.IndexOf("is", StringComparison.Ordinal));

        // Remove number of characters starting at an index
        Console.WriteLine("Remove string : {0}",
            randString.Remove(10, 6));

        // Add a string starting at an index
        Console.WriteLine("Insert String : {0}",
            randString.Insert(10, "short "));

        // Replace a string with another
        Console.WriteLine("Replace String : {0}",
            randString.Replace("string", "sentence"));

        // Compare strings and ignore case
        // < 0 : str1 preceeds str2
        // = : Zero
        // > 0 : str2 preceeds str1
        Console.WriteLine("Compare A to B : {0}",
            string.Compare("A", "B", StringComparison.OrdinalIgnoreCase));

        // Check if strings are equal
        Console.WriteLine("A = a : {0}",
            string.Equals("A", "a", StringComparison.OrdinalIgnoreCase));


        // Add padding left
        Console.WriteLine("Pad Left : {0}",
            randString.PadLeft(20, '.'));

        // Add padding right
        Console.WriteLine("Pad Right : {0} Stuff",
            randString.PadRight(20, '.'));

        // Trim whitespace
        Console.WriteLine("Trim : {0}",
            randString.Trim());

        // Make uppercase
        Console.WriteLine("Uppercase : {0}",
            randString.ToUpper());

        // Make lowercase
        Console.WriteLine("Lowercase : {0}",
            randString.ToLower());

        // Use Format to create strings
        var newString = string.Format("{0} saw a {1} {2} in the {3}",
            "Paul", "rabbit", "eating", "field");

        // You can add newlines with \n and join strings with +
        Console.Write(newString + "\n");

        // Other escape characters
        // \' \" \\ \t \a

        // Verbatim strings ignore escape characters
        Console.WriteLine(@"Exactly What I Typed\n");

        // ------ ARRAYS ------

        // Arrays have fixed sizes
        var favNums =
            new int[3]; // since, no value provided only size so each element will be filled with garbage value i.e. 0 
        Console.WriteLine(favNums[0]);

        // Add a value to the array
        favNums[0] = 23;

        // Retrieve a value
        Console.WriteLine("favNum 0 : {0}", favNums[0]);

        // Create and fill array
        string[] customers = { "Bob", "Sally", "Sue" };
        Console.WriteLine(customers[0]);

        // You can use var to create arrays, but the values must be of the same type
        var employees = new[] { "Mike", "Paul", "Rick" };
        Console.WriteLine(employees[0]);

        // Create an array of base objects which is the base type of all other types
        object[] randomArray = { "Paul", 45, 1.234 };
        Console.WriteLine(randomArray[1]);

        // GetType knows its true type
        Console.WriteLine("randomArray 0 : {0}", randomArray[0].GetType());

        // Get length of an array
        Console.WriteLine("Array Size : {0}", randomArray.Length);


        // var keyword is used for `implicitly typed variables`. It allows the compiler to infer the type of the variable based on the context (e.g., what type of value is assigned to it). Importantly, C# uses block scoping for var within loops, methods, or any other block of code, which makes it more predictable and safer in cases like loops.

        // The C# compiler will infer the type of the variable based on the assigned value, so no need to explicitly specify the type for j thus no need to int j
        // BELOW: {0} : {1} are placeholders so {0} will have value from j and {1} will have value from randomArray[j] from each iteration
        for (var j = 0; j < randomArray.Length; j++) Console.WriteLine("Array {0} : Value : {1}", j, randomArray[j]);

        // Multidimensional arrays
        var twoDimensionalArray = new int[3, 2]; // 3 rows, 2 columns
        twoDimensionalArray[0, 0] = 1;
        twoDimensionalArray[0, 1] = 2;
        twoDimensionalArray[1, 0] = 3;
        twoDimensionalArray[1, 1] = 4;
        twoDimensionalArray[2, 0] = 5;
        twoDimensionalArray[2, 1] = 6;

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 2; j++) Console.Write(twoDimensionalArray[i, j] + " ");
            Console.WriteLine(); // adds a new line
        }

        // Create a 2D array and the [,] notation tells the compiler that the array will have two dimensions (rows and columns).
        int[,] array2D =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };

        // so below tells the compiler that it will be 3dimensional as seen below
        // int[,,] array3D =
        // {
        //     { { 1, 2 }, { 3, 4 } },
        //     { { 5, 6 }, { 7, 8 } },
        //     { { 9, 10 }, { 11, 12 } }
        // };


        // Loop through the 2D array
        for (var i = 0; i < array2D.GetLength(0); i++) // Loop through rows
        {
            for (var j = 0; j < array2D.GetLength(1); j++) // Loop through columns
                // Print each element, followed by a space (no line break yet)
                Console.Write(array2D[i, j] + " ");

            // Print a newline after each row (this simulates the 'endl' behavior)
            Console.WriteLine(); // This adds a newline after each row
        }

        // 2 dimensional
        var customerNames = new string[2, 2]
        {
            { "Bob", "Smith" },
            { "Sally", "Smith" }
        };

        Console.WriteLine("MD Value : {0}", customerNames.GetValue(1, 1));

        for (var j = 0; j < customerNames.GetLength(0); j++)
        {
            // Get length of multidimensional array row
            for (var k = 0; k < customerNames.GetLength(1); k++) Console.Write("{0} ", customerNames[j, k]);
            Console.WriteLine();
        }

        // 3 dimensional
        var customerNames3D = new string[2, 2, 2]
        {
            { { "Bob", "Smith" }, { "Sally", "Smith" } },
            { { "John", "Doe" }, { "Jane", "Doe" } }
        };

        // Iterating through the 3D array
        // for (var depth = 0; depth < customerNames3D.GetLength(0); depth++) // Iterate through layers
        // for (var row = 0; row < customerNames3D.GetLength(1); row++) // Iterate through rows
        // for (var col = 0; col < customerNames3D.GetLength(2); col++) // Iterate through columns
        //     Console.WriteLine($"Depth {depth}, Row {row}, Col {col}: {customerNames3D[depth, row, col]}");

        // foreach can be used to cycle through an array
        int[] randNums = { 1, 4, 9, 2 };

        // You can pass an array to a function
        // PrintArray(randNums, "ForEach");

        // Sort array
        Array.Sort(randNums);

        // Reverse array
        Array.Reverse(randNums);

        // Get index of match or return -1
        Console.WriteLine("1 at index : {0} ",
            Array.IndexOf(randNums, 1));

        // Change value at index 1 to 0
        randNums.SetValue(0, 1);

        // Copy part of an array to another
        int[] srcArray = { 1, 2, 3 };
        var destArray = new int[2];
        var startInd = 0;
        var length = 2;

        Array.Copy(srcArray, startInd, destArray,
            startInd, length);

        // PrintArray(destArray, "Copy");

        // Create an array with CreateInstance
        var anotherArray = new int[10];

        // Copy values in srcArray to destArray starting at index 5 in destination
        srcArray.CopyTo(anotherArray, 5);

        // foreach (var m in anotherArray) Console.WriteLine("CopyTo : {0} ", m);

        // ----- IF / ELSE / -----
        // Relational Operators : > < >= <= == !=
        // Logical Operators : && || !

        var age = 17;

        if (age >= 5 && age <= 7)
            Console.WriteLine("Go to elementary school");
        else if (age > 7 && age < 13)
            Console.WriteLine("Go to middle school");
        else if (age > 13 && age < 19)
            Console.WriteLine("Go to high school");
        else
            Console.WriteLine("Go to college");

        if (age < 14 || age > 67) Console.WriteLine("You shouldn't work");

        Console.WriteLine("! true = " + !true);

        // Ternary Operator
        // Assigns the 1st value if true and otherwise
        // the 2nd
        var canDrive = age >= 16 ? true : false;

        // Switch is used when you have limited options the only way to use ranges is to stack the possible values
        switch (age)
        {
            case 1:
            case 2:
                Console.WriteLine("Go to Day Care");
                break;
            case 3:
            case 4:
                Console.WriteLine("Go to Preschool");
                break;
            case 5:
                Console.WriteLine("Go to Kindergarten");
                break;
            default:
                Console.WriteLine("Go to another school");
                // 'goto' jumps to the specified label (e.g., OtherSchool:) and executes the following code after : i.e. below Console.WriteLine("Elementary, Middle, High School") and it's generally discouraged as it can make code harder to follow.
                goto OtherSchool;
        }

        OtherSchool:
        Console.WriteLine("Elementary, Middle, High School");

        // To compare strings use Equals
        var name2 = "Derek";
        var name3 = "Derek";

        if (name2.Equals(name3, StringComparison.Ordinal)) Console.WriteLine("Names are Equal");

        // ----- WHILE LOOP -----
        // You use the while loop when you want to execute
        // as long as a condition is true

        // This while loop will print odd numbers between
        // 1 and 10
        var iterator = 1;
        while (iterator <= 10)
        {
            // % (Modulus) returns the remainder of a
            // division. If it returns 0 that means the
            // value is even
            if (iterator % 2 == 0)
            {
                iterator++;

                // Continue skips the rest of the code and
                // starts execution back at the top of the
                // while
                continue;
            }

            // Break jumps completely out of the loop
            if (iterator == 9) break;

            Console.WriteLine(iterator);
            iterator++;
        }

        var start = 5;
        do
        {
            Console.WriteLine(start);
            start++;
        } while (start <= 10);

        for (int i = 0, j = 10; i < 5; i++, j--) Console.WriteLine($"i = {i}, j = {j}");

        int[] numbers = { 1, 2, 3, 4, 5 };
        foreach (var number in numbers) Console.Write(number + " "); // print horizontally

        // ----- EXCEPTION HANDLING -----
        try
        {
            var digits = new int[3];
            var result = digits[5]; // This will throw an IndexOutOfRangeException
        }
        // catch (IndexOutOfRangeException ex)
        // {
        //     Console.WriteLine("Logging the exception before re-throwing.");
        //     throw; // Re-throws the caught exception
        // }
        catch (Exception ex) when (ex is IndexOutOfRangeException or ArgumentException)
        {
            Console.WriteLine($"Handled specific exception: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Cleanup done.");
        }

        // ----- STRINGBUILDER -----
        // Each time you change a string you are actually
        // creating a new string which is inefficient
        // when you are working with large blocks of text
        // StringBuilders actually change the text
        // rather than make a copy

        // Create a StringBuilder with a default size
        // of 16 characters, but it grows automatically
        var sb = new StringBuilder("Random Text");

        // Create a StringBuilder with a size of 256
        var sb2 = new StringBuilder("More Stuff that is very important", 256);

        // Get max size
        Console.WriteLine("Capacity : {0}", sb2.Capacity);

        // Get length
        Console.WriteLine("Length : {0}", sb2.Length);

        // Add text to StringBuilder
        sb2.AppendLine("\nMore important text");

        // Define culture-specific formatting
        var enUS = CultureInfo.CreateSpecificCulture("en-US");

        // Append a format string
        var bestCust = "Bob Smith";
        sb2.AppendFormat(enUS, "Best Customer : {0}", bestCust);

        // Output StringBuilder
        Console.WriteLine(sb2.ToString());

        // Replace a string
        sb2.Replace("text", "characters");
        Console.WriteLine(sb2.ToString());

        // Clear a string builder
        sb2.Clear();

        sb2.Append("Random Text");

        // Are objects equal
        Console.WriteLine(sb.Equals(sb2));

        // Insert at an index
        sb2.Insert(11, " that's Great");

        Console.WriteLine("Insert : {0}", sb2);

        // Remove a number of characters starting at index
        sb2.Remove(11, 7);

        Console.WriteLine("Remove : {0}", sb2);

        // ---------- FUNCTIONS / METHODS ----------
        // Functions are used to avoid code duplication, provide
        // organization and allow us to simulate different
        // systems
        // <Access Specifier> <Return Type> <Method Name>(Parameters)
        // { <Body> }

        // Access Specifier determines whether the function can
        // be called from another class
        // public : Can be accessed from another class
        // private : Can't be accessed from another class
        // protected : Can be accessed by class and derived classes

        SayHello();

        // ----- PASSING BY VALUE -----
        // By default, values are passed into a method
        // and not a reference to the variable passed
        // Changes made to those values do not affect the
        // variables outside the method
        double x = 5;
        double y = 4;

        Console.WriteLine("5 + 4 = {0}",
            GetSum(x, y));

        // Even though the value for x changed in
        // method it remains unchanged here
        Console.WriteLine("x = {0}",
            x);

        // ----- OUT PARAMETER -----
        // You can pass a variable as an output
        // variable even without assigning a
        // value to it

        // A parameter passed with out has its value assigned in the method
        DoubleIt(15, out var solution);

        Console.WriteLine("15 * 2 = {0}", solution);

        // ----- PASS BY REFERENCE -----
        var num3 = 10;
        var num4 = 20;

        Console.WriteLine("Before Swap num1 : {0} num2 : {1}", num3, num4);

        Swap(ref num3, ref num4);

        Console.WriteLine("After Swap num1 : {0} num2 : {1}", num3, num4);

        // ----- PARAMS -----
        // You are able to pass a variable amount
        // of data of the same data type into a
        // method using params. You can also pass
        // in an array.
        Console.WriteLine("1 + 2 + 3 = {0}",
            GetSumMore(1, 2, 3));

        // ----- NAMED PARAMETERS -----
        // You can pass values in any order if
        // you used named parameters
        PrintInfo(zipCode: 15147,
            name: "Derek Banas");

        // ----- METHOD OVERLOADING -----
        // You can define methods with the same
        // name that will be called depending on
        // what data is sent automatically
        Console.WriteLine("5.0 + 4.0 = {0}",
            GetSum2(5.0, 4.5));

        Console.WriteLine("5 + 4 = {0}",
            GetSum2(5, 4));

        Console.WriteLine("5 + 4 = {0}",
            GetSum2("5", "4"));

        // ---------- DATETIME & TIMESPAN ----------
        // Used to define dates
        var awesomeDate = new DateTime(1974, 12, 21);
        Console.WriteLine("Day of Week : {0}", awesomeDate.DayOfWeek);

        // You can change values
        awesomeDate = awesomeDate.AddDays(4);
        awesomeDate = awesomeDate.AddMonths(1);
        awesomeDate = awesomeDate.AddYears(1);
        Console.WriteLine("New Date : {0}", awesomeDate.Date);

        // TimeSpan
        // Used to define a time
        var lunchTime = new TimeSpan(12, 30, 0);

        // Change values
        lunchTime = lunchTime.Subtract(new TimeSpan(0, 15, 0));
        lunchTime = lunchTime.Add(new TimeSpan(1, 0, 0));
        Console.WriteLine("New Time : {0}", lunchTime.ToString());

        // ----- ENUM -----
        var car1 = CarColor.Blue;
        PaintCar(car1);

        // Waits for input from the user if you run the consoleApp1.exe instead of instantly closing the executable is in bin/Debug/net8.0
        // Console.ReadLine();

        // Create a Rectangle
        Rectangle rect1;

        // Add values to it and run the Area method
        rect1.length = 200;
        rect1.width = 50;
        Console.WriteLine("Area of rect1 : {0}", rect1.Area());


        // Use a constructor to create a Rectangle
        var rect2 = new Rectangle(100, 40);

        // If you assign one Rectangle to another you are setting the values and not creating a reference
        rect2 = rect1;
        rect1.length = 33;

        Console.WriteLine("rect2.length : {0}", rect2.length);

        // ----- OBJECT ORIENTED PROGRAMMING -----
        // A class models real world objects by
        // defining their attributes (fields) and
        // capabilities (methods)
        // Then unlike with structs you can 
        // inherit from a class and create more
        // specific subclass types

        // Add a class Project -> Add Class

        // Create an Animal object
        // You could also assign values like
        // fox.name = "Red"


        var cat = new Animal();
        cat.SetName("Whiskers"); // setter

        // Set the public property
        cat.Sound = "Meow";

        Console.WriteLine("The cat is named {0} and says {1}", cat.GetName(), cat.Sound);

        // Test auto generated getters and setters
        cat.Owner = "Derek";

        Console.WriteLine("{0} owner is {1}", cat.GetName(), cat.Owner);

        // Get the read-only id number
        Console.WriteLine("{0} shelter id is {1}", cat.GetName(), cat.idNum);

        // ensure static property is working by testing as below
        Console.WriteLine("# of Animals : {0}", Animal.NumOfAnimals);

        // ----- NULLABLE TYPES -----
        // Data types by default cannot have a
        // value of null. Often null is needed
        // when you are working with databases
        // and you can create a null type by 
        // adding a ? to the definition
        int? randNum = null;

        // Check for null
        if (randNum == null) Console.WriteLine("randNum is null");

        // Another check for null
        if (!randNum.HasValue) Console.WriteLine("randNum is null");

        // 2:40:00 , start 4 (https://github.com/derekbanas/C-Sharp-Course/blob/main/C%23%20Code%203/Program.cs)

        Console.ReadLine();
    }

    private static void SayHello()
    {
        Console.WriteLine("Hello, World!");
    }

    private static double GetSum(double a, double b)
    {
        return a + b;
    }

    private static void DoubleIt(int number, out int result)
    {
        result = number * 2;
    }

    private static void Swap(ref int a, ref int b)
    {
        var temp = a;
        a = b;
        b = temp;
    }

    private static int GetSumMore(params int[] numbers)
    {
        var sum = 0;
        foreach (var number in numbers) sum += number;
        return sum;
    }

    private static void PrintInfo(string name, int zipCode)
    {
        Console.WriteLine("Name: {0}, ZipCode: {1}", name, zipCode);
    }

    private static double GetSum2(double a, double b)
    {
        return a + b;
    }

    private static int GetSum2(int a, int b)
    {
        return a + b;
    }

    private static int GetSum2(string a, string b)
    {
        return int.Parse(a) + int.Parse(b);
    }

    private static void PaintCar(CarColor color)
    {
        Console.WriteLine($"The car is painted {color}.");
    }

    // ----- STRUCTS -----
    // A struct is a user defined type that contain multiple fields and methods
    private struct Rectangle
    {
        public double length;
        public double width;

        // You can create a constructor method
        // that will set the values for fields
        public Rectangle(double l = 1, double w = 1)
        {
            length = l;
            width = w;
        }

        public double Area()
        {
            return length * width;
        }
    }

    private enum CarColor
    {
        Red,
        Blue,
        Green,
        Black
    }
}