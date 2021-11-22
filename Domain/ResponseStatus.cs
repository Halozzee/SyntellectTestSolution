namespace Domain
{
	/// <summary>
	/// Статус ответа от сервера
	/// </summary>
	public enum ResponseStatus
	{
		/// <summary>
		/// Не указан (нужен для дебага и других принципов описанных в книге Complete Code)
		/// </summary>
		None,
		/// <summary>
		/// Все операции на сервере прошли успешно.
		/// </summary>
		Success,
		/// <summary>
		/// Ошибки не произошло, но получен неудовлетворительный результат.
		/// </summary>
		Fail,
		/// <summary>
		/// На сервере произошла ошибка.
		/// </summary>
		Exception
	}
}
