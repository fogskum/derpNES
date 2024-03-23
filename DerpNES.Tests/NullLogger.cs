using Microsoft.Extensions.Logging;

namespace DerpNES.Tests;

internal class NullLogger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
        => new NullScope();

    public bool IsEnabled( LogLevel logLevel ) => false;

    public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter )
    {
    }

    private class NullScope : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
