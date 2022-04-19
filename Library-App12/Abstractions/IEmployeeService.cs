using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface IEmployeeService
    {
        //List<Employee> GetEmployees();
        //Employee GetEmployeeById(string employeeId);
        //public bool RemoveById(string employeeId);
        //string GetFullName(string employeeId);

        //bool CreateEmployee(string firstName, string lastName, string phone, string jobTitle, string address,string userId);
        

             List<Employee> GetEmployees();

        Employee GetEmployeeById(int employeeId);
        Employee GetEmployeeByUserId(string userId);

        public bool RemoveById(int employeeId);

        string GetFullName(int employeeId);

        bool CreateEmployee(string firstName, string lastName, string phone, string jobTitle, string address, string userId);

        public bool UpdateEmployee(int employeeId, string firstName, string lastName, string phone, string address, string jobTitle);

    }
}
