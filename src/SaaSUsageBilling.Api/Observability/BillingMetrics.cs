using System.Diagnostics.Metrics;

namespace SaaSUsageBilling.Api.Observability;

/// <summary>
/// Centralized meters/counters for lightweight telemetry.
/// </summary>
public static class BillingMetrics
{
    private static readonly Meter Meter = new("SaaSUsageBilling.Api", "1.0.0");

    public static readonly Counter<long> Requests = Meter.CreateCounter<long>("requests_total");
    public static readonly Counter<long> UsageRecorded = Meter.CreateCounter<long>("usage_recorded_total");
    public static readonly Counter<long> InvoicesGenerated = Meter.CreateCounter<long>("invoices_generated_total");
}

