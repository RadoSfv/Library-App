using Library_App.Abstractions;
using Library_App.Data;
using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateEmployee(string firstName, string lastName, string phone, string jobTitle, string address, string userId)
        {
            if (_context.Employees.Any(p => p.UserId == userId))
            {
                throw new InvalidOperationException("Employee already exist.");
            }
            Employee employeeForDb = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                JobTitle = jobTitle,
                Address = address,
            UserId = userId
            };
            _context.Employees.Add(employeeForDb);

            return _context.SaveChanges() != 0;
        }
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = _context.Employees.Find(employeeId);

            return employee;
        }

        public Employee GetEmployeeByUserId(string userId)
        {
            var employee = _context.Employees.FirstOrDefault(x => x.UserId == userId);

            return employee;
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> employees = _context.Employees.ToList();

            return employees;
        }

        public int GetEmployeesCount()
        {
            return _context.Employees.Count();
        }

        public string GetFullName(int employeeId)
        {
            Employee employee = _context.Employees.Find(employeeId);
            string fullName = employee.FirstName + " " + employee.LastName;
            return fullName;
        }

        public bool RemoveById(int employeeId)
        {
            var item = _context.Employees
                 .FirstOrDefault(c => c.Id == employeeId);
            if (item != null)
            {
                _context.Employees
                    .Remove(item);
                return _context.SaveChanges() != 0;

            }
            else
            {
                //I'm deleting non-existent ID
                return false;
            }
        }
        public bool UpdateEmployee(int employeeId, string firstName, string lastName, string phone, string address,string jobTitle)
        {
            var employee = GetEmployeeById(employeeId);

            if (employee == default(Employee))
            {
                return false;
            }
            employee.FirstName = firstName;
            employee.LastName = lastName;
            employee.Phone = phone;
            employee.JobTitle = jobTitle;
            employee.Address = address;

            _context.Update(employee);
            return _context.SaveChanges() != 0;
        }

        //private readonly ApplicationDbContext _context;

        //public EmployeeService(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public bool CreateEmployee(string firstName, string lastName, string phone, string jobTitle,string address, string userId)
        //{
        //    if (_context.Employees.Any(p => p.UserId == userId))
        //    {
        //        throw new InvalidOperationException("Employee already exist.");
        //    }
        //    Employee employeeForDb = new Employee()
        //    {
        //        FirstName = firstName,
        //        LastName = lastName,
        //        Phone = phone,
        //        JobTitle = jobTitle,
        //        Address= address,
        //        UserId = userId
        //    };

        //    _context.Employees.Add(employeeForDb);

        //    return _context.SaveChanges() != 0;
        //}

        //public Employee GetEmployeeById(string employeeId)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Employee> GetEmployees()
        //{
        //    var employees = _context.Employees.ToList();
        //    return employees;
        //}

        //public string GetFullName(string employeeId)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool RemoveById(string employeeId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

