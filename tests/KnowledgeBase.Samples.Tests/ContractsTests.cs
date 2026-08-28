using KnowledgeBase.Samples.Contracts;

namespace KnowledgeBase.Samples.Tests;

public sealed class ContractsTests
{
    [Fact]
    public void Factory_returns_device_through_abstraction()
    {
        IElectronicDevice device = TvRemote.GetDevice();
        Assert.IsType<Television>(device);
    }

    [Fact]
    public void PowerButton_execute_turns_on_and_undo_turns_off()
    {
        var tv = TvRemote.GetDevice();
        var power = new PowerButton(tv);

        power.Execute();
        Assert.True(tv.IsOn);

        power.Undo();
        Assert.False(tv.IsOn);
    }

    [Fact]
    public void Volume_is_clamped_at_the_top()
    {
        var tv = TvRemote.GetDevice();
        for (var i = 0; i < 150; i++)
        {
            tv.VolumeUp();
        }

        Assert.Equal(100, tv.Volume);
    }

    [Fact]
    public void Volume_is_clamped_at_the_bottom()
    {
        var tv = TvRemote.GetDevice();
        tv.VolumeUp();
        tv.VolumeDown();
        tv.VolumeDown();

        Assert.Equal(0, tv.Volume);
    }

    [Fact]
    public void Vehicle_move_reaches_max_speed_and_stop_resets()
    {
        IDrivable car = new Vehicle("Tesla", 4, 120);
        Assert.Equal(0, car.Speed);

        car.Move();
        Assert.Equal(120, car.Speed);

        car.Stop();
        Assert.Equal(0, car.Speed);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Vehicle_rejects_non_positive_wheel_counts(int wheels)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Vehicle("Tesla", wheels, 120));
    }

    [Fact]
    public void Vehicle_rejects_blank_brand()
    {
        Assert.Throws<ArgumentException>(() => new Vehicle("  ", 4, 120));
    }

    [Fact]
    public void Vehicle_speed_is_queried_via_interface()
    {
        IDrivable car = new Vehicle("Honda", 4, 90);
        car.Move();
        Assert.Equal(90, car.Speed);
    }
}