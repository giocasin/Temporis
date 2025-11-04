using Temporis.Core;
using Temporis.Core.DTOs;

namespace Temporis.Logging.Tests;

public class FakeTemporisService : ITemporisService
{
    public ZoneConversionResult ResultToReturn { get; set; } = new ZoneConversionResult { InstantsIsoUtc = new List<string> { "2018-10-28T01:30:00Z" } };

    public ZoneConversionResult GetZoneConversion(LocalDateTimeDto local, string timeZoneId) => ResultToReturn;

    public bool IsAmbiguous(LocalDateTimeDto local, string timeZoneId) => ResultToReturn.IsAmbiguous;

    public bool IsSkipped(LocalDateTimeDto local, string timeZoneId) => ResultToReturn.IsSkipped;
}