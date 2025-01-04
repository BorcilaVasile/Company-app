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
    public class EmployeeDetailsViewModel : ObservableObject, INotifyPropertyChanged
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        public RelayCommand NavigateBackCommand { get; set; }
        public Employee _currentEmployee { get; set; }

        private object _currentView;
        private readonly MainViewModel _mainViewModel;
        public DepartmentDetailsViewModel DepartmentVM { get; set; }
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


        public EmployeeDetailsViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, Employee employee)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            NavigationSource = "Department";
            NavigateBackCommand = new RelayCommand(o => NavigateBack(o));
        }

        public EmployeeDetailsViewModel(Employee employee, MainViewModel mainViewModel)
        {
            _currentEmployee = employee;
            _mainViewModel = mainViewModel;
            NavigationSource = "EmployeeList";
            NavigateBackCommand = new RelayCommand(o => NavigateBack(o));
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
