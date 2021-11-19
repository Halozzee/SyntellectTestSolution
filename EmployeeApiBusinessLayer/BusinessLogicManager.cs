using System.Collections.Generic;
using EmployeeApiDataAccessLayer;
using Domain.EmployeeObjects;

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
            SqlWhereConditionStringBuilder sqlWhereConditionStringBuilder = new SqlWhereConditionStringBuilder(employeeFilter);
            string condition = sqlWhereConditionStringBuilder.Build();

            return DataAccessManager.GetEmployeesByCondition(condition);
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

        public static bool DeleteEmployee(int employeeId)
        {
            return DataAccessManager.DeleteEmployee(employeeId);
        }
    }
}
