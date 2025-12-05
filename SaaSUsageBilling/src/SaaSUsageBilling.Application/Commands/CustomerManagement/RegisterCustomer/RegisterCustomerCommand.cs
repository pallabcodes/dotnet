using MediatR;

namespace SaaSUsageBilling.Application.Commands.CustomerManagement.RegisterCustomer;

public record RegisterCustomerCommand(string Name, string Email) : IRequest<Guid>;

