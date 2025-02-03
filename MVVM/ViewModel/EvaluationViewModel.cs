using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace Administrare_firma.MVVM.ViewModel
{
    public class EvaluationViewModel : ObservableObject
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        private readonly EmployeeService _employeeService;
        public Employee _currentEmployee { get; set; }
        public RelayCommand NavigateBackCommand { get; set; }

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
        private int _employeeID;

        private ObservableCollection<Evaluation> _evaluations;
        public ObservableCollection<Evaluation> Evaluations
        {
            get => _evaluations;
            set
            {
                _evaluations = value;
                OnPropertyChanged(nameof(Evaluations));
            }
        }

        public EvaluationViewModel(MainViewModel mainViewModel, DepartmentWithManager departmentWithManager, Employee employee, EmployeeService employeeService, string navigationSource)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            _employeeID = employee.ID;
            _employeeService = employeeService;
            NavigationSource = navigationSource;
            NavigateBackCommand = new RelayCommand(o => NavigateBack());

            LoadEvaluations();
        }

        public void NavigateBack()
        {
            _mainViewModel.NavigateToEmployeeDetailsView(CurrentDepartment, CurrentEmployee, NavigationSource);
        }

        public void LoadEvaluations()
        {
            using (var context = new ApplicationDbContext())
            {
                var evaluations = context.Evaluations
                    .Where(e => e.ID_employee == _employeeID)  
                    .OrderByDescending(e => e.Date_of_evaluation) 
                    .ToList();

                Evaluations = new ObservableCollection<Evaluation>(evaluations);
            }
        }
    }
}
