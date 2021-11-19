using Domain;
using System;
using EmployeeApiBusinessLayer;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Linq;

namespace TestConsole
{
	public class Program
	{
		static void Main(string[] args)
		{
			//var employees = BusinessLogicManager.GetAllEmployees();

			EmployeeFilter ef = new EmployeeFilter()
			{
				LastNameFilter = "Z",
				BeginBirthDateFilter = new DateTime(2000, 1, 1),
				EndBirthDateFilter = new DateTime(2010, 1, 1)
			};

            var matches = BusinessLogicManager.GetEmployeesByCondition(ef);

            Console.Read();

			HttpRequester httpRequester = new HttpRequester("http://localhost:17179/EmployeeApi");
            //var response = httpRequester.SendQueryStringRequestWithMethod(System.Net.Http.HttpMethod.Get, $"employeeFilterJson={JsonConvert.SerializeObject(ef)}");
            //var message = response.ReadResponseMessageContent();

            var t = matches.ElementAt(0);

            t.FirstName = "Margarita";
            t.LastName = "Tetcher";
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
