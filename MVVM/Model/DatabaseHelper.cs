using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace Administrare_firma.MVVM.Model
{
    [NotMapped]
    public class Employee_informations
    {
        public Employee Employee_user { get; set; }
        public Department Department { get; set; }
        public Post Post { get; set; }
        public bool? IsManager { get; set; }
    }

    [NotMapped]
    public class DepartmentWithManager
    {
        public Department Department { get; set; }
        public Employee Manager { get; set; }
        public string ManagerFullName => Manager != null
     ? $"Team leader\n{Manager.First_name} {Manager.Last_name}"
     : "No manager assigned";

        public ObservableCollection<Employee_informations> Departments_employees { get; set; }

    }

    [NotMapped]
    public class Request_informations
    {
        public Requests Request { get; set; }
        public Employee Requester { get; set; }
        public bool Important { get; set; }
    }

}
