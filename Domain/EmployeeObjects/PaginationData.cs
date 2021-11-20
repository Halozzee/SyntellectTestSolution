namespace Domain.EmployeeObjects
{
	public class PaginationData
	{
		public int PaginationIndex { get; set; }
		public int PaginationCount { get; set; }

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