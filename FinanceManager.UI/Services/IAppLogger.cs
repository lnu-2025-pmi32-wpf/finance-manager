namespace FinanceManager.UI.Services
{
    public enum LogLevel { Trace, Info, Warning, Error }

    public interface IAppLogger
    {
        void Log(LogLevel level, string message);
        void Info(string message) => Log(LogLevel.Info, message);
        void Trace(string message) => Log(LogLevel.Trace, message);
        void Warning(string message) => Log(LogLevel.Warning, message);
        void Error(string message) => Log(LogLevel.Error, message);
    }
}
