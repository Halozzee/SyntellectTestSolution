using Domain.EmployeeObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmployeeApiBusinessLayer
{
	public class SqlWhereConditionStringBuilder
	{
		private readonly EmployeeFilter _employeeFilter;
		public SqlWhereConditionStringBuilder(EmployeeFilter employeeFilter) 
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

		private string FormDateRangeStringCondition(string fieldName, DateTime beginRangeDate, DateTime endRangeDate)
		{
			if (beginRangeDate != DateTime.MinValue && endRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} between '{beginRangeDate}' and '{endRangeDate}'";
			}
			else if (beginRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} >= '{beginRangeDate}'";
			}
			else if (endRangeDate != DateTime.MinValue)
			{
				return $"{fieldName} <= '{endRangeDate}'";
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

		public string Build()
		{
			List<string> conditionList = FormConditionList();
			return String.Join(" and ", conditionList);
		}
	}
}
