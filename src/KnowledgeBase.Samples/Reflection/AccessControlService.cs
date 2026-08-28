using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace KnowledgeBase.Samples.Reflection;

/// <summary>
/// Declares which "subject" a class represents (e.g. 'admin' or 'default').
/// AttributeUsage restricts it to types, so it cannot be misapplied.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class RoleAttribute : Attribute
{
    public RoleAttribute(string role) => Role = role;

    public string Role { get; }
}

/// <summary>
/// Declares the role required to invoke an individual method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class AuthorizeAttribute : Attribute
{
    public AuthorizeAttribute(string requiredRole) => RequiredRole = requiredRole;

    public string RequiredRole { get; }
}

/// <summary>
/// Attribute-driven, reflection-based access control.
///
/// Production note: ASP.NET Core already provides this via policies +
/// IAuthorizationHandler, and hand-rolled reflection interception is not what
/// you want at scale. The value here is pedagogical: it shows the exact
/// mechanism (metadata discovery + invocation) that framework-level auth sits on.
/// </summary>
public sealed class AccessControlService
{
    // Reflection is expensive; MethodInfo discovery is cached per (type, method).
    // ConcurrentDictionary gives safe, lock-free reads under concurrency.
    private readonly ConcurrentDictionary<(Type Type, string MethodName), MethodInfo> _methodCache = new();

    public async Task<object?> InvokeAsync(object controller, string methodName)
    {
        ArgumentNullException.ThrowIfNull(controller);

        var method = _methodCache.GetOrAdd(
            (controller.GetType(), methodName),
            static key => key.Type.GetMethod(key.MethodName)
                ?? throw new MissingMethodException(key.Type.FullName, key.MethodName));

        EnforceAuthorization(controller.GetType(), method);

        object? result;
        try
        {
            result = method.Invoke(controller, null);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            // Find the case and rethrow the caller's original exception,
            // preserving its original stack trace, instead of leaking a wrapper.
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw; // Not reached.
        }

        return await UnwrapAsync(result).ConfigureAwait(false);
    }

    private static void EnforceAuthorization(Type controllerType, MethodInfo method)
    {
        var guard = method.GetCustomAttribute<AuthorizeAttribute>(inherit: true);
        if (guard is null)
        {
            return; // Public method without a guard: no authorization required.
        }

        var subjectRole = controllerType.GetCustomAttribute<RoleAttribute>(inherit: true);
        if (subjectRole is null ||
            !string.Equals(subjectRole.Role, guard.RequiredRole, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException(
                $"Access denied for role '{subjectRole?.Role ?? "none"}'; '{guard.RequiredRole}' is required.");
        }
    }

    /// <summary>
    /// Flattens an invoked method's result: Task-returning methods are awaited
    /// and their Result surfaced; plain values pass through unchanged.
    /// </summary>
    private static async Task<object?> UnwrapAsync(object? result)
    {
        if (result is null)
        {
            return null;
        }

        if (result is Task task)
        {
            await task.ConfigureAwait(false);
            return task.GetType().GetProperty(nameof(Task<object>.Result))?.GetValue(task);
        }

        return result;
    }
}