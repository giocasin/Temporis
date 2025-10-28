using Temporis.Core.DTOs;

namespace Temporis.Core;

public interface ITemporisService
{
    /// <summary>
    /// Given a LocalDateTimeDto and a timeZoneId (IANA), returns UTC dates in ISO-8601 and checks for ambiguity or skipped times.
    /// </summary>
    ZoneConversionResult GetZoneConversion(LocalDateTimeDto local, string timeZoneId);
    
    bool IsAmbiguous(LocalDateTimeDto local, string timeZoneId);

    bool IsSkipped(LocalDateTimeDto local, string timeZoneId);
}