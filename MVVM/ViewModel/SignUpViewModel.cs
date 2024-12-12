using Administrare_firma.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _email;
        private string _password;


        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(IsFirstNameEmpty));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(IsLastNameEmpty));
            }
        }
         public string Username
        {
            get => _username;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsFirstNameEmpty=> string.IsNullOrEmpty(FirstName);
        public bool IsLastNameEmpty => string.IsNullOrEmpty(LastName);
        public bool IsUsernameEmpty => string.IsNullOrEmpty(Username);
        public bool IsPasswordEmpty => string.IsNullOrEmpty(Password);

        public ICommand SignUpCommand { get; }
        public ICommand LoginCommand { get; }

        public SignUpViewModel()
        {
            SignUpCommand = new RelayCommand(o => ExecuteSignUp());
            LoginCommand = new RelayCommand(o => ExecuteLogin());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ExecuteSignUp() {
            if (_email != null)
            {
                MainPageViewModel.Instance.NavigateToMainView();
            }
        }
        private void ExecuteLogin()
        {
            MainPageViewModel.Instance.NavigateToLoginView();
        }
    }
}
