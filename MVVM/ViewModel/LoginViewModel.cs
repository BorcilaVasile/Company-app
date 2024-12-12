using Administrare_firma.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand SignUpCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(o => ExecuteLogin());
            SignUpCommand = new RelayCommand(o => ExecuteSignUp());
        }

        private void ExecuteLogin()
        {

            using (var context = new CompanyDataContext())
            {
                var query = context.Accounts
                    .FirstOrDefault(account => account.Username == _username && account.Password == _password);

                if (query != null)
                {
                    MainPageViewModel.Instance.NavigateToMainView();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }
            }
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void ExecuteSignUp()
        {
            MainPageViewModel.Instance.NavigateToSignUpView();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
