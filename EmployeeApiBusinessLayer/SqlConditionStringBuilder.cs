using Domain.EmployeeObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmployeeApiBusinessLayer
{
	public class SqlConditionStringBuilder
	{
		private readonly EmployeeFilter _employeeFilter;
		public SqlConditionStringBuilder(EmployeeFilter employeeFilter) 
		{
			_employeeFilter = employeeFilter;
		}

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

		private string FormPaginationCondition() 
		{
			if (_employeeFilter.PaginationData.HasConditionToWorkWith)
			{
				return $"ORDER BY id OFFSET {_employeeFilter.PaginationData.PaginationIndex * _employeeFilter.PaginationData.PaginationCount} " +
					$"ROWS FETCH NEXT {_employeeFilter.PaginationData.PaginationCount} ROWS ONLY";
			}
			return "";
		}

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

		private string BuildPaginationCondition()
		{
			return FormPaginationCondition();
		}

		public string Build() 
		{
			return $" {BuildWhereCondition()} {BuildPaginationCondition()}";
		}
	}
}
