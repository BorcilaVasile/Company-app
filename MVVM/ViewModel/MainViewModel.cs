using Administrare_firma.MVVM.Model;
using Administrare_firma.Core;
using System;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.Linq;


namespace Administrare_firma.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly EmployeeService _employeeService;
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DepartmentsViewCommand { get; set; }
        public RelayCommand ProjectsViewCommand { get; set; }
        public RelayCommand EmployeesViewCommand { get; set; }
        public RelayCommand DepartmentDetailsViewCommand { get; set; }
        public RelayCommand EmployeeDetailsViewCommand { get; set; }
        public RelayCommand EmployeeEditViewCommand { get; set; }
        public RelayCommand AddEmployeeViewCommand { get; set; }
        public RelayCommand RequestDetailsViewCommand { get; set; }
        public RelayCommand AttendanceViewCommand { get; set; }
        public RelayCommand CreateRequestCommand { get; set; }
        public RelayCommand CurrentEmployeeEditViewCommand { get; set; }
        public RelayCommand CurrentEmployeeDetailsViewCommand { get; set; }
        public RelayCommand LogoutCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
        public EmployeesViewModel EmployeesVM { get; set; }
        public ProjectsViewModel ProjectsVM { get; set; }
        public DepartmentDetailsViewModel DepartmentDetailsVM { get; set; }
        public EmployeeDetailsViewModel EmployeeDetailsVM { get; set; }
        public EmployeeEditViewModel EmployeeEditVM { get; set; }
        public AddEmployeeViewModel AddEmployeeVM { get; set; }
        public RequestDetailsViewModel RequestDetailsVM { get; set; }
        public CreateRequestViewModel CreateRequestVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        private bool _isEmployee;
        public bool IsEmployee
        {
            get => _isEmployee;
            set
            {
                if (!_isEmployee != value)
                {
                    _isEmployee = value;
                    OnPropertyChanged(nameof(IsEmployee));
                }
            }
        }

        public string _typeOfUser;
        public int _employeeID;

        public bool IsAdmin { get; set; }

        public bool IsManager { get; set; }

        //le voi folosi pentru butoanele disponibile doar pentru manageri si doar pentru admin
        public bool adminLevel = false;
        public bool managerLevel = false;
        public MainViewModel(string TypeOfUser, int employeeID)
        {

            _typeOfUser = TypeOfUser;
            _employeeID = employeeID;
            if (_typeOfUser == "admin")
            {
                adminLevel = true;
                managerLevel = true;
                IsAdmin = true;
            }
            if (_typeOfUser == "manager")
            {
                managerLevel = true;
                IsManager = true;
            }

            _isEmployee = !IsAdmin && !IsManager;
            _employeeService = new EmployeeService();
            HomeVM = new HomeViewModel(this, employeeID, IsAdmin, IsManager);

            CurrentView = HomeVM;

            //Home view e acelasi pentru admin si manager, vor fi diferite doar statisticile si request-urile
            HomeViewCommand = new RelayCommand(o => NavigateToHomeView(_employeeID, IsAdmin, IsManager));
            DepartmentsViewCommand = new RelayCommand(o => NavigateToDepartmentsView());
            EmployeesViewCommand = new RelayCommand(o => NavigateToEmployeesView());
            ProjectsViewCommand = new RelayCommand(o => NavigateToProjectsView());
            DepartmentDetailsViewCommand = new RelayCommand(o =>
            {
                if (o is DepartmentWithManager departmentWithManager)
                {
                    NavigateToDepartmentDetailsView(departmentWithManager);
                }
            });
            EmployeeDetailsViewCommand = new RelayCommand(o =>
            {
                if (o is Tuple<DepartmentWithManager, Employee> parameters)
                {
                    var departmentWithManager = parameters.Item1;
                    var selectedEmployee = parameters.Item2;

                    NavigateToEmployeeDetailsView(departmentWithManager, selectedEmployee, "Home");
                }
            });
            CreateRequestCommand = new RelayCommand(o => NavigateToCreateRequestView(employeeID));
            CurrentEmployeeEditViewCommand = new RelayCommand(o => NavigateToCurrentEmployeeEditView());
            CurrentEmployeeDetailsViewCommand = new RelayCommand(o => NavigateToCurrentEmployeeDetailsView());
            LogoutCommand = new RelayCommand(o => Logout());
        }
        public void NavigateToHomeView(int employeeID, bool IsAdmin, bool IsManager)
        {
            HomeVM = new HomeViewModel(this, employeeID, IsAdmin, IsManager);
            CurrentView = HomeVM;
        }
        public void NavigateToDepartmentsView()
        {
            if (IsAdmin)
            {
                DepartmentsVM = new DepartmentsViewModel(this);
                CurrentView = DepartmentsVM;
            }
            else if (IsManager)
            {
                using (var context = new CompanyDataContext())
                {
                    var departmentWithManager = (from department in context.Departments
                                                 join manager in context.Employees
                                                 on department.ID_department equals manager.ID_department
                                                 where manager.ID == _employeeID
                                                 select new DepartmentWithManager
                                                 {
                                                     Department = department,
                                                     Manager = manager
                                                 }).FirstOrDefault();

                    if (departmentWithManager != null)
                    {
                        var employeesData = (from employee in context.Employees
                                             where employee.ID_department == departmentWithManager.Department.ID_department
                                             select new
                                             {
                                                 Employee = employee,
                                                 Post = context.Posts.FirstOrDefault(p => p.ID_post == employee.ID_post),
                                                 IsManager = context.Posts
                                                     .Where(p => p.ID_post == employee.ID_post)
                                                     .Select(p => (bool?)(p.Level_of_importance == 3))
                                                     .FirstOrDefault() ?? false
                                             }).ToList();

                        var employees = employeesData.Select(data => new Employee_informations
                        {
                            Employee_user = data.Employee,
                            Post = data.Post,
                            Department = departmentWithManager.Department,
                            IsManager = data.IsManager
                        }).ToList();

                        departmentWithManager.Departments_employees = new ObservableCollection<Employee_informations>(employees);
                        DepartmentDetailsVM = new DepartmentDetailsViewModel(departmentWithManager, this, IsAdmin);
                        CurrentView = DepartmentDetailsVM;
                    }
                    else
                    {
                        MessageBox.Show("Nu s-a găsit departamentul managerului curent.");
                    }
                }
            }
        }


        public void NavigateToEmployeesView()
        {
            EmployeesVM = new EmployeesViewModel(this);
            CurrentView = EmployeesVM;
        }
        public void NavigateToProjectsView()
        {
            ProjectsVM = new ProjectsViewModel();
            CurrentView = ProjectsVM;
        }
        public void NavigateToDepartmentDetailsView(DepartmentWithManager departmentWithManager)
        {
            DepartmentDetailsVM = new DepartmentDetailsViewModel(departmentWithManager, this, IsAdmin);
            CurrentView = DepartmentDetailsVM;
        }

        public void NavigateToEmployeeDetailsView(DepartmentWithManager departmentWithManager, Employee employee, string navigationSource)
        {
            EmployeeDetailsVM = new EmployeeDetailsViewModel(departmentWithManager, this, employee,navigationSource);
            CurrentView = EmployeeDetailsVM;
        }
        public void NavigateToEmployeeDetailsView(Employee employee, string navigationSource)
        {
            EmployeeDetailsVM = new EmployeeDetailsViewModel(employee, this, navigationSource);
            CurrentView = EmployeeDetailsVM;
        }

        public void NavigateToEmployeeEditView(DepartmentWithManager departmentWithManager, Employee employee, string navigationSource)
        {
            EmployeeEditVM = new EmployeeEditViewModel(departmentWithManager, this, employee, _employeeService, navigationSource);
            CurrentView = EmployeeEditVM;
        }
        public void NavigateToEmployeeEditView(Employee employee, string navigationSource)
        {
            EmployeeEditVM = new EmployeeEditViewModel(this, employee.ID, _employeeService, navigationSource);
            CurrentView = EmployeeEditVM;
        }

        public void NavigateToAddEmployeeView(DepartmentWithManager departmentWithManager)
        {
            AddEmployeeVM = new AddEmployeeViewModel(departmentWithManager, this, _employeeService);
            CurrentView = AddEmployeeVM;
        }

        public void NavigateToRequestDetailsView(Request_informations request_Informations)
        {
            RequestDetailsVM = new RequestDetailsViewModel(request_Informations, this, _employeeService, _employeeID, IsAdmin, IsManager);
            CurrentView = RequestDetailsVM;
        }

        public void NavigateToCreateRequestView(int employeeID)
        {
            CreateRequestVM = new CreateRequestViewModel(employeeID, this, IsManager);
            CurrentView = CreateRequestVM;
        }
        public void NavigateToCurrentEmployeeEditView()
        {
            EmployeeEditVM = new EmployeeEditViewModel(this, _employeeID, _employeeService, "Home");
            CurrentView = EmployeeEditVM;
        }

        public void NavigateToCurrentEmployeeDetailsView()
        {
            using (var context = new CompanyDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Employee>(e => e.Post);
                context.LoadOptions = loadOptions;

                var employee = context.Employees.SingleOrDefault(e => e.ID ==_employeeID);

                if (employee != null)
                {
                    EmployeeDetailsVM = new EmployeeDetailsViewModel(employee, this,"Home");
                    CurrentView = EmployeeDetailsVM;
                }
                else
                {
                    Console.WriteLine($"Angajatul cu ID-ul {_employeeID} nu a fost găsit.");
                }
            }
        }


        public void Logout()
        {
            MainPageViewModel.Instance.NavigateToLoginView();
        }
    }
}
