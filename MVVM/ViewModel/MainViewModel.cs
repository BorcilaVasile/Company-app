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
		public RelayCommand HomeViewCommand { get; set; }
		public RelayCommand DepartmentsViewCommand { get; set; }
		public RelayCommand ManagersViewCommand { get; set; }
		public RelayCommand EmployeesViewCommand { get; set; }
		public RelayCommand DepartmentDetailsViewCommand { get; set; }
		public RelayCommand LogoutCommand { get; set; }
		public HomeViewModel HomeVM { get; set; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
		public ManagersViewModel ManagersVM { get; set; }
		public EmployeesViewModel EmployeesVM { get; set; }
		public DepartmentDetailsViewModel DepartmentDetailsVM { get; set; }
        private object _currentView;

		public object CurrentView
		{
			get => _currentView; 
			set { _currentView = value;
				OnPropertyChanged(nameof(CurrentView));
			}
		}

		public MainViewModel() {

            HomeVM = new HomeViewModel();
			DepartmentsVM = new DepartmentsViewModel(this);
			ManagersVM = new ManagersViewModel();
			EmployeesVM = new EmployeesViewModel();

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
			LogoutCommand = new RelayCommand(o => Logout());
        }

        public void NavigateToHomeView()
        {
            HomeVM = new HomeViewModel();
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
			EmployeesVM = new EmployeesViewModel();
			CurrentView = EmployeesVM;
		}
        public void NavigateToDepartmentDetailsView(DepartmentWithManager departmentWithManager)
        {
			DepartmentDetailsVM = new DepartmentDetailsViewModel(departmentWithManager,this);
            CurrentView = DepartmentDetailsVM;
        }

		public void Logout()
		{
           MainPageViewModel.Instance.NavigateToLoginView();
		}
    }
}
