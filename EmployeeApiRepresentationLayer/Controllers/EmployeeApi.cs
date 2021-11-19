using EmployeeApiBusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Domain.DataProtection.Interfaces;
using Domain.EmployeeObjects;
using Domain.EmployeeObjects;
using Domain;

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
			CommunicationMessage communicationMessage = new CommunicationMessage();
			try
			{
				if (!String.IsNullOrEmpty(employeeFilterJson))
				{
					EmployeeFilter employeeFilter = JsonConvert.DeserializeObject<EmployeeFilter>(_textCrypter.Decrypt(employeeFilterJson));

					communicationMessage.Content = JsonConvert.SerializeObject(BusinessLogicManager.GetEmployeesByCondition(employeeFilter));
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
				else
				{
					communicationMessage.Content = JsonConvert.SerializeObject(BusinessLogicManager.GetAllEmployees());
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				communicationMessage.ExceptionMessage = ex.Message;
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpPost]
		public string Post(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();

			try
			{
				Employee employee = JsonConvert.DeserializeObject<Employee>(_textCrypter.Decrypt(employeeJson));
				bool isSuccess = BusinessLogicManager.InsertEmployee(employee);

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(employee);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				communicationMessage.ExceptionMessage = ex.Message;
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpPut]
		public string Put(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();

			try
			{
				Employee employee = JsonConvert.DeserializeObject<Employee>(_textCrypter.Decrypt(employeeJson));
				bool isSuccess = BusinessLogicManager.UpdateEmployee(employee);

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(isSuccess);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				communicationMessage.ExceptionMessage = ex.Message;
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpDelete]
		public string Delete(string employeeId) 
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();

			try
			{
				bool isSuccess = BusinessLogicManager.DeleteEmployee(Convert.ToInt32(_textCrypter.Decrypt(employeeId)));

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(isSuccess);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				communicationMessage.ExceptionMessage = ex.Message;
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}
	}
}
