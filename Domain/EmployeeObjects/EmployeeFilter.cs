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
	}
}
