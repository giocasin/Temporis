using NodaTime;
using Temporis.Core;
using Temporis.Core.DTOs;

namespace Temporis.NodaTimeAdapter;

public class NodaTimeTemporisService(IDateTimeZoneProvider tzdb) : ITemporisService
{
    private readonly IDateTimeZoneProvider _tzdb = tzdb ?? throw new ArgumentNullException(nameof(tzdb));
    public NodaTimeTemporisService() : this(DateTimeZoneProviders.Tzdb) { }

    private static LocalDateTime ToLocal(LocalDateTimeDto dto) =>
        new(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second);
    
    public ZoneConversionResult GetZoneConversion(LocalDateTimeDto local, string timeZoneId)
    {
        var zone = _tzdb[timeZoneId]; // potenzialmente KeyNotFound se id errato -> lasciamo propagare l'errore o gestire a monte
        var ldt = ToLocal(local);
        var map = zone.MapLocal(ldt); // MapLocal ritorna mapping con 0/1/2 target instants

        var res = new ZoneConversionResult();

        switch (map.Count)
        {
            case 0:
                return res;
            case 1:
            {
                var instant = zone.AtStrictly(ldt).ToInstant();
                res.InstantsIsoUtc.Add(instant.ToString()); // ISO
                return res;
            }
            default:
            {
                var first = map.First(); // earlier
                var last  = map.Last();  // later
                res.InstantsIsoUtc.Add(first.ToInstant().ToString());
                if (last.ToInstant() != first.ToInstant())
                    res.InstantsIsoUtc.Add(last.ToInstant().ToString());
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