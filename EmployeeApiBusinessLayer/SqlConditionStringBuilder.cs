using Domain.EmployeeObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmployeeApiBusinessLayer
{
	/// <summary>
	/// Класс Builder для создания условий по фильтру сотрудников.
	/// </summary>
	public class SqlConditionStringBuilder
	{
		private readonly EmployeeFilter _employeeFilter;
		public SqlConditionStringBuilder(EmployeeFilter employeeFilter) 
		{
			_employeeFilter = employeeFilter;
		}

		/// <summary>
		/// Сформировать список условий по полям фильтра.
		/// </summary>
		/// <returns>Список условий по каждому полю.</returns>
		private List<string> FormConditionList()
		{
			List<string> conditionList = new List<string>();
			Type t = _employeeFilter.GetType();

			PropertyInfo[] props = t.GetProperties();

			foreach (var item in props)
			{
				switch (item.PropertyType.ToString())
				{
					case "System.String":
						string propValue = (string)item.GetValue(_employeeFilter);
						if(!String.IsNullOrEmpty(propValue))
							conditionList.Add($"{GetDataBaseColumnNameForProperty(item.Name)} LIKE '{propValue}%'");
						break;
					default:
						break;
				}
			}

			string birthDateRange = FormDateRangeStringCondition("birth_date", _employeeFilter.BeginBirthDateFilter, _employeeFilter.EndBirthDateFilter);

			if(!String.IsNullOrEmpty(birthDateRange))
				conditionList.Add(birthDateRange);

			return conditionList;
		}

		/// <summary>
		/// Сформировать строку условия по промежуткам дат.
		/// </summary>
		/// <param name="fieldName">Название колонки в таблице в БД.</param>
		/// <param name="beginRangeDate">Дата начала (если нет - DateTime.MinValue).</param>
		/// <param name="endRangeDate">Дата начала (если нет - DateTime.MinValue).</param>
		/// <param name="dateFormatString">Строка формата для даты, которая будет в выходной строке.</param>
		/// <returns>Строка условия.</returns>
		private string FormDateRangeStringCondition(string fieldName, DateTime beginRangeDate, DateTime endRangeDate, string dateFormatString = "yyyy.MM.dd")
		{
			if (beginRangeDate != DateTime.MinValue && endRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} between '{beginRangeDate.ToString(dateFormatString)}' and '{endRangeDate.ToString(dateFormatString)}'";
			}
			else if (beginRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} >= '{beginRangeDate.ToString(dateFormatString)}'";
			}
			else if (endRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} <= '{endRangeDate.ToString(dateFormatString)}'";
			}

			return null;
		}

		/// <summary>
		/// Получить имя из PascalCase в {name}_{name} формате.
		/// </summary>
		/// <param name="propertyName">Имя свойства класса, которое будет возвращено в {name}_{name} формате.</param>
		/// <returns>Имя в формате {name}_{name}.</returns>
		private string GetDataBaseColumnNameForProperty(string propertyName) 
		{
			propertyName = propertyName.Replace("Filter","");
			var sb = new StringBuilder();

			char previousChar = char.MinValue; // Unicode '\0'

			foreach (char c in propertyName)
			{
				if (char.IsUpper(c))
				{
					if (sb.Length != 0 && previousChar != ' ')
					{
						sb.Append('_');
					}
				}

				sb.Append(c);

				previousChar = c;
			}

			return sb.ToString().ToLower();
		}

		/// <summary>
		/// Сформировать строку пагинации.
		/// </summary>
		/// <returns>Возвращает строку пагинации, которая будет вставлена в конец всех условий.</returns>
		private string FormPaginationCondition() 
		{
			if (_employeeFilter.PaginationData.HasConditionToWorkWith)
			{
				return $"ORDER BY id OFFSET {_employeeFilter.PaginationData.PaginationIndex * _employeeFilter.PaginationData.PaginationCount} " +
					$"ROWS FETCH NEXT {_employeeFilter.PaginationData.PaginationCount} ROWS ONLY";
			}
			return "";
		}

		/// <summary>
		/// Сформировать строку условий Where полученных из FormConditionList()
		/// </summary>
		/// <returns>Общую строку условий для каждой из строк, полученный из FormConditionList().</returns>
		private string BuildWhereCondition()
		{
			List<string> conditionList = FormConditionList();

			if (conditionList.Count > 0)
			{
				return $"WHERE {String.Join(" and ", conditionList)}";
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Сформировать строку пагинации.
		/// </summary>
		/// <returns>Строка пагинации.</returns>
		private string BuildPaginationCondition()
		{
			return FormPaginationCondition();
		}

		/// <summary>
		/// Построить полную строку условия по указанному фильтру.
		/// </summary>
		/// <returns>Строка условия</returns>
		public string Build() 
		{
			return $" {BuildWhereCondition()} {BuildPaginationCondition()}";
		}
	}
}
