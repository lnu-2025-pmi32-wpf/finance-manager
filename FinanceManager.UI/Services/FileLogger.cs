using System;
using System.IO;
using System.Text;
using System.Threading;

namespace FinanceManager.UI.Services
{
    public class FileLogger : IAppLogger, IDisposable
    {
        private readonly string _logDir;
        private readonly string _logFilePath;
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(1, 1);

        public FileLogger()
        {
            _logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            Directory.CreateDirectory(_logDir);
            _logFilePath = Path.Combine(_logDir, "app.log");
        }

        public void Log(LogLevel level, string message)
        {
            var line = $"[{DateTime.UtcNow:O}] {level.ToString().ToUpper()} {message}{Environment.NewLine}";
            _sem.Wait();
            try
            {
                File.AppendAllText(_logFilePath, line, Encoding.UTF8);
            }
            catch
            {
                // swallow file IO errors to avoid crashing the app
            }
            finally
            {
                _sem.Release();
            }
        }

        public void Dispose()
        {
            _sem?.Dispose();
        }
    }
}
