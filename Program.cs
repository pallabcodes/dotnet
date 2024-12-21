using System.Collections;
using System.Globalization;
using System.Text;

// Used for ArrayLists

// Used for Dictionary

namespace ConsoleApp1;

internal class Program
{
    // Defines a delegate type 'Arithmetic' that represents methods accepting two double parameters and returning void.

    public delegate void Arithmetic(double num1, double num2);

    public delegate double doubleIt(double val);


    private static void Main(string[] args)
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

        // Compare strings and ignore a case
        // < 0: str1 preceeds str2
        // =: Zero
        // > 0: str2 preceeds str1
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

        // SayHello();

        // ----- PASSING BY VALUE -----
        // By default, values are passed into a method
        // and not a reference to the variable passed
        // Changes made to those values do not affect the
        // variables outside the method
        // double x = 5;
        // double y = 4;

        // Console.WriteLine("5 + 4 = {0}", GetSum(x, y));

        // Even though the value for x changed in method it remains unchanged here
        // Console.WriteLine("x = {0}", x);

        // ----- OUT PARAMETER -----
        // You can pass a variable as an output
        // variable even without assigning a
        // value to it

        // A parameter passed without has its value assigned in the method
        // DoubleIt(15, out var solution);

        // Console.WriteLine("15 * 2 = {0}", solution);

        // ----- PASS BY REFERENCE -----
        var num3 = 10;
        var num4 = 20;

        Console.WriteLine("Before Swap num1 : {0} num2 : {1}", num3, num4);

        // Swap(ref num3, ref num4);

        Console.WriteLine("After Swap num1 : {0} num2 : {1}", num3, num4);

        // ----- PARAMS -----
        // You are able to pass a variable amount
        // of data of the same data type into a
        // method using params. You can also pass
        // in an array.
        // Console.WriteLine("1 + 2 + 3 = {0}", GetSumMore(1, 2, 3));

        // ----- NAMED PARAMETERS -----
        // You can pass values in any order if
        // you used named parameters
        // PrintInfo(zipCode: 15147, name: "Derek Banas");

        // ----- METHOD OVERLOADING -----
        // You can define methods with the same
        // name that will be called depending on
        // what data is sent automatically
        // Console.WriteLine("5.0 + 4.0 = {0}", GetSum2(5.0, 4.5));

        // Console.WriteLine("5 + 4 = {0}", GetSum2(5, 4));

        // Console.WriteLine("5 + 4 = {0}", GetSum2("5", "4"));

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
        // var car1 = CarColor.Blue;
        // PaintCar(car1);

        // Waits for input from the user if you run the consoleApp1.exe instead of instantly closing the executable is in bin/Debug/net8.0
        // Console.ReadLine();

        // Create a Rectangle
        // Rectangle rect1;

        // Add values to it and run the Area method
        // rect1.length = 200;
        // rect1.width = 50;
        // Console.WriteLine("Area of rect1 : {0}", rect1.Area());


        // Use a constructor to create a Rectangle
        // var rect2 = new Rectangle(100, 40);

        // If you assign one Rectangle to another you are setting the values and not creating a reference
        // rect2 = rect1;
        // rect1.length = 33;

        // Console.WriteLine("rect2.length : {0}", rect2.length);

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

        var whiskers = new Animal
        {
            Name = "Whiskers",
            Sound = "Meow"
        };

        var grover = new Dog();
        grover.Name = "Grover";
        grover.Sound = "Woof";
        grover.Sound2 = "Grrrrr";

        // Demonstrate changing the protected field sound
        grover.Sound = "Wooooof";
        whiskers.MakeSound();
        grover.MakeSound();

        // Define the AnimalIDInfo
        whiskers.SetAnimalIDInfo(12345, "Sally Smith");
        grover.SetAnimalIDInfo(12346, "Paul Brown");

        whiskers.GetAnimalIDInfo();

        // Test the inner class
        var getHealth = new Animal.AnimalHealth();

        // You can define 2 Animal objects but have
        // one actually be a Dog type. 
        var monkey = new Animal
        {
            Name = "Happy",
            Sound = "Eeeeee"
        };

        Animal spot = new Dog
        {
            Name = "Spot",
            Sound = "Wooooff",
            Sound2 = "Geerrrr"
        };

