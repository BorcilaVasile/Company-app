using System.Windows.Input;
using Administrare_firma.Core;
using System.ComponentModel;
using Administrare_firma.MVVM.Model;
using System;
using Administrare_firma.MVVM.ViewModel;


namespace Administrare_firma.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly EmployeeService _employeeService;
        public RelayCommand HomeViewCommand { get; set; }
		public RelayCommand DepartmentsViewCommand { get; set; }
		public RelayCommand ManagersViewCommand { get; set; }
		public RelayCommand EmployeesViewCommand { get; set; }
		public RelayCommand DepartmentDetailsViewCommand { get; set; }
		public RelayCommand EmployeeDetailsViewCommand { get; set; }
		public RelayCommand EmployeeEditViewCommand { get; set; }
        public RelayCommand AddEmployeeViewCommand { get; set; }
        public RelayCommand RequestDetailsViewCommand { get; set; }
		public RelayCommand LogoutCommand { get; set; }
		public HomeViewModel HomeVM { get; set; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
		public ManagersViewModel ManagersVM { get; set; }
		public EmployeesViewModel EmployeesVM { get; set; }
		public DepartmentDetailsViewModel DepartmentDetailsVM { get; set; }
		public EmployeeDetailsViewModel EmployeeDetailsVM { get; set; }
        public EmployeeEditViewModel EmployeeEditVM { get; set; }
        public AddEmployeeViewModel AddEmployeeVM { get; set; }
        public RequestDetailsViewModel RequestDetailsVM { get; set; }
        private object _currentView;

		public object CurrentView
		{
			get => _currentView; 
			set { _currentView = value;
				OnPropertyChanged(nameof(CurrentView));
			}
		}

		public MainViewModel() {
            _employeeService = new EmployeeService();
            HomeVM = new HomeViewModel(this);
			DepartmentsVM = new DepartmentsViewModel(this);
			ManagersVM = new ManagersViewModel();
			EmployeesVM = new EmployeesViewModel(this);

			CurrentView = HomeVM;

			HomeViewCommand = new RelayCommand(o => NavigateToHomeView());
			DepartmentsViewCommand = new RelayCommand(o => NavigateToDepartmentsView());
			ManagersViewCommand = new RelayCommand(o => NavigateToManagersView());
			EmployeesViewCommand = new RelayCommand(o => NavigateToEmployeesView());
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

                    NavigateToEmployeeDetailsView(departmentWithManager, selectedEmployee);
                }
            });
            EmployeeEditViewCommand = new RelayCommand(o =>
            {
                if (o is Tuple<DepartmentWithManager, Employee> parameters)
                {
                    var departmentWithManager = parameters.Item1;
                    var selectedEmployee = parameters.Item2;

                    NavigateToEmployeeEditView(departmentWithManager, selectedEmployee);
                }
            });
            LogoutCommand = new RelayCommand(o => Logout());
        }

        public void NavigateToHomeView()
        {
            HomeVM = new HomeViewModel(this);
            CurrentView = HomeVM;
        }
		public void NavigateToDepartmentsView()
		{
            DepartmentsVM = new DepartmentsViewModel(this);
            CurrentView = DepartmentsVM;
        }
		public void NavigateToManagersView()
		{
			ManagersVM= new ManagersViewModel();
			CurrentView = ManagersVM;
		}
		public void NavigateToEmployeesView()
		{
			EmployeesVM = new EmployeesViewModel(this);
			CurrentView = EmployeesVM;
		}
        public void NavigateToDepartmentDetailsView(DepartmentWithManager departmentWithManager)
        {
			DepartmentDetailsVM = new DepartmentDetailsViewModel(departmentWithManager,this);
            CurrentView = DepartmentDetailsVM;
        }

		public void NavigateToEmployeeDetailsView(DepartmentWithManager departmentWithManager, Employee employee)
		{
			EmployeeDetailsVM = new EmployeeDetailsViewModel(departmentWithManager, this, employee);
			CurrentView = EmployeeDetailsVM;
		}
        public void NavigateToEmployeeDetailsView( Employee employee)
        {
            EmployeeDetailsVM = new EmployeeDetailsViewModel(employee,this);
            CurrentView = EmployeeDetailsVM;
        }

        public void NavigateToEmployeeEditView(DepartmentWithManager departmentWithManager, Employee employee)
        {
            EmployeeEditVM = new EmployeeEditViewModel(departmentWithManager, this, employee,_employeeService);
            CurrentView = EmployeeEditVM;
        }
        public void NavigateToEmployeeEditView(Employee employee)
        {
            EmployeeEditVM = new EmployeeEditViewModel( this, employee, _employeeService);
            CurrentView = EmployeeEditVM;
        }

        public void NavigateToAddEmployeeView(DepartmentWithManager departmentWithManager)
        {
            AddEmployeeVM= new AddEmployeeViewModel(departmentWithManager,this,_employeeService);
            CurrentView = AddEmployeeVM;
        }

        public void NavigateToRequestDetailsView(Request_informations request_Informations)
        {
            RequestDetailsVM= new RequestDetailsViewModel(request_Informations,this);
            CurrentView = RequestDetailsVM;
        }
        public void Logout()
		{
           MainPageViewModel.Instance.NavigateToLoginView();
		}
    }
}
