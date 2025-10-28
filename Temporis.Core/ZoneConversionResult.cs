namespace Temporis.Core;

public record ZoneConversionResult
{
    // Lista possibili istanti UTC (ISO-8601) corrispondenti al local datetime nella timezone
    public List<string> InstantsIsoUtc { get; init; }

    // true se ambigua (es. autunno fallback -> 2 possibili istanti)
    public bool IsAmbiguous => InstantsIsoUtc?.Count > 1;

    // true se "skipped" (es. spring forward -> nessun istante)
    public bool IsSkipped => InstantsIsoUtc?.Count == 0;
}