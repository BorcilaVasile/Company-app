using System;
using System.Collections.ObjectModel;
using System.Windows;
using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;


namespace Administrare_firma.MVVM.ViewModel
{
    public class CreateEvaluationViewModel : ObservableObject
    {
        private int _employeeID;
        private int _insertedScor;
        private string _insertedFeedback;
        private string _insertedFutureObjectives;
        private DateTime _dateOfEvaluation;
        public DepartmentWithManager _currentDepartment { get; set; }
        public Employee _currentEmployee { get; set; }

        private readonly EmployeeService _employeeService;

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand NavigateBackCommand { get; set; }

        private object _currentView;
        private readonly MainViewModel _mainViewModel;

        public int InsertedScor
        {
            get => _insertedScor;
            set
            {
                    _insertedScor = value;
                    OnPropertyChanged(nameof(InsertedScor));
                    Console.WriteLine("Scor selected: " + InsertedScor);
            }
        }

        public string InsertedFeedback
        {
            get => _insertedFeedback;
            set
            {
                _insertedFeedback = value;
                OnPropertyChanged(nameof(InsertedFeedback));
            }
        }

        public string InsertedFutureObjectives
        {
            get => _insertedFutureObjectives;
            set
            {
                _insertedFutureObjectives = value;
                OnPropertyChanged(nameof(InsertedFutureObjectives));
            }
        }

        public DateTime DateOfEvaluation
        {
            get => _dateOfEvaluation;
            set
            {
                _dateOfEvaluation = value;
                OnPropertyChanged(nameof(DateOfEvaluation));
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

        public CreateEvaluationViewModel(MainViewModel mainViewModel,DepartmentWithManager departmentWithManager, Employee employee, EmployeeService employeeService, string navigationSource)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            _currentEmployee = employee;
            _employeeID = employee.ID;
            _employeeService = employeeService;
            NavigationSource = navigationSource;
            DateOfEvaluation = DateTime.Now;
            SaveCommand = new RelayCommand(o => SubmitEvaluation());
            CancelCommand=new RelayCommand(o => CancelEvaluation());
            NavigateBackCommand = new RelayCommand(o => NavigateBack());
        }

        private void SubmitEvaluation()
        {
            Console.WriteLine("Scor selected: " + InsertedScor);
            using (var context = new ApplicationDbContext())
            {
                var evaluation = new Evaluation
                {
                    ID_employee = _employeeID,
                    Date_of_evaluation = DateTime.Now,
                    Scor = _insertedScor,
                    Feedback = _insertedFeedback,
                    Future_objectives = _insertedFutureObjectives,
                    ID_manager = _mainViewModel._employeeID
                };

                context.Evaluations.Add(evaluation);
                context.SaveChanges();

                MessageBox.Show("Evaluation submitted succesfully");
            }
            NavigateBack();
        }

        private void CancelEvaluation()
        {
            _insertedFeedback = string.Empty;
            _insertedFutureObjectives = string.Empty;

            MessageBox.Show("Evaluation canceled succesfully");
            Console.WriteLine("Evaluation canceled.");
            NavigateBack();
        }

        public void NavigateBack()
        {
            _mainViewModel.NavigateToEmployeeDetailsView(CurrentDepartment, CurrentEmployee, NavigationSource);
        }
    }
}
