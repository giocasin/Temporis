using NodaTime;
using Temporis.Core;
using Temporis.Core.DTOs;

namespace Temporis.NodaTimeAdapter;

public class NodaTimeTemporisService(IDateTimeZoneProvider tzdb) : ITemporisService
{
    private readonly IDateTimeZoneProvider _tzdb = tzdb ?? throw new ArgumentNullException(nameof(tzdb));
    public NodaTimeTemporisService() : this(DateTimeZoneProviders.Tzdb) { }
    
    // For testability

    private LocalDateTime ToLocal(LocalDateTimeDto dto)
        => new LocalDateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second);
    
    public ZoneConversionResult GetZoneConversion(LocalDateTimeDto local, string timeZoneId)
    {
        var zone = _tzdb[timeZoneId]; // potenzialmente KeyNotFound se id errato -> lasciamo propagare l'errore o gestire a monte
        var ldt = ToLocal(local);
        var map = zone.MapLocal(ldt); // MapLocal ritorna mapping con 0/1/2 target instants

        var res = new ZoneConversionResult();

        switch (map.Count)
        {
            case 0:
                // skipped => nessun instant
                return res;
            case 1:
            {
                var instant = zone.AtStrictly(ldt).ToInstant();
                res.InstantsIsoUtc.Add(instant.ToString()); // ISO
                return res;
            }
            default:
            {
                // safer: use ResolveAmbiguous
                // var resolvedEarlier = zone.ResolveAmbiguous(ldt).Earlier;
                // var resolvedLater = zone.ResolveAmbiguous(ldt).Later;
                // res.InstantsIsoUtc.Add(resolvedEarlier.ToInstant().ToString());
                // res.InstantsIsoUtc.Add(resolvedLater.ToInstant().ToString());
                return res;
            }
        }
    }

    public bool IsAmbiguous(LocalDateTimeDto local, string timeZoneId)
    {
        var zone = _tzdb[timeZoneId];
        var ldt = ToLocal(local);
        return zone.MapLocal(ldt).Count == 2;
    }

    public bool IsSkipped(LocalDateTimeDto local, string timeZoneId)
    {
        var zone = _tzdb[timeZoneId];
        var ldt = ToLocal(local);
        return zone.MapLocal(ldt).Count == 0;
    }
}