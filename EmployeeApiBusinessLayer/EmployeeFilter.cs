using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeApiBusinessLayer
{
	public class EmployeeFilter
	{
		public string LastNameFilter { get; set; }
		public string FirstNameFilter { get; set; }
		public string PatronymicFilter { get; set; }
		public DateTime BeginDateFilter { get; set; }
		public DateTime EndDateFilter { get; set; }
	}
}
