using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using Domain.DataProtection.Interfaces;
using Domain.DataProtection.Implementations;
using System.IO;
using EmployeeApiRepresentationLayer.Logging;

namespace EmployeeApiRepresentationLayer
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			if (System.Configuration.ConfigurationManager.AppSettings["IsUsingEncryption"] == "True")
			{
				services.AddSingleton<ITextCrypter>(new SimpleCrypter());
			}
			else
			{
				services.AddSingleton<ITextCrypter>(new EmptyCrypter());
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			string currentDirPath = Directory.GetCurrentDirectory();

			if (!Directory.Exists("Logs"))
				Directory.CreateDirectory("Logs");

			loggerFactory.AddFile(Path.Combine(currentDirPath, $"Logs\\LogFile {DateTime.Now.ToString().Replace(":","_")}.txt"));

			var logger = loggerFactory.CreateLogger("FileLogger");

			logger.LogInformation("Application is starting...");

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
