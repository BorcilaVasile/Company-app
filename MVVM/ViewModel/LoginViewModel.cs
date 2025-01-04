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
        private string _email;
        private string _password;
        public bool _emailIsFocused;
        public bool _passwordIsFocused;
        public bool _inputIsWrong;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool InputIsWrong
        {
            get => _inputIsWrong;
            set
            {
                if (_inputIsWrong != value)
                {
                    _inputIsWrong = value;
                    OnPropertyChanged(nameof(InputIsWrong));
                }
            }
        }
        public bool EmailIsFocused
        {
            get => _emailIsFocused;
            set   
            {
                if (_emailIsFocused != value)
                {
                    _emailIsFocused = value;
                    OnPropertyChanged(nameof(EmailIsFocused));
                }
            }
        }
        public bool PasswordIsFocused
        {
            get => _passwordIsFocused;
            set
            {
                if (_passwordIsFocused != value)
                {
                    _passwordIsFocused = value;
                    OnPropertyChanged(nameof(PasswordIsFocused));
                }
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
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
            MainPageViewModel.Instance.NavigateToMainView();
            //using (var context = new CompanyDataContext())
            //{
            //    var query = context.Accounts
            //        .FirstOrDefault(account => account.Email == _email && account.Password == _password);

            //    if (query != null)
            //    {
            //        MainPageViewModel.Instance.NavigateToMainView();
            //    }
            //    else
            //        InputIsWrong = true;
            //}
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
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
