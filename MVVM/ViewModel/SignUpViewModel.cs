using Administrare_firma.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Administrare_firma.MVVM.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _fullname;
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;

        public bool _fullnameIsFocused;
        public bool _firstNameIsFocused;
        public bool _lastNameIsFocused;
        public bool _usernameIsFocused;
        public bool _emailIsFocused;
        public bool _passwordIsFocused;
        public bool _confirmPasswordIsFocused;
        public bool _nameInputIsWrong;
        public bool _emailInputIsWrong;
        public bool _passwordInputIsWrong;
        public bool _confirmPasswordInputIsWrong;
        public bool _passwordsMatch=true;


        public event PropertyChangedEventHandler PropertyChanged;

        public bool PasswordsMatch
        {
            get => _passwordsMatch;
            set
            {
                _passwordsMatch = value;
                OnPropertyChanged(nameof(PasswordsMatch));
            }
        }
        public bool NameInputIsWrong
        {
            get => _nameInputIsWrong;
            set
            {
                if (_nameInputIsWrong != value)
                {
                    _nameInputIsWrong = value;
                    OnPropertyChanged(nameof(NameInputIsWrong));
                }
            }
        }
        public bool EmailInputIsWrong
        {
            get => _emailInputIsWrong;
            set
            {
                if (_emailInputIsWrong != value)
                {
                    _emailInputIsWrong = value;
                    OnPropertyChanged(nameof(EmailInputIsWrong));
                }
            }
        }

        public bool PasswordInputIsWrong
        {
            get => _passwordInputIsWrong;
            set
            {
                if (_passwordInputIsWrong != value)
                {
                    _passwordInputIsWrong = value;
                    OnPropertyChanged(nameof(PasswordInputIsWrong));
                }
            }
        }

        public bool ConfirmPasswordInputIsWrong
        {
            get => _confirmPasswordInputIsWrong;
            set
            {
                if (_confirmPasswordInputIsWrong != value)
                {
                    _confirmPasswordInputIsWrong = value;
                    OnPropertyChanged(nameof(ConfirmPasswordInputIsWrong));
                }
            }
        }


        public string Fullname
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(Fullname));
                OnPropertyChanged(nameof(IsFullnameEmpty));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
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
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(IsLastNameEmpty));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                EmailInputIsWrong = !IsValidEmail(Email);
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                UpdatePasswordsMatch();
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                UpdatePasswordsMatch();
            }
        }


        public bool FullnameIsFocused
        {
            get => _firstNameIsFocused;
            set
            {
                _firstNameIsFocused = value;
                OnPropertyChanged(nameof(FullnameIsFocused));
            }
        }
        public bool EmailIsFocused
        {
            get => _emailIsFocused;
            set
            {
                _emailIsFocused = value;
                OnPropertyChanged(nameof(EmailIsFocused));
            }
        }
        public bool PasswordIsFocused
        {
            get => _passwordIsFocused;
            set
            {
                _passwordIsFocused = value;
                OnPropertyChanged(nameof(PasswordIsFocused));
            }
        }
        public bool ConfirmPasswordIsFocused
        {
            get => _confirmPasswordIsFocused;
            set
            {
                _confirmPasswordIsFocused = value;
                OnPropertyChanged(nameof(ConfirmPasswordIsFocused));
            }
        }
        public bool IsFullnameEmpty=> string.IsNullOrEmpty(Fullname);
        public bool IsLastNameEmpty => string.IsNullOrEmpty(LastName);
        public bool IsUsernameEmpty => string.IsNullOrEmpty(Username);
        public bool IsPasswordEmpty => string.IsNullOrEmpty(Password);
        public bool IsEmailEmpty => string.IsNullOrEmpty(Email);
        public bool IsConfirmPasswordEmpty => string.IsNullOrEmpty(ConfirmPassword);

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
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
        private void ExecuteSignUp() {
            if (IsFullnameEmpty && IsEmailEmpty && IsPasswordEmpty && IsConfirmPasswordEmpty)
            {
                NameInputIsWrong = true;
                EmailInputIsWrong = true;
                ConfirmPasswordInputIsWrong = true;
                PasswordInputIsWrong = true;
                return;
            }

            if(IsFullnameEmpty)
            {
                NameInputIsWrong = true;
                return;
            }
            if (IsEmailEmpty)
            {
                EmailInputIsWrong = true;
                return;
            }
            if (IsPasswordEmpty)
            {
                PasswordInputIsWrong = true;
                return;
            }
            if (IsConfirmPasswordEmpty)
            {
                ConfirmPasswordInputIsWrong = true;
                return;
            }
            if(Password != ConfirmPassword)
            {
                PasswordsMatch = false;
                return;
            }


            using (var context = new CompanyDataContext())
            {
                var query = context.Employees
                    .FirstOrDefault(employee => (employee.First_name + " " + employee.Last_name).Equals(Fullname) &&
                employee.Email.Equals(Email));

                if (query != null)
                {
                    var newAccount = new Account
                    {
                        EmployeeID=query.ID,
                        Username = Fullname,
                        Password = Password,
                        Email = Email
                    };

                    context.Accounts.InsertOnSubmit(newAccount);
                    context.SubmitChanges();


                    MessageBox.Show("Account created successfully!");
                    MainPageViewModel.Instance.NavigateToMainView();
                }
                else
                {
                    NameInputIsWrong = true;
                    EmailInputIsWrong= true;
                    MessageBox.Show("You are not an employee of Elysium and you don't have access to our company app");
                }
            }
        }
        private void ExecuteLogin()
        {
            MainPageViewModel.Instance.NavigateToLoginView();
        }
        private void UpdatePasswordsMatch()
        {
            PasswordsMatch = Password == ConfirmPassword;
        }
    }
}