        // To ensure a method in `Dog` is called when invoked on an `Animal` reference, the method in the parent (`Animal`) must be marked as `virtual` or `abstract`, and the method in the subclass (`Dog`) should override it.
        spot.MakeSound();

        // The above example demonstrates how polymorphism allows a subclass to override a superclass method, ensuring the correct method is called even when the subclass is referenced as the superclass type.


        Console.WriteLine("Is my animal healthy : {0}", getHealth.HealthyWeight(11, 46));

        // We can store our shapes in
        // a Shape array as long as it 
        // contains subclasses of Shape
        Shape[] shapes =
        {
            new Circle(5),
            new Rectangle(4, 5)
        };

        // Cycle through shapes and print the area
        foreach (var s in shapes)
        {
            // Call the overridden method
            s.GetInfo();

            Console.WriteLine("{0} Area : {1:f2}", s.Name, s.Area());

            // You can use as to check if an
            // object is of a specific type
            var testCirc = s as Circle;
            if (testCirc == null) Console.WriteLine("This isn't a Circle");

            // You can use is to check the data
            // type
            if (s is Circle) Console.WriteLine("This isn't a Rectangle");

            // Create a Vehicle object
            var buick = new Vehicle("Buick",
                4, 160);

            // Check if Vehicle implements 
            // IDrivable
            if (buick is IDrivable)
            {
                buick.Move();
                buick.Stop();
            }
            else
            {
                Console.WriteLine("The {0} can't be driven", buick.Brand);
            }

            // We are now modeling the act of
            // picking up a remote, aiming it
            // at the TV, clicking the power
            // button and then watching as
            // the TV turns on and off

            // Pick up the TV remote
            var TV = TVRemote.GetDevice();

            // Create the power button
            var powBut = new PowerButton(TV);

            // Turn the TV on and off with each 
            // press
            powBut.Execute();
            powBut.Undo();
            powBut.Execute();
            powBut.Undo();

            /*
             * Thor Attacks Hulk and Deals 74 Damage
             * Maximus Has 69 Health
             *
             * Hulk Attacks Thor and Deals 6 Damage
             * Bob Has 6 Health
             *
             * Thor Attacks Hulk and Deals 48 Damage
             * Maximus Has 21 Health
             *
             * Hulk Attacks Thor and Deals 48 Damage
             * Bob Has -42 Health
             *
             * Thor has Died and Hulk is Victorious
             *
             * Game Over
             * */

            var thor = new Warrior("Thor", 100, 26, 10);
            // Warrior loki = new Warrior("Loki", 100, 26, 10);

            // Thor is more powerful, so let's treat him that way
            // Loki will however have magic abilities
            var loki = new MagicWarrior("Loki", 75, 20, 10, 50);

            Battle.StartFight(thor, loki);

            // ----- ARRAYLIST -----
            // Collections can resize unlike arrays

            // #region provides a way to collapse long blocks of code (it is just for folding, that's all)

            #region ArrayList Code

            // ArrayLists are resizable arrays that can hold multiple data types
            var aList = new ArrayList();

            aList.Add("Bob");
            aList.Add(40);

            // Number of items in a list
            Console.WriteLine("Count: {0}", aList.Count);

            // The capacity automatically increases as items are added
            Console.WriteLine("Capacity: {0}", aList.Capacity);

            var aList2 = new ArrayList();

            // Add an object array
            aList2.AddRange(new object[] { "Mike", "Sally", "Egg" });

            // Add 1 array list to another
            aList.AddRange(aList2);

            // You can sort the list if the types are the same
            aList2.Sort();
            aList2.Reverse();

            // Insert at the 2nd position
            aList2.Insert(1, "Turkey");

            // Get the 1st 2 items
            var range = aList2.GetRange(0, 2);

            /*
             * // Remove the first item
             * aList2.RemoveAt(0);
             * // Remove the 1st 2 items
             * aList2.RemoveRange(0, 2);
             * */

            // Search for a match starting at the provided index. You can also find the last index with LastIndexOf
            Console.WriteLine("Turkey Index : {0}", aList2.IndexOf("Turkey", 0));

            // Cycle through the list
            foreach (var o in range) Console.WriteLine(o);

            // Convert an ArrayList into a string array
            string[] myArray = (string[])aList2.ToArray(typeof(string));

            // Convert a string array into an ArrayList
            string[] students = { "Bob", "Sally", "Sue" };
            var listOfStudents = new ArrayList();
            listOfStudents.AddRange(students);

            #endregion


            // ---------- DICTIONARIES ----------

            #region Dictionary Code

            // Dictionaries store key value pairs
            // To create them define the data
            // type for the key and the value
            Dictionary<string, string> superheroes = new();

            superheroes.Add("Clark Kent", "Superman");
            superheroes.Add("Bruce Wayne", "Batman");
            superheroes.Add("Barry West", "Flash");

            // Remove a key / value
            superheroes.Remove("Barry West");

            // Number of keys
            Console.WriteLine("Count : {0}",
                superheroes.Count);

            // Check if a key is present
            Console.WriteLine("Clark Kent : {0}",
                superheroes.ContainsKey("Clark Kent"));

            // Get the value for the key and store it
            // in a string

            superheroes.TryGetValue("Clark Kent", out var test);

            Console.WriteLine($"Clark Kent : {test}");

            // Cycle through key value pairs
            foreach (KeyValuePair<string, string> item in superheroes)
                Console.WriteLine("{0} : {1}",
                    item.Key,
                    item.Value);

            // Clear a dictionary
            superheroes.Clear();

            #endregion

            // ---------- QUEUES ----------

            #region Queue Code

            // Queue 1st in 1st Out Collection

            // Create a Queue
            var queue = new Queue();

            // Add values
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            // Is item in queue
            Console.WriteLine("1 in Queue : {0}",
                queue.Contains(1));

            // Remove 1st item from queue
            Console.WriteLine("Remove 1 : {0}",
                queue.Dequeue());

            // Look at 1st item but don't remove
            Console.WriteLine("Peek 1 : {0}",
                queue.Peek());

            // Copy queue to array
            object[] numArray = queue.ToArray();

            // Print array
            Console.WriteLine(string.Join(",", numArray));

            // Print queue items
            foreach (var o in queue) Console.WriteLine($"Queue : {o}");

            // Clear the queue
            queue.Clear();

            #endregion

            // ---------- STACKS ----------

            #region Stack Code

            // Stack Last in 1st Out Collection

            // Create a stack
            var stack = new Stack();

            // Put items on the stack
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            // Get but don't remove item
            Console.WriteLine("Peek 1 : {0}", stack.Peek());

            // Remove item
            Console.WriteLine("Pop 1 : {0}", stack.Pop());

            // Does item exist on stack?
            Console.WriteLine("Contain 1 : {0}", stack.Contains(1));

            // Copy stack to array
            var numArray2 = stack.ToArray();

            // Print array
            Console.WriteLine(string.Join(",", numArray2));

            // Print the stack
            foreach (var o in stack) Console.WriteLine($"Stack : {o}");

            #endregion

            // Generic collections are type safe
            // and provide performance benefits

            // You define the data type when defining
            // the generic. This is a dynamically
            // resizing collection
            List<Animal> animalList = new();

            // You can also create a list of standard types like int
            var numListAlt = new List<int>();

            // Add an int
            numListAlt.Add(24);

            // Add Animals
            animalList.Add(new Animal { Name = "Doug" });
            animalList.Add(new Animal { Name = "Paul" });
            animalList.Add(new Animal { Name = "Sally" });

            // Insert in index 1
            animalList.Insert(1, new Animal { Name = "Steve" });

            // Remove index 1
            animalList.RemoveAt(1);

            // Get number of Animals
            Console.WriteLine("Num of Animals : {0}", animalList.Count());

            // Cycle through Animals
            foreach (var a in animalList) Console.WriteLine(a.Name);

            // You can also use Stack<T>, Queue<T>,
            // Dictionary<TKey, TValue> like I covered
            // previously

            // Generic methods
            // You can use the type parameter <int>
            // if it can be inferred from the parameters
            // passed (Can't do this if there are no
            // parameters
            int x = 5, y = 4;
            // Animal.GetSum<int>(ref x, ref y);

            // It works for strings as well
            string strX = "5", strY = "4";
            // Animal.GetSum(ref strX, ref strY);

            // Use the generic struct
            // var rec1 = new Rectangle<int>(20, 50);
            // Console.WriteLine(rec1.GetArea());

            // var rec2 = new Rectangle<string>("20", "50");
            // Console.WriteLine(rec2.GetArea());

            // Delegates allow you to reference methods
            // inside a delegate object. The delegate
            // object can then be passed to other
            // methods that can call the methods assigned
            // to the delegate. It can also stack methods
            // that are called in the specified order

            // so, these add, sub, addSub are nothing but method that will same parameter types and returnType as Arithmetic 
            Arithmetic add, sub, addSub;

            // Assign just the Add method
            add = Add;

            // Assign just the Subtract method
            sub = Subtract;

            // Assign Add and Sub
            addSub = add + sub;

            // You could also subtract a method
            // sub = addSub - add;

            // Print out results
            Console.WriteLine($"Add {6} & {10}");
            add(6, 10);

            // Call both methods
            Console.WriteLine($"Add & Subtract {10} & {4}");
            addSub(10, 4);

            // Like we did with predicates earlier
            // Lambda expressions allow you to 
            // use anonymous methods that define
            // the input parameters on the left 
            // and the code to execute on the right

            // Assign a Lambda to the delegate
            doubleIt dblIt = x => x * 2;
            Console.WriteLine($"5 * 2 = {dblIt(5)}");

            // You don't have to use delegates though
            // Here we'll search through a list to 
            // find all the even numbers
            var numList = new List<int> { 1, 9, 2, 6, 3 };

            // Put the number in the list if the 
            // condition is true
            var evenList = numList.Where(a => a % 2 == 0).ToList();

            foreach (var j in evenList)
                Console.WriteLine(j);

            // Add values in a range to a list
            var rangeList = numList.Where(x => x > 2 || x < 9).ToList();

            foreach (var k in rangeList)
                Console.WriteLine(k);

            // Find the number of heads and tails in
            // a list 1 = H, 2 = T

            // Generate our list
            var flipList = new List<int>();
            var i = 0;
            var rnd = new Random();
            while (i < 100)
            {
                flipList.Add(rnd.Next(1, 3));
                i++;
            }

            // Print out the heads and tails
            Console.WriteLine("Heads : {0}",
                flipList.Where(a => a == 1).ToList().Count());
            Console.WriteLine("Tails : {0}",
                flipList.Where(a => a == 2).ToList().Count());

            // Find all names starting with s
            var nameList = new List<string> { "Doug", "Sally", "Sue" };

            var sNameList = nameList.Where(x => x.StartsWith("S"));

            foreach (var m in sNameList)
                Console.WriteLine(m);

            // ---------- SELECT ----------
            // Select allows us to execute a function 
            // on each item in a list

            // Generate a list from 1 to 10
            var oneTo10 = new List<int>();
            oneTo10.AddRange(Enumerable.Range(1, 10));

            var squares = oneTo10.Select(x => x * x);

            foreach (var l in squares)
                Console.WriteLine(l);

            // ---------- ZIP ----------
            // Zip applies a function to two lists 
            // Add values in 2 lists together
            var listOne = new List<int>(new[] { 1, 3, 4 });
            var listTwo = new List<int>(new[] { 4, 6, 8 });

            var sumList = listOne.Zip(listTwo, (x, y) => x + y).ToList();

            foreach (var n in sumList)
                Console.WriteLine(n);

            // ---------- AGGREGATE ----------
            // Aggregate performs an operation on 
            // each item in a list and carries the 
            // results forward 

            // Sum values in a list
            var numList2 = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine("Sum : {0}",
                numList2.Aggregate((a, b) => a + b));

            // ---------- AVERAGE ----------
            // Get the average of a list of values
            var numList3 = new List<int> { 1, 2, 3, 4, 5 };

            // AsQueryable allows you to manipulate the
            // collection with the Average function
            Console.WriteLine("AVG : {0}",
                numList3.AsQueryable().Average());

            // ---------- ALL ----------
            // Determines if all items in a list
            // meet a condition
            var numList4 = new List<int> { 1, 2, 3, 4, 5 };

            Console.WriteLine("All > 3 : {0}",
                numList4.All(x => x > 3));

            // ---------- ANY ----------
            // Determines if any items in a list
            // meet a condition
            var numList5 = new List<int> { 1, 2, 3, 4, 5 };

            Console.WriteLine("Any > 3 : {0}",
                numList5.Any(x => x > 3));

            // ---------- DISTINCT ----------
            // Eliminates duplicates from a list
            var numList6 = new List<int> { 1, 2, 3, 2, 3 };

            Console.WriteLine("Distinct : {0}",
                string.Join(", ", numList6.Distinct()));

            // ---------- EXCEPT ----------
            // Receives 2 lists and returns values not
            // found in the 2nd list
            var numList7 = new List<int> { 1, 2, 3, 2, 3 };
            var numList8 = new List<int> { 3 };

            Console.WriteLine("Except : {0}",
                string.Join(", ", numList7.Except(numList8)));

            // ---------- INTERSECT ----------
            // Receives 2 lists and returns values that
            // both lists have
            var numList9 = new List<int> { 1, 2, 3, 2, 3 };
            var numList10 = new List<int> { 2, 3 };

            Console.WriteLine("Intersect : {0}", string.Join(", ", numList9.Intersect(numList10)));


            // ----- NULLABLE TYPES -----
            // Data types by default cannot have a
            // value of null. Often null is needed
            // when you are working with databases,
            // and you can create a null type by 
            // adding a ? to the definition
            int? randNum = null;

            // Check for null
            if (randNum == null) Console.WriteLine("randNum is null");

            // Another check for null
            if (!randNum.HasValue) Console.WriteLine("randNum is null");


            Console.ReadLine();
        }


