using System.ComponentModel;
using System.Windows.Input;
using Administrare_firma.Core;

namespace Administrare_firma.MVVM.ViewModel
{
    public class MainPageViewModel : ObservableObject
    {
        public static MainPageViewModel _instance;
        private static readonly object _lock = new object();

        public ICommand LoginViewCommand { get; private set; }
        public ICommand MainViewCommand { get; private set; }
        public ICommand SignUpViewCommand { get; private set; }


        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Private constructor to enforce singleton pattern
        public MainPageViewModel()
        {
            if (_instance == null) 
                _instance = this;
            CurrentView = new LoginViewModel();

            LoginViewCommand = new RelayCommand(o => NavigateToLoginView());
            MainViewCommand = new RelayCommand(o => NavigateToMainView());
            SignUpViewCommand = new RelayCommand(o => NavigateToSignUpView());
        }

        // Public static method to get the singleton instance
        public static MainPageViewModel Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MainPageViewModel();
                    }
                }
                return _instance;
            }
        }

        public void NavigateToMainView()
        {
            CurrentView = new MainViewModel();
        }

        public void NavigateToLoginView()
        {
            CurrentView = new LoginViewModel();
        }

        public void NavigateToSignUpView()
        {
            CurrentView = new SignUpViewModel();
        }
    }
}
