using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.EmployeeObjects
{
	public class EmployeeFilter
	{
		public string LastNameFilter { get; set; }
		public string FirstNameFilter { get; set; }
		public string PatronymicFilter { get; set; }
		public DateTime BeginBirthDateFilter { get; set; }
		public DateTime EndBirthDateFilter { get; set; }
		public PaginationData PaginationData { get; set; }

		public EmployeeFilter() 
		{
			PaginationData = new PaginationData();
		}

		public bool HasAtleastOneFieldToFilterWith
		{
			get 
			{
				return !(String.IsNullOrEmpty(LastNameFilter) && String.IsNullOrEmpty(FirstNameFilter) && String.IsNullOrEmpty(PatronymicFilter)
					&& BeginBirthDateFilter == DateTime.MinValue && EndBirthDateFilter == DateTime.MinValue) || PaginationData.HasConditionToWorkWith;
			}
		}
	}
}
