using System;
using System.Collections.ObjectModel;
using System.Linq;
using Administrare_firma.Core;
using Administrare_firma.MVVM.Model;

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

        private int _currentEmployee;
        public int CurrentEmployee
        {
            get => _currentEmployee;
            set
            {
                if (_currentEmployee != value)
                {
                    _currentEmployee = value;
                    OnPropertyChanged(nameof(CurrentEmployee));
                }
            }
        }
        public ObservableCollection<string> RequestTypeOptions { get; set; }
        private MainViewModel _mainViewModel;
        private bool IsManager {  get; set; }
        public CreateRequestViewModel(MainViewModel mainViewModel,int employeeID, bool IsManager)
        {
            _mainViewModel= mainViewModel;
            this.IsManager = IsManager;
            _currentEmployee= employeeID;
            SubmitRequestCommand = new RelayCommand(SubmitRequest);
            CancelCommand = new RelayCommand(Cancel);

            RequestDate = DateTime.Now;
            RequestTypeOptions = new ObservableCollection<string>
                                {
                                    "Invoire",
                                    "Concediu",
                                    "Zile libere"//,
                                    //"Marire salariu"
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


            using (var context = new ApplicationDbContext())
            {
                var newRequest = new Requests
                {
                    RequestType = RequestType,
                    RequesterID = _currentEmployee,
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

                context.Requests.Add(newRequest);

                //int IdReceiver;
                //if (IsManager)
                //    IdReceiver = 88;//id-ul admin-ului
                //else
                //{
                    int id_departament = (int)context.Employee
                            .Where(e => e.ID == _currentEmployee)
                            .Select(e => e.ID_department)
                            .FirstOrDefault();
                int? IdReceiver = context.Employee
                                 .Where(e => e.ID_department == id_departament && e.ID != _currentEmployee)
                                 .Join(context.Posts,
                                     employee => employee.ID_post,
                                     post_local => post_local.ID_post,
                                     (employee, post_local) => new { employee, post_local })
                                 .Where(ep => ep.post_local.Level_of_importance == 3)
                                 .Select(ep => (int?)ep.employee.ID)
                                 .FirstOrDefault();

                if (IdReceiver == null)
                    IdReceiver = 88;


                // }
                int id_post = (int)context.Employee
                            .Where(e => e.ID == _currentEmployee) 
                            .Select(e => e.ID_post) 
                            .FirstOrDefault();
                var post = context.Posts
                          .FirstOrDefault(p => p.ID_post == id_post);

                string senderFirstName = context.Employee
                            .Where(e => e.ID == _currentEmployee)
                            .Select(e => e.First_name)
                            .FirstOrDefault();

                string senderLastName = context.Employee
                                        .Where(e => e.ID == _currentEmployee)
                                        .Select(e => e.Last_name)
                                        .FirstOrDefault();


                var notification = new Notification
                {
                    ID_receiver = (int)IdReceiver,
                    Sender_Name = senderFirstName + " " + senderLastName,
                    Sender_Position = post.Nume,
                    Notification_Details = $"A new request of type '{RequestType}' has been created by {senderFirstName} {senderLastName}.",
                    Seen = false,
                    Date = DateTime.Now
                };
                context.Notifications.Add(notification);

                context.SaveChanges();
                System.Windows.MessageBox.Show("Request submitted successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                ResetForm();
            }
            _mainViewModel.NavigateToHomeView(_currentEmployee, false, IsManager);
        }

        private void Cancel(object parameter)
        {
            ResetForm();
            System.Windows.MessageBox.Show("Request creation canceled.", "Canceled", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            _mainViewModel.NavigateToHomeView(_currentEmployee, false,IsManager);
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
