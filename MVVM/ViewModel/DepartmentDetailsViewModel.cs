using Administrare_firma.MVVM.Model;
using Administrare_firma.Core;
using System;
using System.Windows.Input;
using System.Windows;

namespace Administrare_firma.MVVM.ViewModel
{
    public class DepartmentDetailsViewModel : ObservableObject
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        public RelayCommand DepartmentsViewCommand { get; set; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
        private readonly MainViewModel _mainViewModel;

        public DepartmentWithManager CurrentDepartment
        {
            get => _currentDepartment;
            set
            {
                _currentDepartment = value;
                OnPropertyChanged(nameof(CurrentDepartment));
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }


        public DepartmentDetailsViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, bool isAdmin)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            IsAdmin = isAdmin;
            DepartmentsViewCommand = new RelayCommand(o => NavigateToDepartmentsView());
            AddEmployeeCommand = new RelayCommand(o => NavigateToAddEmployeeView(o));
            ViewDetailsCommand = new RelayCommand(o => ViewDetails(o));
            DeleteCommand = new RelayCommand(o => Delete(o));
        }

        public void NavigateToDepartmentsView()
        {
            _mainViewModel.NavigateToDepartmentsView();
        }

        private void ViewDetails(object parameter)
        {
            if (parameter is Employee selectedEmployee)
            {
                _mainViewModel.NavigateToEmployeeDetailsView(_currentDepartment, selectedEmployee,"Department");
            }
            else
            {
                Console.WriteLine("Invalid parameter for ViewDetailsCommand");
            }
        }
        private void Delete(object parameter)
        {
            if (parameter is Employee_informations _selectedEmployee)
            {
                if (_selectedEmployee == null)
                {
                    MessageBox.Show("Please select an employee to delete.");
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete {_selectedEmployee.Employee_user.First_name} {_selectedEmployee.Employee_user.Last_name}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Call the service to delete the employee
                        EmployeeService.DeleteEmployee(_selectedEmployee.Employee_user);

                        // Remove the employee from the current department's employee list
                        _currentDepartment.Departments_employees.Remove(_selectedEmployee);

                        MessageBox.Show("Employee deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the employee: {ex.Message}");
                    }
                }
            }
        }


        private void NavigateToAddEmployeeView(object parameter)
        {
            if (parameter is DepartmentWithManager departmentWithManager)
            {
                var currentDepartment = _currentDepartment;
                _mainViewModel.NavigateToAddEmployeeView(currentDepartment);
            }
            else
            {
                Console.WriteLine("Invalid parameter for AddEmployeeCommand");
            }
        }
    }
}