        // You can store any class as a base
        // class and call the subclass methods
        // even if they don't exist in the base
        // class by casting
        object circ1 = new Circle(4);
        var circ2 = (Circle)circ1;
        Console.WriteLine("The {0} Area is {1:f2}", circ2.Name, circ2.Area());
        Console.ReadLine();
    }

    public static void Add(double num1, double num2)
    {
        Console.WriteLine($"{num1} + {num2} = {num1 + num2}");
    }

    public static void Subtract(double num1, double num2)
    {
        Console.WriteLine($"{num1} - {num2} = {num1 - num2}");
    }

    // You can also create generic structs and classes in this same way
    public struct RectangleGeneric<T>
    {
        // Generic fields

        // Generic properties
        public T Width { get; set; }

        public T Length { get; set; }

        // Generic constructor
        public RectangleGeneric(T w, T l)
        {
            Width = w;
            Length = l;
        }

        public string GetArea()
        {
            var dblWidth = Convert.ToDouble(Width);
            var dblLength = Convert.ToDouble(Length);
            return string.Format($"{Width} * {Length} = {dblWidth * dblLength}");
        }
    }


    // private static void SayHello()
    // {
    //     Console.WriteLine("Hello, World!");
    // }

    // private static double GetSum(double a, double b)
    // {
    //     return a + b;
    // }

    // private static void DoubleIt(int number, out int result)
    // {
    //     result = number * 2;
    // }

    // private static void Swap(ref int a, ref int b)
    // {
    //     (a, b) = (b, a);
    // }

    // private static int GetSumMore(params int[] numbers)
    // {
    //     var sum = 0;
    //     foreach (var number in numbers) sum += number;
    //     return sum;
    // }

    // private static void PrintInfo(string name, int zipCode)
    // {
    //     Console.WriteLine("Name: {0}, ZipCode: {1}", name, zipCode);
    // }

    // private static double GetSum2(double a, double b)
    // {
    //     return a + b;
    // }

    // private static int GetSum2(int a, int b)
    // {
    //     return a + b;
    // }

    // private static int GetSum2(string a, string b)
    // {
    //     return int.Parse(a) + int.Parse(b);
    // }

    // private static void PaintCar(CarColor color)
    // {
    //     Console.WriteLine($"The car is painted {color}.");
    // }

    // ----- STRUCTS -----
    // A struct is a user defined type that contain multiple fields and methods
    // private struct Rectangle
    // {
    //     public double length;
    //     public double width;
    //
    //     // You can create a constructor method
    //     // that will set the values for fields
    //     public Rectangle(double l = 1, double w = 1)
    //     {
    //         length = l;
    //         width = w;
    //     }
    //
    //     public double Area()
    //     {
    //         return length * width;
    //     }
    // }

    private enum CarColor
    {
        Red,
        Blue,
        Green,
        Black
    }
}

// 6 done