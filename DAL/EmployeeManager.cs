using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Interfaces;

namespace BLL
{
	 public class EmployeeManager
	{
		private IEmployeeRepository employeeRepository;

		public EmployeeManager(IEmployeeRepository employeeRepository)
		{
			this.employeeRepository = employeeRepository;
		}

		public Employee GetEmployeeById(int id)
		{
			return employeeRepository.GetEmployeeById(id);
		}
		public List<Employee> GetEmployees()
		{
			return employeeRepository.GetEmployees();
		}
		public void AddEmployee(Employee employee)
		{
			employeeRepository.AddEmployee(employee);
		}
		public void RemoveEmployee(int Id)
		{
			employeeRepository.RemoveEmployee(Id);
		}
		public void EditEmployee(Employee employee)
		{
			employeeRepository.EditEmployee(employee);	
		}
	}
}
