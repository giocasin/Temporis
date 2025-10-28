using FluentAssertions;
using Temporis.Core.DTOs;
using Temporis.NodaTimeAdapter;
using Xunit;

namespace Temporis.Core.Tests;

public class ZoneConversionTests
{
    [Fact]
    public void Given_LocalDateTime_EuropeRomeAutumn20180230_ShouldBe_Ambiguous()
    {
        var local = new LocalDateTimeDto(2018, 10, 28, 2, 30, 0); // 2018-10-28 02:30 (ambiguous in Europe/Rome)
        var zoneConversion = new NodaTimeTemporisService();
        zoneConversion.IsAmbiguous(local, "Europe/Rome").Should().BeTrue();
        zoneConversion.IsSkipped(local, "Europe/Rome").Should().BeFalse();
    }

    [Fact]
    public void Given_LocalDateTime_EuropeRomeSprint20180325_ShouldBe_Skipped()
    {
        var local = new LocalDateTimeDto(2018, 3, 25, 2, 30, 0);
        var zoneConversion = new NodaTimeTemporisService();
        zoneConversion.IsAmbiguous(local, "Europe/Rome").Should().BeFalse();
        zoneConversion.IsSkipped(local, "Europe/Rome").Should().BeTrue();
    }
}