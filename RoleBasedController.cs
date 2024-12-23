namespace ConsoleApp1;

// Custom attributes for role-based access control

// AttributeUsage is an inbuilt class from `System` package
// AttributeTargets.Class | AttributeTargets.Method means that this attribute could be used on class and method both
// All custom attributes should be extended from its base class i.e. Attribute
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleAttribute : Attribute
{
    public RoleAttribute(string role)
    {
        Role = role;
    }

    public string Role { get; }
}

// This Attribute should only be used for method
[AttributeUsage(AttributeTargets.Method)]
public class ValidateRoleAttribute : Attribute
{
    public ValidateRoleAttribute(string requiredRole)
    {
        RequiredRole = requiredRole;
    }

    public string RequiredRole { get; }
}

// N.B: It's a convention to remove suffix e.g., RoleAttribute to Role when using it (which is what done below)
// Although, using RoleAttribute below would be just fine

// Base controller
[Role("default")]
public class BaseController
{
    [ValidateRole("default")]
    public virtual string GetData()
    {
        return "Base data";
    }

    [ValidateRole("default")]
    public virtual async Task<string> ProcessDataAsync()
    {
        await Task.Delay(100); // Simulate async operation
        return "Processed base data";
    }
}

// Subclass with custom roles
[Role("admin")]
public class AdminController : BaseController
{
    [ValidateRole("admin")]
    public override string GetData()
    {
        return "Admin data";
    }

    [ValidateRole("admin")]
    public override async Task<string> ProcessDataAsync()
    {
        return "Processed admin data";
    }
}

// Attribute validation logic (Reflection-based)

// N.B: By default attributes only adds properties or methods to a class (nothing else) which is why Reflection/Proxy pattern used to check whether that added property and method exists and then validate
public class AttributeEnforcer
{
    public static async Task<string> InvokeMethodAsync(object controller, string methodName)
    {
        // Get the method info using reflection
        var method = controller.GetType().GetMethod(methodName);
        if (method == null) throw new Exception($"Method {methodName} not found.");

        // Get the class-level role
        var classRoleAttribute = controller.GetType().GetCustomAttributes(typeof(RoleAttribute), true)
            .Cast<RoleAttribute>()
            .FirstOrDefault();

        // Get the method-level role
        var methodRoleAttribute = method.GetCustomAttributes(typeof(ValidateRoleAttribute), true)
            .Cast<ValidateRoleAttribute>()
            .FirstOrDefault();

        if (methodRoleAttribute != null && classRoleAttribute != null)
            // Enforce role validation
            if (classRoleAttribute.Role != methodRoleAttribute.RequiredRole)
                throw new UnauthorizedAccessException(
                    $"Access denied. Required role: {methodRoleAttribute.RequiredRole}");

        // Invoke the method
        var result = method.Invoke(controller, null);
        if (result is Task<string> asyncResult) return await asyncResult;
        return result as string;
    }
}