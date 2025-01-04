using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Administrare_firma.MVVM.Model
{
    public class Employee_informations
    {
        public Employee Employee_user { get; set; }
        public Department Department { get; set; }
        public Post Post { get; set; }
        public bool? IsManager { get; set; }
    }

    public class Accounts
    {
        public int AccountID { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string Email { get; set;}
        public string Password { get; set; }
    }

    public class DepartmentWithManager
    {
        public Department Department { get; set; }
        public Employee Manager { get; set; }
        public string ManagerFullName => Manager != null
     ? $"Team leader\n{Manager.First_name} {Manager.Last_name}"
     : "No manager assigned";

        public ObservableCollection<Employee_informations> Departments_employees { get; set; }

    }

    public class Request_informations
    {
        public Request Request { get; set; }
        public Employee Requester { get; set; }
        public bool Important { get; set; }
    }

}
