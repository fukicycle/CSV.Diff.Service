namespace CSV.Diff.Service.Domain.Interfaces;

public interface IAppLogger
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message);
    void LogDebug(string message);
}