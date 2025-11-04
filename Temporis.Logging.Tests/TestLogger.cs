using Microsoft.Extensions.Logging;

namespace Temporis.Logging.Tests;

public class TestLogger : ILogger
{
    public List<dynamic> Scopes { get; } = [];
    public List<(LogLevel level, string message, EventId id, Exception ex)> Events { get; } = [];
    
    public IDisposable BeginScope<T>(T state)
    {
        Scopes.Add(state ?? throw new ArgumentNullException(nameof(state)));
        return new DisposableAction(() => Scopes.Remove(state));
    }
    
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        Events.Add((logLevel, message, eventId, exception));
    }
    
    private class DisposableAction(Action? onDispose) : IDisposable
    {
        private readonly Action _onDispose = onDispose ?? (() => { });
        public void Dispose() => _onDispose();
    }
}