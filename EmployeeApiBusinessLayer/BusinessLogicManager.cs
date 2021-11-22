using System.Collections.Generic;
using EmployeeApiDataAccessLayer;
using Domain.EmployeeObjects;

namespace EmployeeApiBusinessLayer
{
    /// <summary>
    /// Главный класс Бизнес Слоя. Позволяет слою представления взаимодействовать со слоем данных. Выполняет некоторые преобразования перед тем, как обращаться к слою данных. Ошибки не обрабатывает.
    /// </summary>
	public static class BusinessLogicManager
	{
        /// <summary>
        /// Получить список всех сотрудников.
        /// </summary>
        /// <returns>Список всех сотрудников.</returns>
        public static IEnumerable<Employee> GetAllEmployees()
        {
            return DataAccessManager.GetAllEmployees();
        }
        /// <summary>
        /// Получить список сотрудников по указанному фильтру.
        /// </summary>
        /// <param name="employeeFilter">Фильтр.</param>
        /// <returns>Список всех сотрудников, соответствующих указанному фильтру.</returns>
        public static IEnumerable<Employee> GetEmployeesByCondition(EmployeeFilter employeeFilter)
		{
            SqlConditionStringBuilder sqlConditionStringBuilder = new SqlConditionStringBuilder(employeeFilter);
            string conditions = sqlConditionStringBuilder.Build();

            return DataAccessManager.GetEmployeesByCondition(conditions);
		}

        /// <summary>
        /// Вставить сотрудника в БД.
        /// </summary>
        /// <param name="employee">Сотрудник для вставки. Сотруднику назначается новый Id, который потом виден вне метода (меняется у ссылочного типа).</param>
        /// <returns>True - вставилась хотя бы одна строка, False - ни одна строка не была изменена.</returns>
		public static bool InsertEmployee(Employee employee)
        {
            return DataAccessManager.InsertEmployee(employee);
        }

        /// <summary>
        /// Вставить сотрудника в БД.
        /// </summary>
        /// <param name="employee">Новые данные сотрудника для вставки. Должен быть правильный Id (по нему обновляется) - остальное обновиться.</param>
        /// <returns>True - обновилась хотя бы одна строка, False - ни одна строка не была изменена.</returns>
        public static bool UpdateEmployee(Employee employee)
        {
            return DataAccessManager.UpdateEmployee(employee);
        }

        /// <summary>
        /// Удалить сотрудника в БД.
        /// </summary>
        /// <param name="employee">Данные сотрудника для удаления. Должен быть правильный Id (по нему удаляется).</param>
        /// <returns>True - обновилась хотя бы одна строка, False - ни одна строка не была изменена.</returns>
        public static bool DeleteEmployee(Employee employee)
        {
            return DataAccessManager.DeleteEmployee(employee);
        }

        /// <summary>
        /// Удалить сотрудника в БД.
        /// </summary>
        /// <param name="employeeId">Id сотрудника, который будет удален.</param>
        /// <returns>True - обновилась хотя бы одна строка, False - ни одна строка не была изменена.</returns>
        public static bool DeleteEmployee(int employeeId)
        {
            return DataAccessManager.DeleteEmployee(employeeId);
        }
    }
}
