using System;
using Administrare_firma.Core;


namespace Administrare_firma.MWM.ViewModel
{
    class MainViewModel : ObservableObject
    {
		public RelayCommand HomeViewCommand { get; set; }
		public RelayCommand DepartmentsViewCommand { get; set; }
		public RelayCommand ManagersViewCommand { get; set; }
		public RelayCommand EmployeesViewCommand { get; set; }
		public HomeViewModel HomeVM { get; set; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
		public ManagersViewModel ManagersVM { get; set; }
		public EmployeesViewModel EmployeesVM { get; set; }
        private object _currentView;

		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value;
				OnPropertyChanged();
			}
		}

		public MainViewModel() {
			HomeVM = new HomeViewModel();
			DepartmentsVM = new DepartmentsViewModel();
			ManagersVM = new ManagersViewModel();
			EmployeesVM = new EmployeesViewModel();
			CurrentView = HomeVM;
			HomeViewCommand = new RelayCommand(o =>
			{
				CurrentView = HomeVM;
			});
			DepartmentsViewCommand = new RelayCommand(o => {
				CurrentView = DepartmentsVM;
			});
			ManagersViewCommand = new RelayCommand(o =>
			{
				CurrentView = ManagersVM;
			});
			EmployeesViewCommand = new RelayCommand(o =>
			{
				CurrentView = EmployeesVM;
			});
		}
    }
}
