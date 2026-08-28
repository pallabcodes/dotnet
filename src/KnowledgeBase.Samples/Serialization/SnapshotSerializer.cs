using System.Text.Json;

namespace KnowledgeBase.Samples.Serialization;

/// <summary>Immutable snapshot of an animal; serialized with System.Text.Json.</summary>
public sealed record AnimalSnapshot(string Name, double WeightKg, double HeightCm, int ShelterId);

/// <summary>
/// Modern serialization with System.Text.Json. BinaryFormatter and direct
/// ISerializable are obsolete/disabled for security (unsafe type deserialization)
/// and should never be used in new code.
/// </summary>
public static class SnapshotSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string Serialize(AnimalSnapshot snapshot) => JsonSerializer.Serialize(snapshot, Options);

    public static AnimalSnapshot Deserialize(string json) =>
        JsonSerializer.Deserialize<AnimalSnapshot>(json, Options)
        ?? throw new InvalidOperationException("Deserialization returned null.");
}