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
			string decryptedEmployeeFilterJson = employeeFilterJson != null ? _textCrypter.Decrypt(employeeFilterJson) : "";

			try
			{
				if (!String.IsNullOrEmpty(employeeFilterJson))
				{
					EmployeeFilter employeeFilter = JsonConvert.DeserializeObject<EmployeeFilter>(decryptedEmployeeFilterJson);

					communicationMessage.Content = JsonConvert.SerializeObject(BusinessLogicManager.GetEmployeesByCondition(employeeFilter));
					_logger.LogInformation("GetRequestSelectWithFilter", decryptedEmployeeFilterJson);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
				}
				else
				{
					communicationMessage.Content = JsonConvert.SerializeObject(BusinessLogicManager.GetAllEmployees());
					communicationMessage.ResponseStatus = ResponseStatus.Success;
					_logger.LogInformation("GetRequestSelectWithNoFilter");
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ExceptionMessage = ex.Message;
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				_logger.LogError(ex, $"GetRequestError \t StackTrace: {ex.StackTrace}", decryptedEmployeeFilterJson);
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpPost]
		public string Post(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeJson = _textCrypter.Decrypt(employeeJson);

			try
			{
				Employee employee = JsonConvert.DeserializeObject<Employee>(decryptedEmployeeJson);
				bool isSuccess = BusinessLogicManager.InsertEmployee(employee);

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(employee);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
					_logger.LogInformation("PostRequestSuccess", decryptedEmployeeJson, communicationMessage.Content);
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
					_logger.LogInformation("PostRequestNoRowAffected", decryptedEmployeeJson, communicationMessage.Content);
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ExceptionMessage = ex.Message;
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				_logger.LogError(ex, "PostRequestError", decryptedEmployeeJson);
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpPut]
		public string Put(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeJson = _textCrypter.Decrypt(employeeJson);

			try
			{
				Employee employee = JsonConvert.DeserializeObject<Employee>(decryptedEmployeeJson);
				bool isSuccess = BusinessLogicManager.UpdateEmployee(employee);

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(isSuccess);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
					_logger.LogInformation("PutRequestSuccess", decryptedEmployeeJson);
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
					_logger.LogInformation("PutRequestNoRowAffected", decryptedEmployeeJson);
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ExceptionMessage = ex.Message;
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				_logger.LogError(ex, "PutRequestError", decryptedEmployeeJson);
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}

		[HttpDelete]
		public string Delete(string employeeId) 
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeId = _textCrypter.Decrypt(employeeId);

			try
			{
				bool isSuccess = BusinessLogicManager.DeleteEmployee(Convert.ToInt32(decryptedEmployeeId));

				if (isSuccess)
				{
					communicationMessage.Content = JsonConvert.SerializeObject(isSuccess);
					communicationMessage.ResponseStatus = ResponseStatus.Success;
					_logger.LogInformation("DeleteRequestSuccess", decryptedEmployeeId);
				}
				else
				{
					communicationMessage.Content = "No exceptions accured, but no row was affected.";
					communicationMessage.ResponseStatus = ResponseStatus.Fail;
					_logger.LogInformation("DeleteRequestNoRowAffected", decryptedEmployeeId);
				}
			}
			catch (Exception ex)
			{
				communicationMessage.ExceptionMessage = ex.Message;
				communicationMessage.ResponseStatus = ResponseStatus.Exception;
				_logger.LogError(ex, "DeleteRequestError", decryptedEmployeeId);
			}

			return _textCrypter.Crypt(JsonConvert.SerializeObject(communicationMessage));
		}
	}
}
