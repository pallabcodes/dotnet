namespace SaaSUsageBilling.Application.Contracts;

public record RegisterCustomerRequest(string Name, string Email);
public record CreatePlanRequest(string Name, string? Description, decimal MonthlyBase, int IncludedUnits, decimal PricePerUnit);
public record StartSubscriptionRequest(Guid CustomerId, Guid PlanId);
public record RecordUsageRequest(Guid SubscriptionId, int Quantity, DateTimeOffset OccurredAt);
public record GenerateInvoiceRequest(Guid SubscriptionId, DateTimeOffset NowUtc);
