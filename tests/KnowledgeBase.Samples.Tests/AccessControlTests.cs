using KnowledgeBase.Samples.Reflection;

namespace KnowledgeBase.Samples.Tests;

public sealed class AccessControlTests
{
    private readonly AccessControlService _enforcer = new();
    private readonly BaseController _user = new();
    private readonly AdminController _admin = new();

    [Fact]
    public async Task Matching_role_is_authorized()
    {
        Assert.Equal("Base data", await _enforcer.InvokeAsync(_user, "GetData"));
        Assert.Equal("Admin data", await _enforcer.InvokeAsync(_admin, "GetData"));
    }

    [Fact]
    public async Task Unguarded_methods_are_accessible_to_any_role()
    {
        Assert.Equal("pong", await _enforcer.InvokeAsync(_user, "Ping"));
        Assert.Equal("pong (admin)", await _enforcer.InvokeAsync(_admin, "Ping"));
    }

    [Fact]
    public async Task Async_methods_are_awaited_and_their_result_surfaced()
    {
        Assert.Equal("Processed base data", await _enforcer.InvokeAsync(_user, "ProcessDataAsync"));
        Assert.Equal("Processed admin data", await _enforcer.InvokeAsync(_admin, "ProcessDataAsync"));
    }

    [Fact]
    public async Task Role_mismatch_is_rejected_with_unauthorized()
    {
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _enforcer.InvokeAsync(_user, "AdminOnly"));

        Assert.Contains("'admin' is required", ex.Message);
    }

    [Fact]
    public async Task Missing_role_on_the_subject_class_is_rejected()
    {
        object orphan = new NoRoleController();
        var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _enforcer.InvokeAsync(orphan, "Secured"));

        Assert.Contains("role 'none'", ex.Message);
    }

    [Fact]
    public async Task Unknown_method_raises_missing_method_info()
    {
        await Assert.ThrowsAsync<MissingMethodException>(
            () => _enforcer.InvokeAsync(_user, "DoesNotExist"));
    }

    [Fact]
    public async Task Exceptions_inside_the_invoked_method_are_not_wrapped()
    {
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _enforcer.InvokeAsync(new ThrowingController(), "Crash"));

        Assert.Equal("boom", ex.Message);
    }

    [Fact]
    public async Task Second_invocation_reuses_the_cached_method_info()
    {
        Assert.Equal("Base data", await _enforcer.InvokeAsync(_user, "GetData"));
        Assert.Equal("Admin data", await _enforcer.InvokeAsync(_admin, "GetData"));
        Assert.Equal("Base data", await _enforcer.InvokeAsync(new BaseController(), "GetData"));
    }

    [Fact]
    public async Task Null_controller_is_rejected()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _enforcer.InvokeAsync(null!, "GetData"));
    }

    private sealed class NoRoleController
    {
        [Authorize("admin")]
        public string Secured() => "n/a";
    }

    [Role("default")]
    private sealed class ThrowingController
    {
        [Authorize("default")]
        public string Crash() => throw new InvalidOperationException("boom");
    }
}