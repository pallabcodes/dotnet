using MediatR;
using SaaSUsageBilling.Application.Abstractions;
using SaaSUsageBilling.Domain.Entities;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Application.Commands.Billing.GenerateInvoice;

public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, Invoice>
{
    private readonly ISubscriptionRepository _subscriptions;
    private readonly IPlanRepository _plans;
    private readonly IUsageEventRepository _usageEvents;
    private readonly IInvoiceRepository _invoices;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateInvoiceCommandHandler(
        ISubscriptionRepository subscriptions,
        IPlanRepository plans,
        IUsageEventRepository usageEvents,
        IInvoiceRepository invoices,
        IUnitOfWork unitOfWork)
    {
        _subscriptions = subscriptions;
        _plans = plans;
        _usageEvents = usageEvents;
        _invoices = invoices;
        _unitOfWork = unitOfWork;
    }

    public async Task<Invoice> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var subscription = await _subscriptions.GetAsync(request.SubscriptionId, cancellationToken)
                ?? throw new InvalidOperationException("Subscription not found.");

            var plan = await _plans.GetAsync(subscription.PlanId, cancellationToken)
                ?? throw new InvalidOperationException("Plan not found.");

            var period = Period.CurrentMonthUtc();
            if (!period.Contains(request.NowUtc))
            {
                period = new Period(
                    new DateTimeOffset(request.NowUtc.Year, request.NowUtc.Month, 1, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(request.NowUtc.Year, request.NowUtc.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(1));
            }

            // Calculate proration if subscription started mid-period
            var prorationFactor = subscription.CalculateProrationFactor(period.From, period.To);

            var usageEvents = await _usageEvents.GetForPeriodAsync(subscription.Id, period.From, period.To, cancellationToken);
            var totalUnits = usageEvents.Sum(u => u.Quantity);

            var issuedOn = DateTimeOffset.UtcNow;
            var dueOn = issuedOn.AddDays(30); // 30-day payment terms

            var invoice = new Invoice(subscription.Id, period, issuedOn, dueOn);
            invoice.SetProrationFactor(prorationFactor);

            // Calculate usage costs using the plan's pricing rules
            var usageCost = plan.CalculateUsageCost(totalUnits, issuedOn);
            var proratedBase = new Money(plan.MonthlyBase.Amount * prorationFactor, plan.MonthlyBase.Currency);

            invoice.AddLine("Base subscription", proratedBase, "subscription");
            if (usageCost.Amount > 0)
            {
                invoice.AddLine($"Usage charges ({totalUnits} units)", usageCost, "usage");
            }

            // Apply taxes
            var taxableAmount = proratedBase.Add(usageCost);
            foreach (var tax in plan.ApplicableTaxes)
            {
                invoice.AddTax(tax, taxableAmount);
            }

            // Apply discounts
            foreach (var discount in plan.Discounts)
            {
                invoice.AddDiscount(discount, taxableAmount, issuedOn);
            }

            invoice.FinalizeInvoice();
            await _invoices.AddAsync(invoice, cancellationToken);

            subscription.ResetPeriod(period, issuedOn);
            await _subscriptions.UpdateAsync(subscription, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return invoice;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

