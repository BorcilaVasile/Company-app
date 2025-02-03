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
using System.Windows;

namespace Administrare_firma.MVVM.ViewModel
{
    public class RequestDetailsViewModel: ObservableObject
    {
        private Request_informations _currentRequest {  get; set; }
        private EmployeeService service { get; set; }
        private MainViewModel _mainViewModel;
        public RelayCommand AcceptCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand RejectCommand { get; }

        public Request_informations CurrentRequest
        {
            get => _currentRequest;
            set
            {
                _currentRequest = value;
                OnPropertyChanged(nameof(CurrentRequest));
            }
        }
        private int _userID;
        private bool _isAdmin;
        private bool _isManager;
        public RequestDetailsViewModel(Request_informations request_Informations, MainViewModel mainViewModel,EmployeeService service, int userID, bool IsAdmin, bool IsManager) {
            this.service= service;
            this._userID = userID;
            this._isAdmin = IsAdmin;
            this._isManager = IsManager;
            _currentRequest = request_Informations;
            _mainViewModel = mainViewModel;
            AcceptCommand=new RelayCommand(o => AcceptRequest(o));
            CancelCommand=new RelayCommand(o =>  CancelRequest());
            RejectCommand=new RelayCommand(o => RejectRequest(o));
        }

        private void AcceptRequest(object parameter)
        {
            var requestInfo = parameter as Request_informations;
            if (requestInfo != null)
            {
                service.AcceptRequest(requestInfo.Request);
                requestInfo.Request.Status = "Approved";

                using (var context = new ApplicationDbContext())
                {
                    var sender = context.Employee.FirstOrDefault(e => e.ID == _userID);
                    var receiver = context.Employee.FirstOrDefault(e => e.ID == requestInfo.Request.RequesterID);

                    if (sender != null && receiver != null)
                    {
                        var notification = new Notification
                        {
                            ID_receiver = receiver.ID,
                            Sender_Name = $"{sender.First_name} {sender.Last_name}",
                            Sender_Position = context.Posts.FirstOrDefault(p => p.ID_post == sender.ID_post)?.Nume,
                            Notification_Details = $"Your request of type '{requestInfo.Request.RequestType}' has been approved by {sender.First_name} {sender.Last_name}.",
                            Seen = false,
                            Date = DateTime.Now
                        };

                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                }
            }
            _mainViewModel.NavigateToHomeView(_userID,_isAdmin,_isManager);
        }
        private void RejectRequest(object parameter)
        {
            var requestInfo = parameter as Request_informations;
            if (requestInfo != null)
            {
                service.RejectRequest(requestInfo.Request);
                requestInfo.Request.Status = "Rejected";

                using (var context = new ApplicationDbContext())
                {
                    var sender = context.Employee.FirstOrDefault(e => e.ID == _userID);
                    var receiver = context.Employee.FirstOrDefault(e => e.ID == requestInfo.Request.RequesterID);

                    if (sender != null && receiver != null)
                    {
                        var notification = new Notification
                        {
                            ID_receiver = receiver.ID,
                            Sender_Name = $"{sender.First_name} {sender.Last_name}",
                            Sender_Position = context.Posts.FirstOrDefault(p => p.ID_post == sender.ID_post)?.Nume,
                            Notification_Details = $"Your request of type '{requestInfo.Request.RequestType}' has been rejected by {sender.First_name} {sender.Last_name}.",
                            Seen = false,
                            Date = DateTime.Now
                        };

                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                }
            }
            _mainViewModel.NavigateToHomeView(_userID, _isAdmin, _isManager);
        }
        private void CancelRequest()
        {
            _mainViewModel.NavigateToHomeView(_userID, _isAdmin, _isManager);
        }
    }
}
