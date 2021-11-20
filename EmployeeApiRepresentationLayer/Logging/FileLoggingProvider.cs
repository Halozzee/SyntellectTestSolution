using Microsoft.Extensions.Logging;

namespace EmployeeApiRepresentationLayer.Logging
{
    /// <summary>
    /// Provider class to add file logging
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        private string path;
        public FileLoggerProvider(string _path)
        {
            path = _path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        {
        }
    }
}
