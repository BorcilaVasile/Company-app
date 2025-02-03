using Administrare_firma;
using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;
using Administrare_firma.MVVM.ViewModel;
using System;
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
            this._mainViewModel = mainViewModel;

            DepartmentsWithManagers = new ObservableCollection<DepartmentWithManager>();
            LoadDepartmentsWithManagers();

            DepartmentDetailsCommand = new RelayCommand(o => ExecuteNavigateToDepartmentDetails(o));
        }

        private void LoadDepartmentsWithManagers()
        {
            using (var context = new ApplicationDbContext())
            {
                var query = from department in context.Departments
                            join manager in context.Employee
                            on department.ID_department equals manager.ID_department into managerGroup
                            from manager in managerGroup
                            .Where(m => (context.Posts
                                   .Where(p => p.ID_post == m.ID_post)
                                   .Select(p => (int?)p.Level_of_importance)
                                   .FirstOrDefault() ?? -1) == 3)
                                .DefaultIfEmpty() 
                            select new DepartmentWithManager
                            {
                                Department = department,
                                Manager = manager // Poate fi null
                            };

                DepartmentsWithManagers.Clear();

                foreach (var item in query)
                {
                    var employees = context.Employee
                        .Where(e => e.ID_department == item.Department.ID_department)
                        .Select(e => new
                        {
                            e.ID,
                            e.ID_department,
                            e.First_name,
                            e.Last_name,
                            e.CNP,
                            e.Date_of_birth,
                            e.Home_address,
                            e.Email,
                            e.Phone_number,
                            e.ID_post,
                            e.Base_salary,
                            e.Status,
                            e.Contract_period,
                            e.Contract_type,
                            Post = context.Posts
                                .Where(p => p.ID_post == e.ID_post)
                                .FirstOrDefault(),
                            IsManager = context.Posts
                                .Where(p => p.ID_post == e.ID_post)
                                .Select(p => (bool?)(p.Level_of_importance == 3))
                                .FirstOrDefault()
                        })
                        .ToList();

                    item.Departments_employees = new ObservableCollection<Employee_informations>(
                        employees.Select(e => new Employee_informations
                        {
                            Employee_user = new Employee
                            {
                                ID = e.ID,
                                ID_department = e.ID_department,
                                First_name = e.First_name,
                                Last_name = e.Last_name,
                                CNP = e.CNP,
                                Date_of_birth = e.Date_of_birth,
                                Home_address = e.Home_address,
                                Email = e.Email,
                                Phone_number = e.Phone_number,
                                ID_post = e.ID_post,
                                Base_salary = e.Base_salary,
                                Status = e.Status,
                                Contract_period = e.Contract_period,
                                Contract_type = e.Contract_type,
                            },
                            Post = e.Post,
                            Department = item.Department,
                            IsManager = e.IsManager
                        })
                    );
                DepartmentsWithManagers.Add(item );
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
