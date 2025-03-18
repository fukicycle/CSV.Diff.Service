namespace CSV.Diff.Service.Domain;

[Flags]
public enum AppLoggingLevel
{
    Info = 1,
    Warn = 2,
    Error = 4,
    Debug = 8
}