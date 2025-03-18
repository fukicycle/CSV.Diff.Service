using CSV.Diff.Service.Domain;
using CSV.Diff.Service.Domain.Interfaces;

namespace CSV.Diff.Service.Infrastructure.LocalFiles;

public sealed class FileAppLogger : AppLoggerBase
{
    public FileAppLogger(IAppLoggerProvider provider) : base(provider)
    {
        
    }
    public override AppLoggingLevel EnableLoggingLevel()
    {
#if DEBUG
        return AppLoggingLevel.Info | AppLoggingLevel.Warn | AppLoggingLevel.Error | AppLoggingLevel.Debug;
#else
        return AppLoggingLevel.Info | AppLoggingLevel.Warn | AppLoggingLevel.Error;
#endif
    }
}
