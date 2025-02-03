using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class EmployeesViewModel : ObservableObject, INotifyPropertyChanged
    {
        public ICommand ViewDetailsCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private object _currentView;
        private readonly MainViewModel _mainViewModel;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private ObservableCollection<Employee_informations> _employees_info;
        public ObservableCollection<Employee_informations> Employees_info
        {
            get => _employees_info;
            set
            {
                _employees_info = value;
                OnPropertyChanged(nameof(Employees_info));
            }
        }
        private ObservableCollection<Employee_informations> _allEmployeesInfo;
        public ObservableCollection<Employee_informations> AllEmployeesInfo
        {
            get => _allEmployeesInfo;
            set
            {
                _allEmployeesInfo = value;
                OnPropertyChanged(nameof(AllEmployeesInfo));
            }
        }
      
        public EmployeesViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Employees_info = new ObservableCollection<Employee_informations>();
            AllEmployeesInfo = new ObservableCollection<Employee_informations>();
            ViewDetailsCommand = new RelayCommand(o => ViewDetails(o));
            EditCommand = new RelayCommand(o => Edit(o));
            DeleteCommand = new RelayCommand(o => Delete(o));
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = from employee in context.Employee
                            join department in context.Departments
                            on employee.ID_department equals department.ID_department
                            join post in context.Posts
                            on employee.ID_post equals post.ID_post
                            select new Employee_informations
                            {
                                Employee_user = employee,
                                Department = department,
                                Post = post, 
                                IsManager= context.Posts
                                    .Where(p => p.ID_post == employee.ID_post)
                                    .Select(p => (bool?)(p.Level_of_importance == 3))
                                    .FirstOrDefault()
                            };
                var orderedQuery = query.OrderBy(emp => emp.Employee_user.Last_name).ToList();
                AllEmployeesInfo.Clear();

                foreach (var emp in orderedQuery)
                {
                    AllEmployeesInfo.Add(emp);
                }

                Employees_info.Clear();
                foreach (var emp in AllEmployeesInfo)
                {
                    Employees_info.Add(emp);
                }

            }
        }
        public void FilterEmployees(string filterOption)
        {
            if (string.IsNullOrEmpty(filterOption) || filterOption == "All Employees")
            {
                Employees_info.Clear();
                foreach (var emp in AllEmployeesInfo)
                {
                    Employees_info.Add(emp);
                }
            }
            else if (filterOption == "Managers")
            {
                var managers = AllEmployeesInfo.Where(emp => emp.IsManager == true).ToList();
                Employees_info.Clear();
                foreach (var emp in managers)
                {
                    Employees_info.Add(emp);
                }
            }
            else if (filterOption == "Non-Managers")
            {
                var nonManagers = AllEmployeesInfo.Where(emp => emp.IsManager == false).ToList();
                Employees_info.Clear();
                foreach (var emp in nonManagers)
                {
                    Employees_info.Add(emp);
                }
            }
        }
        private void ViewDetails(object parameter)
        {
            if (parameter is Employee selectedEmployee)
            {
                _mainViewModel.NavigateToEmployeeDetailsView(selectedEmployee, "EmployeeList");
            }
            else
            {
                Console.WriteLine("Invalid parameter for ViewDetailsCommand");
            }
        }
        private void Edit(object parameter)
        {
            if (parameter is Employee selectedEmployee)
            {
                _mainViewModel.NavigateToEmployeeEditView(selectedEmployee, "EmployeeList");
            }
            else
            {
                Console.WriteLine("Invalid parameter for ViewDetailsCommand");
            }
        }
        private void Delete(object parameter)
        {
            if (parameter is Employee _selectedEmployee)
            {
                if (_selectedEmployee == null)
                {
                    MessageBox.Show("Please select an employee to delete.");
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete {_selectedEmployee.First_name} {_selectedEmployee.Last_name}?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _mainViewModel._employeeService.DeleteEmployee(_selectedEmployee);
                        MessageBox.Show("Employee deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while deleting the employee: {ex.Message}");
                    }
                }
            }
        }
    }

}
