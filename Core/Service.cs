using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;  // Pentru Entity Framework 6
using Administrare_firma.MVVM.Model;

namespace Administrare_firma.Core
{
    public class EmployeeService
    {

        public EmployeeService()
        {
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var context = new ApplicationDbContext())
            {
                var existingEmployee = context.Employee
                    .FirstOrDefault(e => e.ID == employee.ID);

                if (existingEmployee != null)
                {
                    // Actualizăm datele angajatului existent
                    existingEmployee.First_name = employee.First_name;
                    existingEmployee.Last_name = employee.Last_name;
                    existingEmployee.CNP = employee.CNP;
                    existingEmployee.Date_of_birth = employee.Date_of_birth;
                    existingEmployee.Home_address = employee.Home_address;
                    existingEmployee.Phone_number = employee.Phone_number;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.ID_post = employee.ID_post;
                    existingEmployee.ID_department = employee.ID_department;
                    existingEmployee.Base_salary = employee.Base_salary;
                    existingEmployee.Status = employee.Status;
                    existingEmployee.Contract_period = employee.Contract_period;
                    existingEmployee.Contract_type = employee.Contract_type;
                    existingEmployee.ID_post = employee.ID_post;

                    context.SaveChanges();
                    MessageBox.Show("The information on this employee has been updated successfully.");
                }
                else
                {
                    MessageBox.Show("The employee doesn't exist in the database.");
                }
            }
        }

        public void SaveNewEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Employee object cannot be null.");
            }

            using (var context = new ApplicationDbContext())
            {
                var existingEmployee = context.Employee
                    .FirstOrDefault(e => e.CNP == employee.CNP);  // Verificăm unicitatea CNP-ului

                if (existingEmployee == null)
                {
                    // Adăugăm angajatul nou
                    context.Employee.Add(new Employee
                    {
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
                        Status = "Active",  // Se poate adăuga un status implicit
                    });

                    context.SaveChanges();
                    MessageBox.Show("The new employee has been successfully added.");
                }
                else
                {
                    MessageBox.Show("An employee with this CNP already exists.");
                }
            }
        }

        public void DeleteEmployee(Employee employee)
        {
            using (var context = new ApplicationDbContext())
            {
                var employeeToDelete = context.Employee
                    .SingleOrDefault(e => e.CNP == employee.CNP);

                if (employeeToDelete != null)
                {
                    context.Employee.Remove(employeeToDelete);
                    context.SaveChanges();
                    MessageBox.Show("The employee has been deleted.");
                }
                else
                {
                    MessageBox.Show("The employee doesn't exist.");
                }
            }
        }

        public void AcceptRequest(Requests request)
        {
            if (request != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var requestToAccept = context.Requests
                        .FirstOrDefault(r => r.RequestID == request.RequestID);

                    if (requestToAccept != null)
                    {
                        requestToAccept.Status = "Approved";
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("The request doesn't exist.");
                    }
                }
            }
        }

        public void RejectRequest(Requests request)
        {
            if (request != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var requestToReject = context.Requests
                        .FirstOrDefault(r => r.RequestID == request.RequestID);

                    if (requestToReject != null)
                    {
                        requestToReject.Status = "Rejected";
                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("The request doesn't exist.");
                    }
                }
            }
        }
    }
}
