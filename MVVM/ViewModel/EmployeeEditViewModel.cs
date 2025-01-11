using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class EmployeeEditViewModel : ObservableObject, INotifyPropertyChanged
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        public Employee _currentEmployee { get; set; }

        private readonly EmployeeService _employeeService;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NavigateBackCommand { get; set; }

        private object _currentView;
        private readonly MainViewModel _mainViewModel;
        public DepartmentDetailsViewModel DepartmentVM { get; set; }

        public List<string> ContractTypes { get; set; } = new List<string>
            {
                "full-time",
                "part-time",
                "remote"
            };
        private string _contractType;
        public string Contract_type
        {
            get => _contractType;
            set
            {
                _contractType = value;
                OnPropertyChanged(nameof(Contract_type));
            }
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public DepartmentWithManager CurrentDepartment
        {
            get => _currentDepartment;
            set
            {
                _currentDepartment = value;
                OnPropertyChanged(nameof(CurrentDepartment));
            }
        }

        public Employee CurrentEmployee
        {
            get => _currentEmployee;
            set
            {
                _currentEmployee = value;
                OnPropertyChanged(nameof(CurrentEmployee));
            }
        }

        private int _currentEmployeeId;
        public int CurrentEmployeeId
        {
            get => _currentEmployeeId;
            set
            {
                _currentEmployeeId = value;
                OnPropertyChanged(nameof(CurrentEmployeeId));
            }
        }
        public string NavigationSource { get; set; }
        public EmployeeEditViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, Employee employee, EmployeeService employeeService, string navigationSource)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            _employeeService = employeeService;
            NavigationSource = navigationSource;
            SaveCommand = new RelayCommand(o => Save(o));
            NavigateBackCommand = new RelayCommand(o => NavigateBack());
        }
        public EmployeeEditViewModel(MainViewModel mainViewModel, int employeeId, EmployeeService employeeService, string navigationSource)
        {
            _mainViewModel = mainViewModel;
            _employeeService = employeeService;
            _currentEmployeeId= employeeId;

            using (var context = new CompanyDataContext())
            {
                var employee = context.Employees.SingleOrDefault(e => e.ID == employeeId);
                if (employee != null)
                {
                    CurrentEmployee = employee;

                    var department = context.Departments.SingleOrDefault(d => d.ID_department == employee.ID_department);
                    if (department != null)
                    {
                        var manager = context.Employees.SingleOrDefault(m => m.ID_department == department.ID_department &&
                                                context.Posts.Any(p => p.ID_post == m.ID_post && p.Level_of_importance == 3));

                        CurrentDepartment = new DepartmentWithManager
                        {
                            Department = department,
                            Manager = manager
                        };
                    }
                }
            }

            NavigationSource = navigationSource;
            SaveCommand = new RelayCommand(o => Save(o));
            NavigateBackCommand = new RelayCommand(o => NavigateBack());
            LoadEmployeeWithDepartment();
        }


        public void Save(object parameter)
        {
            _employeeService.UpdateEmployee(CurrentEmployee);
        }
        public void NavigateBack()
        {
            _mainViewModel.NavigateToEmployeeDetailsView(CurrentDepartment,CurrentEmployee, NavigationSource);
        }
        private void LoadEmployeeWithDepartment()
        {
            using (var context = new CompanyDataContext())
            {
                // Obține angajatul specificat
                var employee = (from e in context.Employees
                                where e.ID == _currentEmployeeId
                                select new
                                {
                                    e.ID,
                                    e.ID_department,
                                    e.First_name,
                                    e.Last_name,
                                    e.CNP,
                                    e.Date_of_birth,
                                    e.Home_address,
                                    e.Email,
                                    e.Phone_number,
                                    e.ID_post,
                                    e.Base_salary,
                                    e.Status,
                                    e.Contract_period,
                                    e.Contract_type,
                                    Post = context.Posts.FirstOrDefault(p => p.ID_post == e.ID_post)
                                }).FirstOrDefault();

                if (employee == null)
                {
                    throw new Exception("Angajatul nu a fost găsit.");
                }

                // Obține departamentul și managerul asociat
                var departmentWithManager = (from department in context.Departments
                                             where department.ID_department == employee.ID_department
                                             join manager in context.Employees
                                             on department.ID_department equals manager.ID_department into managerGroup
                                             from manager in managerGroup
                                             .Where(m => (context.Posts
                                                    .Where(p => p.ID_post == m.ID_post)
                                                    .Select(p => (int?)p.Level_of_importance)
                                                    .FirstOrDefault() ?? -1) == 3)
                                             .DefaultIfEmpty()
                                             select new DepartmentWithManager
                                             {
                                                 Department = department,
                                                 Manager = manager // Poate fi null
                                             }).FirstOrDefault();

                if (departmentWithManager == null)
                {
                    throw new Exception("Departamentul angajatului nu a fost găsit.");
                }

                // Setează angajatul și departamentul curent
                CurrentEmployee = new Employee
                {
                    ID = employee.ID,
                    ID_department = employee.ID_department,
                    First_name = employee.First_name,
                    Last_name = employee.Last_name,
                    CNP = employee.CNP,
                    Date_of_birth = employee.Date_of_birth,
                    Home_address = employee.Home_address,
                    Email = employee.Email,
                    Phone_number = employee.Phone_number,
                    ID_post = employee.ID_post,
                    Base_salary = employee.Base_salary,
                    Status = employee.Status,
                    Contract_period = employee.Contract_period,
                    Contract_type = employee.Contract_type
                };

                CurrentDepartment = departmentWithManager;
            }
        }

    }
}
