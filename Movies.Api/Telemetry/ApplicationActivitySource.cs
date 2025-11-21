using System.Diagnostics;

namespace Movies.Api.Telemetry;

public static class ApplicationActivitySource
{
    private static readonly ActivitySource Source = new("Movies.Api", "1.0.0");

    public static ActivitySource GetSource() => Source;

    public static Activity? StartActivity(string name, ActivityKind kind = ActivityKind.Internal)
    {
        return Source.StartActivity(name, kind);
    }
}

