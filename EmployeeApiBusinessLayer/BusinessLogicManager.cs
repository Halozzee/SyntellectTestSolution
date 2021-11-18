using Domain;
using System;
using System.Collections.Generic;
using EmployeeApiDataAccessLayer;

namespace EmployeeApiBusinessLayer
{
	public static class BusinessLogicManager
	{
        public static IEnumerable<Employee> GetAllEmployees()
        {
            return DataAccessManager.GetAllEmployees();
        }
        public static IEnumerable<Employee> GetEmployeesByCondition(EmployeeFilter employeeFilter)
		{
			return DataAccessManager.GetEmployeesByCondition(FormConditionString(employeeFilter));
		}

		private static string FormConditionString(EmployeeFilter employeeFilter)
		{
			List<string> conditionList = new List<string>();

			if (employeeFilter.LastNameFilter != null)
			{
				conditionList.Add($"last_name LIKE '{employeeFilter.LastNameFilter}%'");
			}
			if (employeeFilter.FirstNameFilter != null)
			{
				conditionList.Add($"first_name LIKE '{employeeFilter.FirstNameFilter}%'");
			}
			if (employeeFilter.PatronymicFilter != null)
			{
				conditionList.Add($"patronymic LIKE '{employeeFilter.PatronymicFilter}%'");
			}

			if (employeeFilter.BeginDateFilter != DateTime.MinValue && employeeFilter.EndDateFilter != DateTime.MinValue)
			{
				conditionList.Add($"birth_date between '{employeeFilter.BeginDateFilter}' and '{employeeFilter.EndDateFilter}'");
			}
			else if (employeeFilter.BeginDateFilter != DateTime.MinValue)
			{
				conditionList.Add($"birth_date >= '{employeeFilter.BeginDateFilter}'");
			}
			else if (employeeFilter.EndDateFilter != DateTime.MinValue)
			{
				conditionList.Add($"birth_date <= '{employeeFilter.EndDateFilter}'");
			}

			return String.Join(" and ", conditionList);
		}

		public static bool InsertEmployee(Employee employee)
        {
            return DataAccessManager.InsertEmployee(employee);
        }
        public static bool UpdateEmployee(Employee employee)
        {
            return DataAccessManager.UpdateEmployee(employee);
        }
        public static bool DeleteEmployee(Employee employee)
        {
            return DataAccessManager.DeleteEmployee(employee);
        }
    }
}
