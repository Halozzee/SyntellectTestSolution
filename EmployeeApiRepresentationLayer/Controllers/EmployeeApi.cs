using Domain;
using EmployeeApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Domain.DataProtection;

namespace EmployeeApiRepresentationLayer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeeApi : ControllerBase
	{
		private readonly ILogger<EmployeeApi> _logger;
		private readonly ITextCrypter _textCrypter;

		public EmployeeApi(ILogger<EmployeeApi> logger, ITextCrypter textCrypter)
		{
			_logger = logger;
			_textCrypter = textCrypter;
		}

		[HttpGet]
		public string Get(string employeeFilterJson)
		{
			if (!String.IsNullOrEmpty(employeeFilterJson))
			{
				EmployeeFilter employeeFilter = JsonConvert.DeserializeObject<EmployeeFilter>(_textCrypter.Decrypt(employeeFilterJson));

				return _textCrypter.Crypt(JsonConvert.SerializeObject(BusinessLogicManager.GetEmployeesByCondition(employeeFilter)));
			}
			else
			{
				return _textCrypter.Crypt(JsonConvert.SerializeObject(BusinessLogicManager.GetAllEmployees()));
			}
		}

		[HttpPost]
		public string Post(string employeeJson)
		{
			Employee employee = JsonConvert.DeserializeObject<Employee>(_textCrypter.Decrypt(employeeJson));
			bool isSuccess = BusinessLogicManager.InsertEmployee(employee);
			return _textCrypter.Crypt((isSuccess ? JsonConvert.SerializeObject(employee) : "error"));
		}

		[HttpPut]
		public string Put(string employeeJson)
		{
			Employee employee = JsonConvert.DeserializeObject<Employee>(_textCrypter.Decrypt(employeeJson));
			bool isSuccess = BusinessLogicManager.UpdateEmployee(employee);
			return _textCrypter.Crypt(isSuccess.ToString());
		}

		[HttpDelete]
		public string Delete(string employeeId) 
		{
			bool isSuccess = BusinessLogicManager.DeleteEmployee(Convert.ToInt32(_textCrypter.Decrypt(employeeId)));
			return _textCrypter.Crypt(isSuccess.ToString());
		}
	}
}
