using Administrare_firma.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administrare_firma.Core;
using System.Windows.Input;
using System.Security.Permissions;

namespace Administrare_firma.MVVM.ViewModel
{
    public class DepartmentDetailsViewModel : ObservableObject
    {
        public DepartmentWithManager _currentDepartment { get; set; }
        public RelayCommand DepartmentsViewCommand { get; set; }
        public DepartmentsViewModel DepartmentsVM { get; set; }
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

        public DepartmentDetailsViewModel(DepartmentWithManager departmentWithManager,MainViewModel mainViewModel)
        {
            _currentDepartment = departmentWithManager;
            _mainViewModel = mainViewModel;
            DepartmentsViewCommand = new RelayCommand(o => NavigateToDepartmentsView());
        }

        public void NavigateToDepartmentsView()
        {
            _mainViewModel.NavigateToDepartmentsView();
        }
    }
}
