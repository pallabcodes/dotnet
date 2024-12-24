using System.Runtime.Serialization;

namespace ConsoleApp1;

// Implementing ISerializable to allow custom serialization and deserialization

// With serialization, you can store the state of an object in a file stream, pass it toa remote network
public class AnimalSerialized : ISerializable
{
    // Default constructor (optional, but useful for deserialization)
    public AnimalSerialized()
    {
    }

    // Constructor to initialize properties with default or provided values
    public AnimalSerialized(string name = "No Name", double weight = 0, double height = 0)
    {
        Name = name;
        Weight = weight;
        Height = height;
    }

    // The deserialize constructor (Removes Object Data from File)
    // Called when deserializing the object
    public AnimalSerialized(SerializationInfo info, StreamingContext ctxt)
    {
        // Get the values from info and assign them to the properties
        Name = (string)info.GetValue("Name", typeof(string));
        Weight = (double)info.GetValue("Weight", typeof(double));
        Height = (double)info.GetValue("Height", typeof(double));
        AnimalID = (int)info.GetValue("AnimalID", typeof(int));
    }

    public string Name { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public int AnimalID { get; set; }

    // Serialization function (Stores Object Data in File)
    // SerializationInfo holds the key-value pairs
    // StreamingContext can hold additional info, but we aren't using it here
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // Assign key-value pairs for your data
        info.AddValue("Name", Name);
        info.AddValue("Weight", Weight);
        info.AddValue("Height", Height);
        info.AddValue("AnimalID", AnimalID);
    }

    // Override ToString for easy representation of the object
    public override string ToString()
    {
        return string.Format("{0} weighs {1} lbs and is {2} inches tall", Name, Weight, Height);
    }
}