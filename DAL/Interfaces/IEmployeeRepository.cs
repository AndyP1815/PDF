using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
	public interface IEmployeeRepository
	{
		Employee GetEmployeeById(int id);
		List<Employee> GetEmployees();
		void AddEmployee(Employee employee);
		void RemoveEmployee (int Id);
		void EditEmployee (Employee employee);

	}
}
