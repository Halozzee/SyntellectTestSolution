using Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeApiDataAccessLayer
{
	public static class DataAccessManager
	{
		#region TableColumnNames
		private const string _lastNameTableColumnName = "last_name";
		private const string _firstNameTableColumnName = "first_name";
		private const string _patronymicTableColumnName = "patronymic";
		private const string _birthDateTableColumnName = "birth_date";
		#endregion

		#region Configs
		private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;
        private static string _tableName = System.Configuration.ConfigurationManager.AppSettings["EmployeeTableName"];
        #endregion

        #region CRUD Functions
        public static IEnumerable<Employee> GetAllEmployees() 
		{
            IList<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

					string sqlExpression = "SELECT * " +
						$"FROM {_tableName}";

                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employeeToAdd = new Employee((int)reader["id"], (string)reader[_lastNameTableColumnName], (string)reader[_firstNameTableColumnName], 
                                    (string)reader[_patronymicTableColumnName], (DateTime)reader[_birthDateTableColumnName]);
                                employees.Add(employeeToAdd);
                            }
                        }
                    }
                }

                return employees;
            }
            catch (Exception) 
            {
                throw;
            }
        }
        public static IEnumerable<Employee> GetEmployeesByCondition(string whereCondition)
        {
            IList<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = "SELECT * " +
						$"FROM {_tableName} " +
						$"WHERE {whereCondition}";

                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employee employeeToAdd = new Employee((int)reader["id"], (string)reader[_lastNameTableColumnName], (string)reader[_firstNameTableColumnName],
                                    (string)reader[_patronymicTableColumnName], (DateTime)reader[_birthDateTableColumnName]);
                                employees.Add(employeeToAdd);
                            }
                        }
                    }
                }

                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool InsertEmployee(Employee employee) 
        {
            if (employee.Id != -1)
                throw new ArgumentException("This employee already has its own ID - it means that it is already stored in the DB!");

            try
            {
                int insertedId = -1;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = $"INSERT INTO {_tableName} ({_lastNameTableColumnName}, {_firstNameTableColumnName}, {_patronymicTableColumnName}, {_birthDateTableColumnName}) " +
						$"VALUES ('{employee.LastName}', '{employee.FirstName}', '{employee.Patronymic}', '{employee.BirthDate.ToString("yyyy-MM-dd")}'); " +
						$"SELECT CAST(scope_identity() AS int)";
                    
                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        insertedId = (int)(int?)command.ExecuteScalar();
                    }
                }

                employee.Id = insertedId;
                return insertedId != -1;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool UpdateEmployee(Employee employee)
        {
            try
            {
                int updatedRows = -1;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = $"UPDATE {_tableName} " +
						$"SET {_lastNameTableColumnName}='{employee.LastName}', {_firstNameTableColumnName}='{employee.FirstName}', " +
						$"{_patronymicTableColumnName}='{employee.Patronymic}', {_birthDateTableColumnName}='{employee.BirthDate.ToString("yyyy-MM-dd")}'" +
						$"WHERE id={employee.Id}";

                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        updatedRows = command.ExecuteNonQuery();
                    }
                }

                return updatedRows > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool DeleteEmployee(Employee employee)
        {
            try
            {
                int deletedRows = -1;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = $"DELETE FROM {_tableName} " +
                        $"WHERE id={employee.Id}";

                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        deletedRows = command.ExecuteNonQuery();
                    }
                }

                return deletedRows > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteEmployee(int employeeId)
        {
            try
            {
                int deletedRows = -1;
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = $"DELETE FROM {_tableName} " +
                        $"WHERE id={employeeId}";

                    using (SqlCommand command = new SqlCommand(sqlExpression, connection))
                    {
                        deletedRows = command.ExecuteNonQuery();
                    }
                }

                return deletedRows > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
