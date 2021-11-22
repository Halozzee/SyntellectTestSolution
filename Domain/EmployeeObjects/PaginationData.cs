namespace Domain.EmployeeObjects
{
	/// <summary>
	/// Данные для пагинации.
	/// </summary>
	public class PaginationData
	{
		public int PaginationIndex { get; set; }
		public int PaginationCount { get; set; }

		/// <summary>
		/// Есть корректные данные для работы с пагинацией.
		/// </summary>
		public bool HasConditionToWorkWith 
		{ 
			get => (PaginationIndex > -1 && PaginationCount > 0);
		}

		public PaginationData() 
		{
			PaginationIndex = int.MinValue;
			PaginationCount = int.MinValue;
		}
	}
}