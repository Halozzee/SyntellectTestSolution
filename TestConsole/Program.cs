using Domain;
using System;
using EmployeeApiBusinessLayer;
using System.Collections.Generic;

namespace TestConsole
{
	public class Program
	{
		static void Main(string[] args)
		{
			var employees = BusinessLogicManager.GetAllEmployees();

            EmployeeApiBusinessLayer.EmployeeFilter ef = new EmployeeApiBusinessLayer.EmployeeFilter()
            {
                LastNameFilter = "Z"
            };

            var matches = BusinessLogicManager.GetEmployeesByCondition(ef);
		}

        public static List<Employee> MakeRandomEmployees(int count) 
        {
            List<Employee> result = new List<Employee>();

            Random random = new Random();

			for (int i = 0; i < count; i++)
			{
                result.Add(new Employee(GenerateName(random.Next(0,10)), GenerateName(random.Next(0, 10)), GenerateName(random.Next(0, 10)), new DateTime(random.Next(1990, 2020), random.Next(0, 10), random.Next(0, 10))));
            }

            return result;
        }

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }
    }
}
