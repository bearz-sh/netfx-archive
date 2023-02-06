using Microsoft.Extensions.Logging;

namespace Bearz.Extensions.Logging;

#pragma warning disable SA1629 // Documentation text should end with a period
public static class AppLog
{
    private static ILoggerFactory s_loggerFactory = LoggerFactory.Create(lb => lb.AddConsole());

    private static ILogger? s_log;

    internal static ILogger Logger
    {
        get
        {
            if (s_log is not null)
                return s_log;

            if (s_loggerFactory is null)
                throw new InvalidOperationException($"{nameof(ILoggerFactory)} is not set. Call {nameof(SetLoggerFactory)} first.");

            s_log = s_loggerFactory.CreateLogger("App");

            return s_log;
        }
    }

    public static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        s_loggerFactory = loggerFactory;
    }

    public static void SetLogger(ILogger logger)
    {
        s_log = logger;
    }

    public static IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return Logger.BeginScope(state);
    }

    public static IDisposable? BeginScope(string messageFormat, params object?[] args)
    {
        return Logger.BeginScope(messageFormat, args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example><c>logger.LogDebug(0, exception, "Error while processing request from {Address}", address)</c></example>
    public static void Debug(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Debug, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug(0, "Processing request from {Address}", address)</example>
    public static void Debug(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Debug, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug(exception, "Error while processing request from {Address}", address)</example>
    public static void Debug(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Debug, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a debug log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogDebug("Processing request from {Address}", address)</example>
    public static void Debug(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Debug, message, args);
    }

    //------------------------------------------TRACE------------------------------------------//

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace(0, exception, "Error while processing request from {Address}", address)</example>
    public static void Trace(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Trace, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace(0, "Processing request from {Address}", address)</example>
    public static void Trace(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Trace, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace(exception, "Error while processing request from {Address}", address)</example>
    public static void Trace(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Trace, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogTrace("Processing request from {Address}", address)</example>
    public static void Trace(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Trace, message, args);
    }

    //------------------------------------------INFORMATION------------------------------------------//

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation(0, exception, "Error while processing request from {Address}", address)</example>
    public static void Info(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Information, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation(0, "Processing request from {Address}", address)</example>
    public static void Info(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Information, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation(exception, "Error while processing request from {Address}", address)</example>
    public static void Info(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Information, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an informational log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogInformation("Processing request from {Address}", address)</example>
    public static void Info(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Information, message, args);
    }

    //------------------------------------------WARNING------------------------------------------//

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning(0, exception, "Error while processing request from {Address}", address)</example>
    public static void Warn(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Warning, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning(0, "Processing request from {Address}", address)</example>
    public static void Warn(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Warning, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning(exception, "Error while processing request from {Address}", address)</example>
    public static void Warn(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Warning, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a warning log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogWarning("Processing request from {Address}", address)</example>
    public static void Warn(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Warning, message, args);
    }

    //------------------------------------------ERROR------------------------------------------//

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError(0, exception, "Error while processing request from {Address}", address)</example>
    public static void Error(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Error, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError(0, "Processing request from {Address}", address)</example>
    public static void Error(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Error, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError(exception, "Error while processing request from {Address}", address)</example>
    public static void Error(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Error, exception, message, args);
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogError("Processing request from {Address}", address)</example>
    public static void Error(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Error, message, args);
    }

    //------------------------------------------CRITICAL------------------------------------------//

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical(0, exception, "Error while processing request from {Address}", address)</example>
    public static void Critical(EventId eventId, Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Critical, eventId, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical(0, "Processing request from {Address}", address)</example>
    public static void Critical(EventId eventId, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Critical, eventId, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical(exception, "Error while processing request from {Address}", address)</example>
    public static void Critical(Exception? exception, string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Critical, exception, message, args);
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="message">Format string of the log message in message template format. Example: <c>"User {User} logged in from {Address}"</c></param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <example>logger.LogCritical("Processing request from {Address}", address)</example>
    public static void Critical(string? message, params object?[] args)
    {
        Logger.Log(LogLevel.Critical, message, args);
    }
}