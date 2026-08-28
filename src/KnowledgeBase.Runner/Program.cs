using KnowledgeBase.Samples.Concurrency;
using KnowledgeBase.Samples.Contracts;
using KnowledgeBase.Samples.Delegates;
using KnowledgeBase.Samples.Generics;
using KnowledgeBase.Samples.Oop;
using KnowledgeBase.Samples.Operators;
using KnowledgeBase.Samples.Polymorphism;
using KnowledgeBase.Samples.Reflection;
using KnowledgeBase.Samples.Serialization;
using KnowledgeBase.Samples.Simulation;

namespace KnowledgeBase.Runner;

/// <summary>
/// Ordered walkthrough of every sample in the KnowledgeBase library.
/// Each demo is a thin presentation layer over pure, testable domain code.
/// </summary>
public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            await RunAllAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Demo suite failed: {ex}");
            return 1;
        }
    }

    private static async Task RunAllAsync()
    {
        RunOop();
        RunContracts();
        RunPolymorphism();
        RunOperators();
        RunGenerics();
        RunDelegates();
        RunSimulation();
        await RunReflectionAsync();
        await RunConcurrencyAsync();
        RunSerialization();
    }

    private static void Header(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 72));
        Console.WriteLine(title);
        Console.WriteLine(new string('=', 72));
    }

    private static void RunOop()
    {
        Header("1. OOP: encapsulation, inheritance, composition, IEnumerable + indexer");

        var farm = new AnimalFarm
        {
            [0] = new Animal("Cow", "Moo"),
            [1] = new Animal("Horse", "Neigh"),
            [2] = new Dog("Rex", "Woof", "Grrr")
        };

        foreach (var animal in farm)
        {
            Console.Write($"[{animal.IdNumber}] {animal.Name} -> ");
            animal.MakeSound();
        }

        farm[0].IdInfo = farm[0].IdInfo with { Owner = "Farm registry" };
        Console.WriteLine($"Farm holds {farm.Count} animals; total animals created in process: {Animal.NumOfAnimals}");
    }

    private static void RunContracts()
    {
        Header("2. Contracts: interfaces, Command pattern, and a simple factory");

        var tv = TvRemote.GetDevice();
        var power = new PowerButton(tv);

        power.Execute();
        Console.WriteLine($"Power pressed -> TV on: {tv.IsOn}");
        power.Undo();
        Console.WriteLine($"Undo pressed -> TV on: {tv.IsOn}");

        tv.VolumeUp();
        tv.VolumeUp();
        Console.WriteLine($"Volume after two presses: {tv.Volume}");

        IDrivable car = new Vehicle("Tesla", 4, 120);
        car.Move();
        Console.WriteLine($"Driving: {car}");
        car.Stop();
        Console.WriteLine($"Stopped: {car}");
    }

    private static void RunPolymorphism()
    {
        Header("3. Polymorphism: abstract Shape family, dispatch by runtime type");

        Shape[] shapes = [new Circle(5), new Rectangle(4, 6)];
        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.Describe()} -> area {shape.Area():F2}");
        }
    }

    private static void RunOperators()
    {
        Header("4. Operators: overloaded +, -, casts, and a consistent equality contract");

        var box = new Box(2, 3, 4);
        var cube = (Box)5;
        Console.WriteLine($"{box} + {cube} = {box + cube}");
        Console.WriteLine($"Average side of {box + cube} as int: {(int)(box + cube)}");
        Console.WriteLine($"hash({box}) == hash({new Box(2, 3, 4)}): {box.GetHashCode() == new Box(2, 3, 4).GetHashCode()}");
    }

    private static void RunGenerics()
    {
        Header("5. Generics: static-abstract INumber<T> without boxing");

        Console.WriteLine($"Add(3, 4) = {Numeric.Add(3, 4)}");
        Console.WriteLine($"Add(3.5, 4.25) = {Numeric.Add(3.5, 4.25)}");
        Console.WriteLine($"Sum([1..10]) = {Numeric.Sum(Enumerable.Range(1, 10))}");
    }

    private static void RunDelegates()
    {
        Header("6. Delegates & events: multicast, opt-in subscription");

        var bus = new ChannelBus();
        bus.Published += (_, message) => Console.WriteLine($"[{message.Channel}] {message.Content}");
        bus.Publish("system", "first subscriber notified");
        bus.Publish("system", "second message, still just one subscriber");
    }

    private static void RunSimulation()
    {
        Header("7. Simulation: inheritance + strategy (teleport) with injected randomness");

        var rng = new RandomGenerator();
        var thor = new Warrior("Thor", 100, 26, 10, rng);
        var loki = new MagicWarrior("Loki", 75, 20, 10, 50, rng, new CanTeleport());

        var fight = BattleArena.Fight(thor, loki);
        Console.WriteLine($"The fight lasted {fight.Count} rounds. ");
        Console.WriteLine($"Thor alive: {thor.IsAlive} (health {thor.Health:F0}) | Loki alive: {loki.IsAlive} (health {loki.Health:F0})");

        var last = fight.LastOrDefault();
        if (last is not null)
        {
            Console.WriteLine($"Last round -> {last.Attacker} dealt {last.DamageDealt:F0} damage; {last.Defender} has {last.DefenderHealthRemaining:F0} health");
        }
    }

    private static async Task RunReflectionAsync()
    {
        Header("8. Reflection + attributes: metadata-driven access control");

        var enforcer = new AccessControlService();
        var user = new BaseController();
        var admin = new AdminController();

        Console.WriteLine($"user.GetData()   -> {await enforcer.InvokeAsync(user, "GetData")}");
        Console.WriteLine($"user.Ping()      -> {await enforcer.InvokeAsync(user, "Ping")}");
        Console.WriteLine($"user.Async       -> {await enforcer.InvokeAsync(user, "ProcessDataAsync")}");
        Console.WriteLine($"admin.GetData()  -> {await enforcer.InvokeAsync(admin, "GetData")}");
        Console.WriteLine($"admin.AdminOnly() -> {await enforcer.InvokeAsync(admin, "AdminOnly")}");

        try
        {
            await enforcer.InvokeAsync(user, "AdminOnly");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"user.AdminOnly()  -> DENIED: {ex.Message}");
        }
    }

    private static async Task RunConcurrencyAsync()
    {
        Header("9. Concurrency: TAP, fan-out, cancellation, bounded parallelism");

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var items = new[]
        {
            new WorkItem("fetch user", TimeSpan.FromMilliseconds(30)),
            new WorkItem("fetch orders", TimeSpan.FromMilliseconds(20)),
            new WorkItem("fetch invoices", TimeSpan.FromMilliseconds(40))
        };

        var all = await WorkloadRunner.RunAllAsync(items, cts.Token);
        Console.WriteLine($"RunAllAsync completed: {string.Join(", ", all)}");

        var bounded = await WorkloadRunner.RunBoundedAsync(items, 2, cts.Token);
        Console.WriteLine($"RunBoundedAsync(max 2) completed: {string.Join(", ", bounded)}");
    }

    private static void RunSerialization()
    {
        Header("10. Serialization: System.Text.Json records round-trip");

        var original = new AnimalSnapshot("Bowser", 45, 25, 1);
        var json = SnapshotSerializer.Serialize(original);
        Console.WriteLine(json);

        var restored = SnapshotSerializer.Deserialize(json);
        Console.WriteLine($"Round-trip equal: {restored == original} -> {restored}");
    }
}