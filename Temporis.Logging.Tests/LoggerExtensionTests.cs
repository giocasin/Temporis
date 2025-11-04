using FluentAssertions;
using Microsoft.Extensions.Logging;
using Temporis.Core;
using Temporis.Core.DTOs;
using Xunit;
using Temporis.Logging;

namespace Temporis.Logging.Tests;

public class LoggerExtensionTests
{
    [Fact]
    public void BeginTemporisScopeShouldAddScopeProperties()
    {
        var logger = new TestLogger();
        var fake = new FakeTemporisService
        {
            ResultToReturn = new ZoneConversionResult { InstantsIsoUtc = new List<string> { "2018-10-28T01:30:00Z", "2018-10-28T02:30:00Z" } }
        };
        var local = new LocalDateTimeDto(2018, 10, 28, 2, 30, 0);

        logger.BeginTemporisScope(fake, local, "Europe/Rome"); 
        logger.LogInformation("hello");
        
        logger.Scopes.Should().NotBeEmpty();
        var dict = logger.Scopes[^1];

        ((string)dict["temporis_timezone_id"]).Should().Be("Europe/Rome");
        ((bool)dict["temporis_is_ambiguous"]).Should().Be(true);
        ((List<string>)dict["temporis_timestamp_instants"])
            .Should().Contain("2018-10-28T01:30:00Z").And.Contain("2018-10-28T02:30:00Z");
    }
}