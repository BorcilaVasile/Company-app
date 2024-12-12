using Administrare_firma;
using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using Administrare_firma.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class DepartmentsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private ObservableCollection<DepartmentWithManager> _departmentsWithManagers;
        public ObservableCollection<DepartmentWithManager> DepartmentsWithManagers
        {
            get => _departmentsWithManagers;
            set
            {
                _departmentsWithManagers = value;
                OnPropertyChanged(nameof(DepartmentsWithManagers));
            }
        }
        public ICommand DepartmentDetailsCommand { get; }

        private MainViewModel _mainViewModel;

        public DepartmentsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            DepartmentsWithManagers = new ObservableCollection<DepartmentWithManager>();
            LoadDepartmentsWithManagers();

            DepartmentDetailsCommand = new RelayCommand(o => ExecuteNavigateToDepartmentDetails(o));
        }

        private void LoadDepartmentsWithManagers()
        {
            using (var context = new CompanyDataContext())
            {
                var query = from department in context.Departments
                            join manager in context.Employees
                            on department.ID_department equals manager.ID_department
                            join post in context.Posts
                            on manager.ID_post equals post.ID_post
                            where post.Level_of_importance == 3
                            select new DepartmentWithManager
                            {
                                Department = department,
                                Manager = manager
                            };

                DepartmentsWithManagers.Clear();

                foreach (var item in query)
                {
                    DepartmentsWithManagers.Add(item);
                }
            }
        }

        private void ExecuteNavigateToDepartmentDetails(object parameter)
        {
            var departmentWithManager = parameter as DepartmentWithManager;

            if (departmentWithManager != null)
            {
                _mainViewModel.NavigateToDepartmentDetailsView(departmentWithManager);
            }
        }
    }
}
