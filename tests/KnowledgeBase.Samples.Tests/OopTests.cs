using System.Text;
using KnowledgeBase.Samples.Oop;

namespace KnowledgeBase.Samples.Tests;

public sealed class OopTests
{
    [Theory]
    [InlineData("Whiskers4")]
    [InlineData("Mr. 1")]
    public void Animal_rejects_names_containing_digits(string name)
    {
        Assert.Throws<ArgumentException>(() => new Animal(name, "Meow"));
    }

    [Fact]
    public void Animal_rejects_null_names()
    {
        Assert.Throws<ArgumentNullException>(() => new Animal(null!, "Meow"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Animal_rejects_blank_names(string name)
    {
        Assert.Throws<ArgumentException>(() => new Animal(name, "Meow"));
    }

    [Fact]
    public void Animal_name_is_assignable_and_validates_later_writes()
    {
        var cat = new Animal("Whiskers", "Meow");
        cat.Name = "Whiskers II";
        Assert.Equal("Whiskers II", cat.Name);

        Assert.Throws<ArgumentException>(() => cat.Name = "Whisk3rs");
    }

    [Fact]
    public void Animal_health_check_uses_height_weight_ratio()
    {
        Assert.True(new Animal.AnimalHealth().HealthyWeight(11, 46));
        Assert.False(new Animal.AnimalHealth().HealthyWeight(2, 100));
    }

    [Fact]
    public void Dog_overrides_make_sound_and_prints_both_sounds()
    {
        var output = CaptureConsole(() => new Dog("Rex", "Woof", "Grrr").MakeSound());
        Assert.Contains("Rex says Woof and Grrr", output);
    }

    [Fact]
    public void Composition_carries_a_mutable_registration_record()
    {
        var cat = new Animal("Whiskers", "Meow");
        cat.IdInfo = cat.IdInfo with { Owner = "Registry" };

        Assert.Equal("Registry", cat.IdInfo.Owner);
    }

    [Fact]
    public void Static_counter_has_process_wide_scope()
    {
        var baseline = Animal.NumOfAnimals;
        _ = new Animal("A", "aa");
        _ = new Animal("B", "bb");

        Assert.Equal(baseline + 2, Animal.NumOfAnimals);
    }

    [Fact]
    public void Farm_appends_on_out_of_range_writes_and_can_foreach()
    {
        var farm = new AnimalFarm
        {
            [0] = new Animal("Cow", "Moo"),
            [1] = new Animal("Horse", "Neigh"),
            [2] = new Animal("Sheep", "Baa")
        };

        Assert.Equal(3, farm.Count);
        Assert.Equal(new[] { "Cow", "Horse", "Sheep" }, farm.Select(a => a.Name));
    }

    [Fact]
    public void Farm_replaces_existing_slots()
    {
        var farm = new AnimalFarm { [0] = new Animal("Cow", "Moo") };
        farm[0] = new Animal("Bull", "Moo");

        Assert.Single(farm);
        Assert.Equal("Bull", farm[0].Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Farm_rejects_non_contiguous_writes(int index)
    {
        var farm = new AnimalFarm();
        Assert.Throws<ArgumentOutOfRangeException>(() => farm[index] = new Animal("Cow", "Moo"));
    }

    private static string CaptureConsole(Action action)
    {
        var original = Console.Out;
        using var buffer = new StringWriter(new StringBuilder());
        Console.SetOut(buffer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }

        return buffer.ToString();
    }
}