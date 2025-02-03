using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
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
        public RelayCommand EditCommand { get; set; }
        public RelayCommand CreateEvaluationCommand {  get; set; }
        public RelayCommand EvaluationCommand { get; set; }
        public Employee _currentEmployee { get; set; }
        private ObservableCollection<Clocking> _attendance;
        public ObservableCollection<Clocking> Attendance
        {
            get => _attendance;
            set
            {
                _attendance = value;
                OnPropertyChanged(nameof(Attendance));
            }
        }

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

        public bool IsManager { get; set; }

        private Post _employeePost;
        public Post EmployeePost
        {
            get => _employeePost;
            set
            {
                _employeePost = value;
                OnPropertyChanged(nameof(EmployeePost));
            }
        }

        public EmployeeDetailsViewModel(DepartmentWithManager departmentWithManager, MainViewModel mainViewModel, Employee employee, string navigationSource, bool isManager)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            NavigationSource = navigationSource;
            NavigateBackCommand = new RelayCommand(o => NavigateBack());
            EditCommand = new RelayCommand(o => NavigateToEditView());
            CreateEvaluationCommand = new RelayCommand(o => NavigateToCreateEvaluationView());
            EvaluationCommand = new RelayCommand(o => NavigateToEvaluationView());
            LoadEmployeePost();
            LoadAttendance();
            IsManager = isManager;
        }

        public EmployeeDetailsViewModel(Employee employee, MainViewModel mainViewModel, string navigationSource, bool isManager)
        {
            _currentEmployee = employee;
            _mainViewModel = mainViewModel;
            NavigationSource = navigationSource;
            NavigateBackCommand = new RelayCommand(o => NavigateBack());
            EditCommand = new RelayCommand(o => NavigateToEditView());
            CreateEvaluationCommand = new RelayCommand(o => NavigateToCreateEvaluationView());
            EvaluationCommand = new RelayCommand(o => NavigateToEvaluationView());
            LoadEmployeePost();
            LoadAttendance();
        }
        public void LoadEmployeePost()
        {
            using (var context = new ApplicationDbContext())
            {
                var employeePost = context.Posts.FirstOrDefault(p => p.ID_post == CurrentEmployee.ID_post);
                EmployeePost = employeePost; // Setează postul
            }
        }

        public void NavigateBack()
        {
            if (NavigationSource == "Department")
            {
                _mainViewModel.NavigateToDepartmentDetailsView(CurrentDepartment);
            }
            else if (NavigationSource == "EmployeeList")
            {
                _mainViewModel.NavigateToEmployeesView();
            }
            else if (NavigationSource == "Home")
                _mainViewModel.NavigateToHomeView(CurrentEmployee.ID, _mainViewModel.IsAdmin, _mainViewModel.IsManager);
        }
        
        public void NavigateToEditView()
        {
            _mainViewModel.NavigateToEmployeeEditView(CurrentDepartment, _currentEmployee, NavigationSource);
        }

        public void NavigateToCreateEvaluationView()
        {
            _mainViewModel.NavigateToCreateEvaluationView(CurrentDepartment, CurrentEmployee, NavigationSource);
        }
        public void NavigateToEvaluationView()
        {
            _mainViewModel.NavigateToEvaluationView(CurrentDepartment, CurrentEmployee, NavigationSource);
        }
        public void LoadAttendance()
        {
            using (var context = new ApplicationDbContext())
            {
                var attendanceRecords = from clocking in context.Clockings
                                        where clocking.ID_employee == CurrentEmployee.ID
                                        orderby clocking.Date_of_clocking descending
                                        select clocking;

                Attendance = new ObservableCollection<Clocking>(attendanceRecords);
            }
        }

    }
}
