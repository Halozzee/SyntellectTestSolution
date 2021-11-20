using Microsoft.Extensions.Logging;

namespace EmployeeApiRepresentationLayer.Logging
{
    /// <summary>
    /// Extension class to add file logging
    /// </summary>
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));

            return factory;
        }
    }
}
