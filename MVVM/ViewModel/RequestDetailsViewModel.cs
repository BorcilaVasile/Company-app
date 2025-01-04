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
        private MainViewModel _mainViewModel; 

        public Request_informations CurrentRequest
        {
            get => _currentRequest;
            set
            {
                _currentRequest = value;
                OnPropertyChanged(nameof(CurrentRequest));
            }
        } 
        public RequestDetailsViewModel(Request_informations request_Informations, MainViewModel mainViewModel) {
            _currentRequest = request_Informations;
            _mainViewModel = mainViewModel;
        }
    }
}
