using Domain.EmployeeObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeApiDataAccessLayer
{
	public static class DataAccessManager
	{
        //Имена колонок в таблице. Менят можно, если они меняются в таблице. Добавлять можно, но нужно обновлять тогда и в CRUD методах.
        //Если изменять придется больше 3 раз => сделать через рефлексию (поиск по этому классу по приватным константам).
		#region TableColumnNames
		private const string _lastNameTableColumnName = "last_name";
		private const string _firstNameTableColumnName = "first_name";
		private const string _patronymicTableColumnName = "patronymic";
		private const string _birthDateTableColumnName = "birth_date";
		#endregion

        //Конфигурации из файла App.Config
		#region Configs
		private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQLConnectionString"].ConnectionString;
        private static string _tableName = System.Configuration.ConfigurationManager.AppSettings["EmployeeTableName"];
        #endregion

        #region CRUD Functions
        /// <summary>
        /// Получить список всех сотрудников.
        /// </summary>
        /// <returns>Возвращает список всех сотрудников.</returns>
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

        /// <summary>
        /// Получить список всех сотрудников по строке условия.
        /// </summary>
        /// <param name="conditions">Строка условия, формируемая через инстанцию SqlConditionStringBuilder метод Build(). Можно указывать и свою строку.</param>
        /// <returns>Отфильтрованный список сотрудников.</returns>
        public static IEnumerable<Employee> GetEmployeesByCondition(string conditions)
        {
            IList<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string sqlExpression = "SELECT * " +
                        $"FROM {_tableName} {conditions}";

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
        /// <summary>
        /// Вставить сотрудника в БД.
        /// </summary>
        /// <param name="employee">Сотрудник для вставки.</param>
        /// <returns>True - хотя бы одна строка изменилась, False - ни одна строка не была изменена.</returns>
        /// <exception cref="ArgumentException">Неправильно указан ID сотрудника. Если он не -1, значит сотрудник уже был вставлен в таблицу.</exception>
        public static bool InsertEmployee(Employee employee) 
        {
            if (employee.Id != -1)
                throw new ArgumentException("У этого сотрудника уже есть свой Id - это значит, что он уже был вставлен в таблицу.");

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
        /// <summary>
        /// Обновить данные о сотруднике.
        /// </summary>
        /// <param name="employee">Сотрудник, данные которого будут обновлены. Должен быть правильный Id!</param>
        /// <returns>True - хотя бы одна строка изменилась, False - ни одна строка не была изменена.</returns>
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
        /// <summary>
        /// Удалить сотрудника в БД. 
        /// </summary>
        /// <param name="employee">Сотрудник, данные которого будут удалены. Должен быть правильный Id!</param>
        /// <returns>True - хотя бы одна строка изменилась, False - ни одна строка не была изменена.</returns>
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

        /// <summary>
        /// Удалить сотрудника в БД. 
        /// </summary>
        /// <param name="employeeId">Id сотрудника, данные которого будут удалены. Должен быть правильный Id!</param>
        /// <returns>True - хотя бы одна строка изменилась, False - ни одна строка не была изменена.</returns>
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
