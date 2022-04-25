using Library_App.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Models.Order
{
    public class OrderCreateBindingModel
    {
        public string SignatureId { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public virtual List<EmployeePairVM> Employees { get; set; }=new List<EmployeePairVM>();
        public string IsTaken { get; set; }
        public int Count { get; set; }

        //public string BookTitle { get; set; }
    }
}
