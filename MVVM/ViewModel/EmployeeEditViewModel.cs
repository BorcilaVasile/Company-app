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
        public string NavigationSource { get; set; }
        public EmployeeEditViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, Employee employee, EmployeeService employeeService)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            _employeeService = employeeService;
            NavigationSource = "Department";
            SaveCommand = new RelayCommand(o => Save(o));
            NavigateBackCommand = new RelayCommand(o => NavigateBack(o));
        }
        public EmployeeEditViewModel( MainViewModel mainViewModel, Employee employee, EmployeeService employeeService)
        {
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            _employeeService = employeeService;
            NavigationSource = "EmployeeList";
            SaveCommand = new RelayCommand(o => Save(o));
            NavigateBackCommand = new RelayCommand(o => NavigateBack(o));
        }

        public void Save(object parameter)
        {
            _employeeService.UpdateEmployee(CurrentEmployee);
        }
        public void NavigateBack(object parameter)
        {
            if (NavigationSource == "Department")
            {
                NavigateToDepartmentDetailsView(parameter);
            }
            else if (NavigationSource == "EmployeeList")
            {
                NavigateToEmployeesView();
            }
        }
        public void NavigateToDepartmentDetailsView(object parameter)
        {
            var departmentWithManager = parameter as DepartmentWithManager;
            if (departmentWithManager != null)
            {
                _mainViewModel.NavigateToDepartmentDetailsView(departmentWithManager);
            }
        }
        public void NavigateToEmployeesView()
        {
            _mainViewModel.NavigateToEmployeesView();
        }
    }
}
