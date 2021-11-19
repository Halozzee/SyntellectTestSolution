using System;
using EmployeeApiBusinessLayer;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Linq;
using Domain.DataProtection;
using EmployeeClientApp.RequestSending;
using Domain.DataProtection.Implementations;
using Domain.EmployeeObjects;
using Domain.EmployeeObjects;

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
			};

			HttpRequester httpRequester = new HttpRequester("http://localhost:17179/EmployeeApi", new SimpleCrypter());

            Console.Read();

            var t = MakeRandomEmployee();

            var response = httpRequester.SendInsertRequest(t);
			Console.WriteLine(response.ReadResponseMessageContent());
            t = response.ParseResponseMessageContentToObject<Employee>();

            t.LastName = "Zuckerberg";
            t.FirstName = "Mark";
            t.BirthDate = new DateTime(2900, 1, 1);

            response = httpRequester.SendUpdateRequest(t);
			Console.WriteLine(response.ReadResponseMessageContent());

            response = httpRequester.SendDeleteRequest(t);
            Console.WriteLine(response.ReadResponseMessageContent());
        }

        public static List<Employee> MakeRandomEmployees(int count) 
        {
            List<Employee> result = new List<Employee>();

			for (int i = 0; i < count; i++)
			{
                result.Add(MakeRandomEmployee());
            }

            return result;
        }

        public static Employee MakeRandomEmployee() 
        {
            Random random = new Random();
            return new Employee(GenerateName(random.Next(0, 10)), GenerateName(random.Next(0, 10)), GenerateName(random.Next(0, 10)), new DateTime(random.Next(1990, 2020), random.Next(1, 10), random.Next(1, 10)));
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
