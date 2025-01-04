using System;
using System.Linq;
using System.Data.Linq;
using System.Windows;

namespace Administrare_firma.Core
{
    public class EmployeeService
    {
        public void UpdateEmployee(Employee employee)
        {
            using (var context = new CompanyDataContext())
            {
                var existingEmployee = context.Employees
                    .FirstOrDefault(e => e.ID == employee.ID);

                if (existingEmployee != null)
                {
                    // Actualizăm datele angajatului existent
                    existingEmployee.ID = employee.ID;
                    existingEmployee.First_name = employee.First_name;
                    existingEmployee.Last_name = employee.Last_name;
                    existingEmployee.CNP = employee.CNP;
                    existingEmployee.Date_of_birth = employee.Date_of_birth;
                    existingEmployee.Home_address = employee.Home_address;
                    existingEmployee.Phone_number = employee.Phone_number;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.ID_post=employee.ID_post;
                    existingEmployee.ID_department= employee.ID_department;
                    existingEmployee.Base_salary = employee.Base_salary;
                    existingEmployee.Status = employee.Status;
                    existingEmployee.Contract_period = employee.Contract_period;
                    existingEmployee.Contract_type = employee.Contract_type;
                    existingEmployee.Post = employee.Post;
                }
                else
                {
                    MessageBox.Show("The employee doesn't exist in database");
                    //context.Employees.InsertOnSubmit(employee);
                }

                context.SubmitChanges();
                MessageBox.Show("The information on this employee have been updated succesfully");
            }
        }
        public void SaveNewEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee object cannot be null.");
            }
            using (var context = new CompanyDataContext())
            {
                var existingEmployee = context.Employees
                    .FirstOrDefault(e => e.CNP == employee.CNP); // Verificăm unicitatea CNP-ului

                if (existingEmployee == null)
                {
                    // Adăugăm angajatul nou
                    int newId = context.Employees.Max(e => e.ID) + 1;
                    context.Employees.InsertOnSubmit(new Employee
                    {
                        ID= newId,
                        First_name = employee.First_name,
                        Last_name = employee.Last_name,
                        Email = employee.Email,
                        Phone_number = employee.Phone_number,
                        CNP = employee.CNP,
                        Date_of_birth = employee.Date_of_birth,
                        Home_address = employee.Home_address,
                        Base_salary = employee.Base_salary,
                        Contract_type = employee.Contract_type,
                        ID_post = employee.ID_post,
                        ID_department = employee.ID_department,
                        Status = "Active", // Se poate adăuga un status implicit
                    });

                    context.SubmitChanges();
                    MessageBox.Show("The new employee has been successfully added.");
                }
                else
                {
                    MessageBox.Show("An employee with this CNP already exists.");
                }
            }
        }
        public static void DeleteEmployee(Employee employee)
        {
            using (var db = new CompanyDataContext())
            {
                var employeeToDelete = db.Employees.SingleOrDefault(e => e.CNP == employee.CNP);
                if (employeeToDelete != null)
                {
                    db.Employees.DeleteOnSubmit(employeeToDelete);
                    db.SubmitChanges();
                }
            }
        }

    }
}
