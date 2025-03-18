namespace CSV.Diff.Service.Domain.Interfaces;

public abstract class AppLoggerBase : IAppLogger
{
    private readonly string FORMAT = "{0:yyyy-MM-dd HH:mm:ss}\t[{1}]\t{2}";
    private readonly IAppLoggerProvider _provider;
    private readonly AppLoggingLevel _enableLevel;

    protected AppLoggerBase(IAppLoggerProvider provider)
    {
        _provider = provider;
        _enableLevel = EnableLoggingLevel();
    }

    public abstract AppLoggingLevel EnableLoggingLevel();

    private void LoggingCore(AppLoggingLevel level, string message)
    {
        string logMessage = Format(level, message);
        _provider.WriteLine(logMessage);
    }

    private string Format(AppLoggingLevel level, string message)
    {
        return string.Format(FORMAT, DateTime.Now, level, message);
    }

    public void LogDebug(string message)
    {
        if (_enableLevel.HasFlag(AppLoggingLevel.Debug))
        {
            LoggingCore(AppLoggingLevel.Debug, message);
        }
    }

    public void LogError(string message)
    {
        if (_enableLevel.HasFlag(AppLoggingLevel.Error))
        {
            LoggingCore(AppLoggingLevel.Error, message);
        }
    }

    public void LogInformation(string message)
    {
        if (_enableLevel.HasFlag(AppLoggingLevel.Info))
        {
            LoggingCore(AppLoggingLevel.Info, message);
        }
    }

    public void LogWarning(string message)
    {
        if (_enableLevel.HasFlag(AppLoggingLevel.Warn))
        {
            LoggingCore(AppLoggingLevel.Warn, message);
        }
    }
}