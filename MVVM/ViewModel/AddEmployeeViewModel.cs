using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administrare_firma.MVVM.ViewModel
{
    public class AddEmployeeViewModel : ObservableObject, INotifyPropertyChanged
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        public Employee _currentEmployee { get; set; }

        private readonly EmployeeService _employeeService;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand DepartmentDetailsViewCommand { get; set; }

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

        public AddEmployeeViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, EmployeeService employeeService)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = new Employee();
            _employeeService = employeeService;
            SaveCommand = new RelayCommand(o => Save(o));
            DepartmentDetailsViewCommand = new RelayCommand(o => NavigateToDepartmentDetailsView(o));
        }

        public void Save(object parameter)
        {
            if (_currentEmployee == null)
            {
                MessageBox.Show("Please fill in the employee details before saving.");
                return;
            }
            if (_currentDepartment != null && _currentDepartment.Department != null)
            {
                _currentEmployee.ID_department = _currentDepartment.Department.ID_department;
                _currentEmployee.ID_post = 9;
            }
            else
            {
                MessageBox.Show("No department selected. Please select a department.");
                return;
            }
            _employeeService.SaveNewEmployee(_currentEmployee);
        }
        public void NavigateToDepartmentDetailsView(object parameter)
        {
            var departmentWithManager = parameter as DepartmentWithManager;
            if (departmentWithManager != null)
            {
                _mainViewModel.NavigateToDepartmentDetailsView(departmentWithManager);
            }
        }
    }
}
