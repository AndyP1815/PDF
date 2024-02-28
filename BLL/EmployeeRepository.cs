using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DAL;
using DAL.Interfaces;

namespace BLL
{
	public class EmployeeRepository : IEmployeeRepository
	{
		const string FileName = "C:/Users/liu/Desktop/email/emails.csv";

		public void AddEmployee(Employee employee)
		{
			string newData = $"{employee.Id.ToString()},{employee.Email}";
			using (StreamWriter writer = File.AppendText(FileName))
			{
				writer.WriteLine(newData);
			}
		}

		public void EditEmployee(Employee employee)
		{
			int targetId = employee.Id; 

			
			string[] lines = File.ReadAllLines(FileName);

		
			int recordIndexToEdit = -1;
			for (int i = 1; i < lines.Length; i++) 
			{
				string[] recordData = lines[i].Split(',');
				int recordId = int.Parse(recordData[0]); 
				if (recordId == targetId)
				{
					recordIndexToEdit = i;
					break;
				}
			}

			if (recordIndexToEdit == -1)
			{
				Console.WriteLine("Record with the specified ID not found.");
				return;
			}

			
			string[] recordDataToEdit = lines[recordIndexToEdit].Split(',');
			recordDataToEdit[1] = employee.Email; // Change the age

			string modifiedRecord = string.Join(",", recordDataToEdit);

			lines[recordIndexToEdit] = modifiedRecord;

			File.WriteAllLines(FileName, lines);

			Console.WriteLine("Record edited in the CSV file.");

		}

		public List<Employee> GetEmployees()
		{
			List<Employee> employees = new List<Employee>();
			foreach (var line in File.ReadAllLines(FileName).Skip(1))
			{
				string[] data = line.Split(',');
				employees.Add(new Employee(Convert.ToInt32(data[0]), data[1]));
			}
			return employees;
		}

		public Employee GetEmployeeById(int id)
		{
			foreach (var line in File.ReadAllLines(FileName).Skip(1))
			{
				string[] data = line.Split(',');
				if (Convert.ToInt32(data[0]) == id)
				{
					return new Employee(id, data[1]);
				}
			}
			return null;
		}

		public void RemoveEmployee(int id)
		{
			int targetId = id;

			List<string> updatedLines = new List<string>();

			string[] lines = File.ReadAllLines(FileName);

			if (lines.Length > 0)
			{
				updatedLines.Add(lines[0]); 

				foreach (string line in lines.Skip(1))
				{
					string[] recordData = line.Split(',');
					int recordId = int.Parse(recordData[0]);

					if (recordId != targetId)
					{
						updatedLines.Add(line);
					}
				}

				File.WriteAllLines(FileName, updatedLines);
			}
		}

	}
}
