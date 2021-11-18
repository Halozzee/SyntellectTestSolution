using EmployeeApiDataAccessLayer;
using Domain;
using System;

namespace TestConsole
{
	public class Program
	{
		static void Main(string[] args)
		{
			var employees = EmployeeApiDataAccessLayer.DataAccessManager.GetAllEmployees();
			var emp = new Employee("Smith", "Patrick", "Robirtovich", new DateTime(1991, 12, 23));
			DataAccessManager.InsertEmployee(emp);

			//emp.LastName = "Tantrum";
			//emp.FirstName = "Forte";
			//emp.BirthDate = new DateTime(2020, 9, 9);

			DataAccessManager.UpdateEmployee(emp);
		}
	}
}
