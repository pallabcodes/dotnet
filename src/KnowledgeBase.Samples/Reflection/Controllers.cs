namespace KnowledgeBase.Samples.Reflection;

/// <summary>A controller available to users with the default role.</summary>
[Role("default")]
public class BaseController
{
    /// <summary>Guarded: the subject's role must match 'default'.</summary>
    [Authorize("default")]
    public virtual string GetData() => "Base data";

    /// <summary>Guarded, and asynchronous: the enforcer must flatten the Task.</summary>
    [Authorize("default")]
    public virtual async Task<string> ProcessDataAsync()
    {
        await Task.Delay(10);
        return "Processed base data";
    }

    /// <summary>Deliberately unguarded: any role may call this.</summary>
    public virtual string Ping() => "pong";

    /// <summary>Guarded by 'admin': a default-role user must be denied.</summary>
    [Authorize("admin")]
    public virtual string AdminOnly() => "Super secret admin data";
}

/// <summary>
/// A controller for admins. Class-level RoleAttribute declares the subject;
/// overridden methods re-declare stricter guards by matching lifecycles.
/// </summary>
[Role("admin")]
public sealed class AdminController : BaseController
{
    [Authorize("admin")]
    public override string GetData() => "Admin data";

    [Authorize("admin")]
    public override Task<string> ProcessDataAsync() => Task.FromResult("Processed admin data");

    public override string Ping() => "pong (admin)";
}