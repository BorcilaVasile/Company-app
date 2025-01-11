using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Administrare_firma.Core;

namespace Administrare_firma.MVVM.ViewModel
{
    public class CreateRequestViewModel : ObservableObject
    {
        private int _requestID;
        private string _requestType;
        private int _requesterID;
        private DateTime _requestDate;
        private int? _durationDays;
        private string _reason;
        private string _explanation;
        private int? _projectID;

        public int RequestID
        {
            get => _requestID;
            set
            {
                _requestID = value;
                OnPropertyChanged(nameof(RequestID));
            }
        }

        public string RequestType
        {
            get => _requestType;
            set
            {
                _requestType = value;
                OnPropertyChanged(nameof(RequestType));
            }
        }

        public int RequesterID
        {
            get => _requesterID;
            set
            {
                _requesterID = value;
                OnPropertyChanged(nameof(RequesterID));
            }
        }

        public DateTime RequestDate
        {
            get => _requestDate;
            set
            {
                _requestDate = value;
                OnPropertyChanged(nameof(RequestDate));
            }
        }

        public int? DurationDays
        {
            get => _durationDays;
            set
            {
                _durationDays = value;
                OnPropertyChanged(nameof(DurationDays));
            }
        }

        public string Reason
        {
            get => _reason;
            set
            {
                _reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }

        public string Explanation
        {
            get => _explanation;
            set
            {
                _explanation = value;
                OnPropertyChanged(nameof(Explanation));
            }
        }

        public int? ProjectID
        {
            get => _projectID;
            set
            {
                _projectID = value;
                OnPropertyChanged(nameof(ProjectID));
            }
        }

        public RelayCommand SubmitRequestCommand { get; }
        public RelayCommand CancelCommand { get; }

        private int _employeeID;
        public int EmployeeID
        {
            get => _employeeID;
            set
            {
                if (_employeeID != value)
                {
                    _employeeID = value;
                    OnPropertyChanged(nameof(EmployeeID));
                }
            }
        }
        public ObservableCollection<string> RequestTypeOptions { get; set; }
        private MainViewModel _mainViewModel;
        private bool IsManager {  get; set; }
        public CreateRequestViewModel(int employeeID, MainViewModel mainViewModel, bool IsManager)
        {
            _mainViewModel= mainViewModel;
            EmployeeID = employeeID;
            this.IsManager = IsManager;
            SubmitRequestCommand = new RelayCommand(SubmitRequest);
            CancelCommand = new RelayCommand(Cancel);

            RequestDate = DateTime.Now;
            RequestTypeOptions = new ObservableCollection<string>
                                {
                                    "Invoire",
                                    "Concediu",
                                    "Zile libere",
                                    "Marire salariu"
                                };
            _mainViewModel = mainViewModel;
        }

        private void SubmitRequest(object parameter)
        {
            Console.WriteLine($"RequestType: {RequestType}");
            Console.WriteLine($"Reason: {Reason}");
            Console.WriteLine($"RequesterID: {RequesterID}");

            if (string.IsNullOrWhiteSpace(RequestType) || string.IsNullOrWhiteSpace(Reason))
            {
                System.Windows.MessageBox.Show("Please fill all mandatory fields.", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }


            using (var context = new CompanyDataContext())
            {
                var newRequest = new Request
                {
                    RequestType = RequestType,
                    RequesterID = EmployeeID,
                    RequestDate = RequestDate,
                    DurationDays = DurationDays,
                    Reason = Reason,
                    Explanation = Explanation,
                    Status = "Pending", 
                    ReviewedBy = null,
                    ReviewDate = null,
                    Remarks = null,
                    ProjectID = null
                };

                context.Requests.InsertOnSubmit(newRequest);

                context.SubmitChanges();
                System.Windows.MessageBox.Show("Request submitted successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                ResetForm();
            }
        }

        private void Cancel(object parameter)
        {
            ResetForm();
            System.Windows.MessageBox.Show("Request creation canceled.", "Canceled", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            _mainViewModel.NavigateToHomeView(EmployeeID, false,IsManager);
        }

        private void ResetForm()
        {
            RequestID = 0;
            RequestType = string.Empty;
            RequesterID = 0;
            RequestDate = DateTime.Now;
            DurationDays = null;
            Reason = string.Empty;
            Explanation = string.Empty;
            ProjectID = null;
        }

    }
}
