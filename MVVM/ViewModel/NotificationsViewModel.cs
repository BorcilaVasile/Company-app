using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Administrare_firma.Core;

namespace Administrare_firma.MVVM.ViewModel
{
    public class NotificationsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
            }
        }

        private int _employeeID; 
        public int EmployeeID
        {
            get => _employeeID;
            set
            {
                _employeeID = value;
                OnPropertyChanged(nameof(EmployeeID));
            }
        }

        public RelayCommand MarkAsSeenCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        private MainViewModel _mainViewModel;

        public NotificationsViewModel(MainViewModel mainViewModel, int employeeID)
        {
            _employeeID = employeeID;
            _mainViewModel = mainViewModel;
            MarkAsSeenCommand = new RelayCommand(o => MarkAsSeen(o));
            DeleteCommand = new RelayCommand(o => Delete(o));
            LoadNotifications();
        }

        public void LoadNotifications()
        {
            using (var context = new ApplicationDbContext())
            {
                _notifications = new ObservableCollection<Notification>(context.Notifications.Where(n => n.ID_receiver== _employeeID).OrderByDescending(n => n.Date));
            }
        }

        public void MarkAsSeen(object parameter)
        {
            if (parameter is Notification notification)
            {
                notification.Seen = true;

                using (var context = new ApplicationDbContext())
                {
                    context.Entry(notification).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                _mainViewModel.UpdateNumberOfNotifications();
            }
        }
        public void Delete(object parameter)
        {
            if (parameter is Notification notification)
            {
                using (var context = new ApplicationDbContext())
                {
                    var notificationToDelete = context.Notifications
                                              .FirstOrDefault(n => n.ID == notification.ID);

                    if (notificationToDelete != null)
                    {
                        context.Notifications.Remove(notificationToDelete);
                        context.SaveChanges();

                        Notifications.Remove(notification);
                    }
                }
            }
            _mainViewModel.UpdateNumberOfNotifications();
        }
    }
}
