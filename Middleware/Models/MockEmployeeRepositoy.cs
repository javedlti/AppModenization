using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Models
{
    public class MockEmployeeRepositoy : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepositoy()
        {
            _employeeList = new List<Employee>()
            {
                new Employee{Id=1,Name="John",Email="john@gmail.com",Department=Dept.Payroll},
                new Employee{Id=2,Name="Aman",Email="Ama@gmail.com",Department=Dept.HR}
            };
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Id = _employeeList.Max(x => x.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee DeleteEmployee(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if(employee !=null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }


        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(x => x.Id == Id);
        }

        public Employee UpdateEmployee(Employee employeechanges)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == employeechanges.Id);
            if(employee!=null)
            {
                employee.Name = employeechanges.Name;
                employee.Email = employeechanges.Email;
                employee.Department = employeechanges.Department;
            }
            return employee;
        }
    }
}
