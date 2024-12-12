using System;

namespace Administrare_firma.MVVM.Model
{
    public class Departments
    {
        public int ID_department { get; set; }
        public string Nume { get; set; }
        public string Description { get; set; }
    }
    public enum Status { active, inactive }
    public enum ContractType { full_time, part_time, remote}

    public class Employees
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CNP { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Home_address { get; set; }
        public string Phone_number { get; set; }
        public string Email { get; set; }
        public int ID_post { get; set; }
        public int ID_department { get; set; }
        public int Base_salary { get; set; }
        public Status Status { get; set; }
        public int Contract_period { get; set; }
        public ContractType Contract_type { get; set; }

    }

    public class Post
    {
        public int ID_post { get; set; }
        public string Nume { get; set; }
        public string Descriere { get; set; }
        public int Level_of_importance { get; set; }
    }

    public class Accounts
    {
        public int AccountID { get; set; }
        public int EmployeeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DepartmentWithManager
    {
        public Department Department { get; set; }
        public Employee Manager { get; set; }
        public string ManagerFullName => $"Manager {Manager.First_name} {Manager.Last_name}";
    }

}
