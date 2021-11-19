using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
	public class Employee
	{
		public int Id { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string Patronymic { get; set; }
		public DateTime BirthDate { get; set; }

		public Employee()
		{

		}

		public Employee(string lastName, string firstName, string patronymic, DateTime birthDate)
		{
			Id = -1;
			LastName = lastName;
			FirstName = firstName;
			Patronymic = patronymic;
			BirthDate = birthDate;
		}

		public Employee(int id, string lastName, string firstName, string patronymic, DateTime birthDate)
		{
			Id = id;
			LastName = lastName;
			FirstName = firstName;
			Patronymic = patronymic;
			BirthDate = birthDate;
		}
	}
}
