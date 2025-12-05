using SaaSUsageBilling.Domain.Common;
using SaaSUsageBilling.Domain.ValueObjects;

namespace SaaSUsageBilling.Domain.Entities;

public enum InvoiceStatus { Draft, Finalized, Paid, Overdue, Voided }

/// <summary>
/// Comprehensive invoice with proration, taxes, discounts, and detailed line items.
/// </summary>
public sealed class Invoice : Entity
{
    public Guid SubscriptionId { get; private set; }
    public Period Period { get; private set; }
    public DateTimeOffset IssuedOn { get; private set; }
    public DateTimeOffset DueOn { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public Money Subtotal { get; private set; } = Money.Zero();
    public Money TaxTotal { get; private set; } = Money.Zero();
    public Money DiscountTotal { get; private set; } = Money.Zero();
    public Money Total { get; private set; } = Money.Zero();
    public decimal ProrationFactor { get; private set; } = 1.0m;
    public string? Notes { get; private set; }

    public IReadOnlyCollection<InvoiceLine> Lines => _lines.AsReadOnly();
    public IReadOnlyCollection<InvoiceTax> Taxes => _taxes.AsReadOnly();
    public IReadOnlyCollection<InvoiceDiscount> Discounts => _discounts.AsReadOnly();

    private readonly List<InvoiceLine> _lines = new();
    private readonly List<InvoiceTax> _taxes = new();
    private readonly List<InvoiceDiscount> _discounts = new();

    private Invoice() { }

    public Invoice(Guid subscriptionId, Period period, DateTimeOffset issuedOn, DateTimeOffset dueOn)
    {
        SubscriptionId = subscriptionId;
        Period = period;
        IssuedOn = issuedOn;
        DueOn = dueOn;
        Status = InvoiceStatus.Draft;
        ProrationFactor = 1.0m;
    }

    public void SetProrationFactor(decimal factor)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Cannot modify finalized invoice");
        if (factor < 0 || factor > 1) throw new ArgumentOutOfRangeException(nameof(factor));

        ProrationFactor = factor;
        RecalculateTotal();
    }

    public void AddLine(string description, Money amount, string? category = null)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Cannot modify finalized invoice");

        _lines.Add(new InvoiceLine(description, amount, category));
        Subtotal = Subtotal.Add(amount);
        RecalculateTotal();
    }

    public void AddTax(Tax tax, Money taxableAmount)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Cannot modify finalized invoice");

        var taxAmount = tax.CalculateTax(taxableAmount);
        _taxes.Add(new InvoiceTax(tax.Name, tax.Rate, taxAmount));
        TaxTotal = TaxTotal.Add(taxAmount);
        RecalculateTotal();
    }

    public void AddDiscount(Discount discount, Money originalAmount, DateTimeOffset appliedOn)
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Cannot modify finalized invoice");

        if (!discount.IsValid(appliedOn)) return;

        var discountAmount = originalAmount.Amount - discount.Apply(originalAmount, appliedOn).Amount;
        var discountMoney = new Money(discountAmount, originalAmount.Currency);

        _discounts.Add(new InvoiceDiscount(discount.Description, discountAmount, discountMoney));
        DiscountTotal = DiscountTotal.Add(discountMoney);
        RecalculateTotal();
    }

    public void SetNotes(string notes)
    {
        Notes = notes;
    }

    public void FinalizeInvoice()
    {
        if (Status != InvoiceStatus.Draft) throw new InvalidOperationException("Invoice already finalized");
        Status = InvoiceStatus.Finalized;
    }

    public void MarkAsPaid()
    {
        if (Status != InvoiceStatus.Finalized) throw new InvalidOperationException("Can only mark finalized invoices as paid");
        Status = InvoiceStatus.Paid;
    }

    public void MarkAsOverdue()
    {
        if (Status == InvoiceStatus.Paid || Status == InvoiceStatus.Voided) return;
        Status = InvoiceStatus.Overdue;
    }

    public void Void()
    {
        if (Status == InvoiceStatus.Paid) throw new InvalidOperationException("Cannot void paid invoice");
        Status = InvoiceStatus.Voided;
    }

    private void RecalculateTotal()
    {
        // Apply proration to subtotal first
        var proratedSubtotal = new Money(Subtotal.Amount * ProrationFactor, Subtotal.Currency);

        // Apply discounts to prorated subtotal
        var discountedSubtotal = proratedSubtotal.Subtract(DiscountTotal.Amount);

        // Add taxes to discounted amount
        Total = discountedSubtotal.Add(TaxTotal);
    }
}

public sealed record InvoiceLine(string Description, Money Amount, string? Category = null);

public sealed record InvoiceTax(string Name, decimal Rate, Money Amount);

public sealed record InvoiceDiscount(string Description, decimal Amount, Money MoneyAmount);
