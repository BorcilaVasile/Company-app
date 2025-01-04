using Administrare_firma.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using Administrare_firma.MVVM.Model;
using System.Collections.ObjectModel;


namespace Administrare_firma.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        private int _activeEmployeesCount;
        private int _inactiveEmployeesCount;
        private int _fullTimeEmployeesCount;
        private int _remoteEmployeesCount;
        private int _partTimeEmployeesCount;
        private PlotModel _activeInactiveModel;
        private PlotModel _employmentTypesModel;
        public RelayCommand ViewRequestDetailsCommand { get; }
        public MainViewModel _mainViewModel;

        private ObservableCollection<Request_informations> _requests;
        public ObservableCollection<Request_informations> Requests
        {
            get => _requests;
            set
            {
                _requests = value;
                OnPropertyChanged(nameof(Requests));
            }
        }
        public int ActiveEmployeesCount
        {
            get => _activeEmployeesCount;
            set
            {
                _activeEmployeesCount = value;
                OnPropertyChanged(nameof(ActiveEmployeesCount));
            }
        }

        public int InactiveEmployeesCount
        {
            get => _inactiveEmployeesCount;
            set
            {
                _inactiveEmployeesCount = value;
                OnPropertyChanged(nameof(InactiveEmployeesCount));
            }
        }

        public int FullTimeEmployeesCount
        {
            get => _fullTimeEmployeesCount;
            set
            {
                _fullTimeEmployeesCount = value;
                OnPropertyChanged(nameof(FullTimeEmployeesCount));
            }
        }

        public int RemoteEmployeesCount
        {
            get => _remoteEmployeesCount;
            set
            {
                _remoteEmployeesCount = value;
                OnPropertyChanged(nameof(RemoteEmployeesCount));
            }
        }

        public int PartTimeEmployeesCount
        {
            get => _partTimeEmployeesCount;
            set
            {
                _partTimeEmployeesCount = value;
                OnPropertyChanged(nameof(_partTimeEmployeesCount));
            }
        }
        public PlotModel ActiveInactiveModel
        {
            get => _activeInactiveModel;
            set
            {
                _activeInactiveModel = value;
                OnPropertyChanged(nameof(ActiveInactiveModel));
            }
        }

        public PlotModel EmploymentTypesModel
        {
            get => _employmentTypesModel;
            set
            {
                _employmentTypesModel = value;
                OnPropertyChanged(nameof(EmploymentTypesModel));
            }
        }
        public HomeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            ViewRequestDetailsCommand= new RelayCommand(o => NavigateToRequestDetailsView(o));
            LoadRequestsFromDatabase();
            LoadStatisticsFromDatabase();

            ActiveInactiveModel = new PlotModel { Title = "Active vs Inactive Employees" };
            EmploymentTypesModel = new PlotModel { Title = "Employment Types" };

            UpdateActiveInactiveChart();
            UpdateEmploymentTypesChart();
        }
        private void UpdateActiveInactiveChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSeries.Slices.Add(new PieSlice("Active", ActiveEmployeesCount) { Fill = OxyColors.Green });
            pieSeries.Slices.Add(new PieSlice("Inactive", InactiveEmployeesCount) { Fill = OxyColors.Red });

            ActiveInactiveModel.Series.Clear();
            ActiveInactiveModel.Series.Add(pieSeries);

            ActiveInactiveModel.InvalidatePlot(true);
        }

        private void UpdateEmploymentTypesChart()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            pieSeries.Slices.Add(new PieSlice("Full-Time", FullTimeEmployeesCount) { Fill = OxyColors.Blue });
            pieSeries.Slices.Add(new PieSlice("Remote", RemoteEmployeesCount) { Fill = OxyColors.Orange });
            pieSeries.Slices.Add(new PieSlice("Part-time", PartTimeEmployeesCount) { Fill = OxyColors.Gray });

            EmploymentTypesModel.Series.Clear();
            EmploymentTypesModel.Series.Add(pieSeries);

            EmploymentTypesModel.InvalidatePlot(true);
        }

        private void LoadStatisticsFromDatabase()
        {
            using (var context = new CompanyDataContext())
            {
                ActiveEmployeesCount = context.Employees.Count(e => e.Status == "active");

                InactiveEmployeesCount = context.Employees.Count(e => e.Status == "inactive");

                FullTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "full-time");

                RemoteEmployeesCount = context.Employees.Count(e => e.Contract_type == "remote");

                PartTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "part-time");
            }
        }
        private void LoadRequestsFromDatabase()
        {
            using (var context = new CompanyDataContext())
            {
                var requests = context.Requests
                    .Select(r => new Request_informations
                    {
                        Request = r,
                        Requester = context.Employees.FirstOrDefault(e => e.ID == r.RequesterID)
                    })
                    .ToList();

                Requests = new ObservableCollection<Request_informations>(requests);
            }
        }

        private void NavigateToRequestDetailsView(object parameter)
        { 
            var requestInformations = parameter as Request_informations;

            if (requestInformations != null)
            {
                _mainViewModel.NavigateToRequestDetailsView(requestInformations);
            }
            else
                Console.WriteLine("Invalid parameter for NavigateToRequestDetailsView");
        }
    }
}
