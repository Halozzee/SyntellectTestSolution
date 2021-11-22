using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	/// <summary>
	/// Класс представляющий оболочку для взаимодействий Клиент-Сервер.
	/// </summary>
	public class CommunicationMessage
	{
		/// <summary>
		/// Сообщение ошибки, произошедшей на сервере.
		/// </summary>
		public string ExceptionMessage { get; set; }
		/// <summary>
		/// Статус ответа от сервера.
		/// </summary>
		public ResponseStatus ResponseStatus { get; set; }
		/// <summary>
		/// Полученный контент при корректном завершении операций.
		/// </summary>
		public string Content { get; set; }
	}
}
