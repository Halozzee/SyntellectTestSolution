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

		/// <summary>
		/// Получение списка сотрудников.
		/// </summary>
		/// <param name="employeeFilterJson">Полученный через QueryString зашифрованный(если включено шифрование) фильтр сотрудников в формате Json. Если null - происходит выбор всех сотрудников.</param>
		/// <returns>
		/// <para>CommunicationMessage.Content = Список отфильтрованных сотрудников в формате Json. Если employeeFilterJson == null, тогда возвращает всех сотрудников в формате Json.</para>
		/// <para>
		///		CommunicationMessage.ResponseStatus = Success, если список получен и находится в CommunicationMessage.Content.
		///		CommunicationMessage.ResponseStatus = Exception, если произошла ошибка. Сообщение ошибки находится в CommunicationMessage.ExceptionMessage.
		/// </para>
		/// </returns>
		[HttpGet]
		public string Get(string employeeFilterJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeFilterJson = "";

			try
			{
				decryptedEmployeeFilterJson = employeeFilterJson != null ? _textCrypter.Decrypt(employeeFilterJson) : "";
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

		/// <summary>
		/// Вставка сотрудника в БД.
		/// </summary>
		/// <param name="employeeJson">Полученный через QueryString зашифрованный(если включено шифрование) сотрудник в формате Json.</param>
		/// <returns>
		/// <para>CommunicationMessage.Content = Сотрудник в формате Json.</para>
		/// <para>
		///		CommunicationMessage.ResponseStatus = Success	, если сотрудник вставлен и находится в CommunicationMessage.Content значение True.
		///		CommunicationMessage.ResponseStatus = Fail		, если сотрудник не был вставлен и в CommunicationMessage.Content находится дублирование этого комментария.
		///		CommunicationMessage.ResponseStatus = Exception	, если произошла ошибка. Сообщение ошибки находится в CommunicationMessage.ExceptionMessage.
		/// </para>
		/// </returns>
		[HttpPost]
		public string Post(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeJson = "";

			try
			{
				decryptedEmployeeJson = _textCrypter.Decrypt(employeeJson);

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

		/// <summary>
		/// Обновление данных сотрудника в БД.
		/// </summary>
		/// <param name="employeeJson">Полученный через QueryString зашифрованный(если включено шифрование) сотрудник в формате Json.</param>
		/// <returns>
		/// <para>CommunicationMessage.Content = True или False. True - изменена хотя бы одна строка, False - ни одна строка не была измена.</para>
		/// <para>
		///		CommunicationMessage.ResponseStatus = Success	, если сотрудник изменен и находится в CommunicationMessage.Content значение True.
		///		CommunicationMessage.ResponseStatus = Fail		, если сотрудник не был изменен и в CommunicationMessage.Content находится дублирование этого комментария.
		///		CommunicationMessage.ResponseStatus = Exception	, если произошла ошибка. Сообщение ошибки находится в CommunicationMessage.ExceptionMessage.
		/// </para>
		/// </returns>
		[HttpPut]
		public string Put(string employeeJson)
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeJson = "";

			try
			{
				decryptedEmployeeJson = _textCrypter.Decrypt(employeeJson);

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

		/// <summary>
		/// Удаление данных сотрудника в БД.
		/// </summary>
		/// <param name="employeeId">Полученный через QueryString зашифрованный(если включено шифрование) Id сотрудника в формате числа.</param>
		/// <returns>
		/// <para>CommunicationMessage.Content = True или False. True - изменена хотя бы одна строка, False - ни одна строка не была измена.</para>
		/// <para>
		///		CommunicationMessage.ResponseStatus = Success	, если сотрудник удален и находится в CommunicationMessage.Content значение True.
		///		CommunicationMessage.ResponseStatus = Fail		, если сотрудник не был удален и в CommunicationMessage.Content находится дублирование этого комментария.
		///		CommunicationMessage.ResponseStatus = Exception	, если произошла ошибка. Сообщение ошибки находится в CommunicationMessage.ExceptionMessage.
		/// </para>
		/// </returns>
		[HttpDelete]
		public string Delete(string employeeId) 
		{
			CommunicationMessage communicationMessage = new CommunicationMessage();
			string decryptedEmployeeId = "";

			try
			{
				decryptedEmployeeId = _textCrypter.Decrypt(employeeId);
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
