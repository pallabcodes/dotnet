using KnowledgeBase.Samples.Serialization;

namespace KnowledgeBase.Samples.Tests;

public sealed class SerializationTests
{
    [Fact]
    public void Round_trip_preserves_record_equality()
    {
        var original = new AnimalSnapshot("Bowser", 45, 25, 1);

        var json = SnapshotSerializer.Serialize(original);
        var restored = SnapshotSerializer.Deserialize(json);

        Assert.Equal(original, restored);
    }

    [Fact]
    public void Output_is_indented_camel_case_json()
    {
        var json = SnapshotSerializer.Serialize(new AnimalSnapshot("Bowser", 45, 25, 1));

        Assert.Contains("\"name\": \"Bowser\"", json);
        Assert.Contains("\"shelterId\": 1", json);
    }

    [Fact]
    public void Camel_case_json_can_be_read_back()
    {
        const string json = """{"name":"Bowser","weightKg":45,"heightCm":25,"shelterId":1}""";

        var restored = SnapshotSerializer.Deserialize(json);

        Assert.Equal(new AnimalSnapshot("Bowser", 45, 25, 1), restored);
    }

    [Fact]
    public void Multiple_snapshots_round_trip_as_a_list()
    {
        var list = new[]
        {
            new AnimalSnapshot("Mario", 60, 30, 2),
            new AnimalSnapshot("Luigi", 55, 24, 3)
        };

        var json = System.Text.Json.JsonSerializer.Serialize(list);
        var restored = System.Text.Json.JsonSerializer.Deserialize<AnimalSnapshot[]>(json);

        Assert.Equal(list, restored);
    }
}