using Microsoft.Extensions.Logging;
using Temporis.Core;
using Temporis.Core.DTOs;

namespace Temporis.Logging;

public static class LoggerExtension
{
    public static IDisposable BeginTemporisScope(this ILogger logger, ITemporisService service, LocalDateTimeDto local,
        string timeZoneId)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(service);
        ArgumentNullException.ThrowIfNull(local);
        if (string.IsNullOrWhiteSpace(timeZoneId)) throw new ArgumentNullException(nameof(timeZoneId));
        
        var conv = service.GetZoneConversion(local, timeZoneId);
        
        var dict = new Dictionary<string, object>
        {
            ["temporis_timestamp_instants"] = conv.InstantsIsoUtc,
            ["temporis_timezone_id"] = timeZoneId,
            ["temporis_local_datetime"] = local.ToString(),
            ["temporis_is_ambiguous"] = conv.IsAmbiguous,
            ["temporis_is_skipped"] = conv.IsSkipped
        };

        return logger.BeginScope(dict) ?? throw new InvalidOperationException();
    }
    
    public static void LogChrono(this ILogger logger, LogLevel level, ITemporisService service, LocalDateTimeDto local, string timeZoneId, string message, params object[] args)
    {
        using (logger.BeginTemporisScope(service, local, timeZoneId))
        {
            logger.Log(level, message, args);
        }
    }
}