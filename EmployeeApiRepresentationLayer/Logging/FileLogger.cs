using System;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace EmployeeApiRepresentationLayer.Logging
{
	/// <summary>
	/// File logger class
	/// </summary>
    public class FileLogger : ILogger
	{
		private string filePath;

		private static object _lock = new object();

		public FileLogger(string path)
		{
			filePath = path;
		}
		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (formatter != null)
			{
				lock (_lock)
				{
					DateTime dtNow = DateTime.Now;
					string s = "";

					if (state.ToString() == "Application is starting...")
						s = $"{Environment.NewLine}{Environment.NewLine}";

					string textToAppend = $"{s}{$"{dtNow}   "}{formatter(state, exception)}{Environment.NewLine}";

					Type tState = state.GetType();
					var values = (object[])tState.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)[1].GetValue(state);

					if (values.Length > 0)
					{
						textToAppend += $"\tparam objects: {Environment.NewLine}";

						int objectCounter = 0;
						foreach (var item in values)
						{
							textToAppend += $"\t\t{objectCounter++} : {item.ToString()}{Environment.NewLine}";
						}

						textToAppend += $"{Environment.NewLine}";
					}

					File.AppendAllText(filePath, textToAppend);
				}
			}
		}
	}

}
