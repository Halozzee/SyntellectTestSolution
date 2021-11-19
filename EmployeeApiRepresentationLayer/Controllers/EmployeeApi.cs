using Domain;
using EmployeeApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EmployeeApiRepresentationLayer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeeApi : ControllerBase
	{
		private readonly ILogger<EmployeeApi> _logger;

		public EmployeeApi(ILogger<EmployeeApi> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public string Get(string employeeFilterJson)
		{
			if (!String.IsNullOrEmpty(employeeFilterJson))
			{
				EmployeeFilter employeeFilter = JsonConvert.DeserializeObject<EmployeeFilter>(employeeFilterJson);

				return JsonConvert.SerializeObject(BusinessLogicManager.GetEmployeesByCondition(employeeFilter));
			}
			else
			{
				return JsonConvert.SerializeObject(BusinessLogicManager.GetAllEmployees());
			}
		}

		[HttpPost]
		public string Post(string employeeJson)
		{
			Employee employee = JsonConvert.DeserializeObject<Employee>(employeeJson);
			bool isSuccess = BusinessLogicManager.InsertEmployee(employee);
			return (isSuccess ? JsonConvert.SerializeObject(employee) : "error");
		}

		[HttpPut]
		public bool Put(string employeeJson)
		{
			Employee employee = JsonConvert.DeserializeObject<Employee>(employeeJson);
			bool isSuccess = BusinessLogicManager.UpdateEmployee(employee);
			return isSuccess;
		}

		[HttpDelete]
		public bool Delete(int employeeId) 
		{
			bool isSuccess = BusinessLogicManager.DeleteEmployee(employeeId);
			return isSuccess;
		}
	}
}
