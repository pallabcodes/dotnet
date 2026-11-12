using System.Diagnostics.Metrics;

namespace Template.Api.Observability;

public static class TemplateMetrics
{
    private static readonly Meter Meter = new("Template.Api", "1.0.0");

    public static readonly Counter<long> Requests = Meter.CreateCounter<long>("requests_total");
}
