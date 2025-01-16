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
using System.Timers;


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
        private DateTime? _workStartTime;
        private DateTime? _pauseStartTime;
        private Timer _workSessionTimer;
        public RelayCommand ViewRequestDetailsCommand { get; }
        public RelayCommand CreateViewRequestCommand { get; }
        public RelayCommand CreateRequestCommand { get; }
        public RelayCommand SetTimeCommand { get; }
        public RelayCommand SortRequestsCommand { get; }
        public RelayCommand SetBreakTimeCommand {  get; }
        public MainViewModel _mainViewModel;

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));
                SortRequests(); 
            }
        }

        public ObservableCollection<Request_informations> SortedRequests { get; set; }
        public List<string> SortOptions { get; set; } = new List<string>
                {
                    "All",
                    "Approved",
                    "Pending",
                    "Rejected",
                    "Zile libere",
                    "Concediu",
                    "Marire salariu",
                    "Invoire"
                };



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
        public int employeeID;
        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }

        private bool _isManager;
        public bool IsManager
        {
            get => _isManager;
            set
            {
                if (_isManager != value)
                {
                    _isManager = value;
                    OnPropertyChanged(nameof(IsManager));
                }
            }
        }

        private bool _isEmployee;
        public bool IsEmployee
        {
            get => _isEmployee;
            set
            {
                if (!_isEmployee != value)
                {
                    _isEmployee = value;
                    OnPropertyChanged(nameof(IsEmployee));
                }
            }
        }

        private bool _pause;
        public bool Pause
        {
            get => _pause;
            set
            {
                if (_pause != value)
                {
                    _pause = value;
                    OnPropertyChanged(nameof(Pause));
                }
            }
        }

        private bool _working;
        public bool Working
        {
            get => _working;
            set
            {
                if (_working != value)
                {
                    _working = value;
                    OnPropertyChanged(nameof(Working));
                }
            }
        }

        private ObservableCollection<Clocking> _clockingData;
        public ObservableCollection<Clocking> ClockingData
        {
            get => _clockingData;
            set
            {
                _clockingData = value;
                OnPropertyChanged(nameof(ClockingData));
            }
        }
        private TimeSpan _totalPauseDuration = TimeSpan.Zero;


        public HomeViewModel(MainViewModel mainViewModel, int employeeID, bool IsAdmin, bool IsManager)
        {
            _mainViewModel = mainViewModel;
            this.employeeID = employeeID;
            _isAdmin = IsAdmin;
            _isManager = IsManager;
            _isEmployee= !IsAdmin && !IsManager;
            _working = false;
            _pause = false;

            ViewRequestDetailsCommand = new RelayCommand(o => NavigateToRequestDetailsView(o));
            CreateRequestCommand=new RelayCommand(o => NavigateToCreateRequestView());
            SetTimeCommand = new RelayCommand(o => SetTime());
            SetBreakTimeCommand = new RelayCommand(o => SetBreak());

            LoadRequestsFromDatabase();
            LoadStatisticsFromDatabase();
            LoadClockingData();

            ActiveInactiveModel = new PlotModel { Title = "Active vs Inactive Employees" };
            EmploymentTypesModel = new PlotModel { Title = "Employment Types" };

            UpdateActiveInactiveChart();
            UpdateEmploymentTypesChart();

            _workSessionTimer = new Timer(10 * 1000); 
            _workSessionTimer.Elapsed += AutoCloseWorkSession;
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
                if (IsAdmin)
                {
                    ActiveEmployeesCount = context.Employees.Count(e => e.Status == "active");

                    InactiveEmployeesCount = context.Employees.Count(e => e.Status == "inactive");

                    FullTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "full-time");

                    RemoteEmployeesCount = context.Employees.Count(e => e.Contract_type == "remote");

                    PartTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "part-time");
                }
                else if (IsManager)
                {
                    var managerDepartmentID = context.Employees
                                            .Where(e => e.ID == employeeID)
                                            .Select(e => e.ID_department)
                                            .FirstOrDefault();
                    ActiveEmployeesCount = context.Employees.Count(e => e.Status == "active" && e.ID_department == managerDepartmentID && e.ID != employeeID);
                    InactiveEmployeesCount = context.Employees.Count(e => e.Status == "inactive" && e.ID_department == managerDepartmentID && e.ID != employeeID);
                    FullTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "full-time" && e.ID_department == managerDepartmentID && e.ID != employeeID);
                    RemoteEmployeesCount = context.Employees.Count(e => e.Contract_type == "remote" && e.ID_department == managerDepartmentID && e.ID != employeeID);
                    PartTimeEmployeesCount = context.Employees.Count(e => e.Contract_type == "part-time" && e.ID_department == managerDepartmentID && e.ID != employeeID);
                }
            }
        }
        private void LoadRequestsFromDatabase()
        {
            using (var context = new CompanyDataContext())
            {
                if (IsAdmin)
                {
                    var departmentsWithoutManagers = context.Departments
                                                   .Where(d => !context.Employees.Any(e => e.ID_department == d.ID_department && !context.Posts.Any(p => p.ID_post == e.ID_post && p.Level_of_importance == 3)))
                                                   .Select(d => d.ID_department)
                                                   .ToList();
                    var requests = context.Requests
                                .Select(r => new Request_informations
                                {
                                    Request = r,
                                    Requester = context.Employees.FirstOrDefault(e => e.ID == r.RequesterID),
                                    Important = context.Employees.Any(e => e.ID == r.RequesterID &&
                                                                           (context.Posts.Any(p => p.ID_post == e.ID_post && p.Level_of_importance == 3) ||
                                                                           departmentsWithoutManagers.Contains((int)e.ID_department)))
                                })
                                .ToList();
                    Requests = new ObservableCollection<Request_informations>(requests);
                }
                else if (IsManager)
                {
                    var managerDepartmentID = context.Employees
                                            .Where(e => e.ID == employeeID)
                                            .Select(e => e.ID_department)
                                            .FirstOrDefault();
                    if (managerDepartmentID == null)
                    {
                        throw new InvalidOperationException("Manager not found or department not assigned.");
                    }

                    var requests = context.Requests
                                    .Where(r => context.Employees.Any(e => e.ID == r.RequesterID && e.ID_department == managerDepartmentID && e.ID != employeeID))
                                    .Select(r => new Request_informations
                                    {
                                        Request = r,
                                        Requester = context.Employees.FirstOrDefault(e => e.ID == r.RequesterID),
                                        Important = false 
                                    })
                                    .ToList();

                    Requests = new ObservableCollection<Request_informations>(requests);
                }


            }
        }

        private void SortRequests()
        {
            if (Requests == null) return;

            switch (SelectedSortOption)
            {
                case "Approved":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.Status == "Approved"));
                    break;

                case "Pending":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.Status == "Pending"));
                    break;

                case "Rejected":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.Status == "Rejected"));
                    break;

                case "Zile libere":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.RequestType == "Zile libere"));
                    break;

                case "Invoire":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.RequestType == "Invoire"));
                    break;

                case "Concediu":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.RequestType == "Concediu"));
                    break;

                case "Marire salariu":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests.Where(r => r.Request.RequestType == "Marire salariu"));
                    break;

                default:
                    SortedRequests = new ObservableCollection<Request_informations>(Requests);
                    break;
            }

            OnPropertyChanged(nameof(SortedRequests));
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

        private void NavigateToCreateRequestView()
        {
            _mainViewModel.NavigateToCreateRequestView(employeeID);
        }
        private void SetTime()
        {
            using (var context = new CompanyDataContext())
            {
                if (Working)
                {
                    var currentRecord = context.Clockings
                        .FirstOrDefault(c => c.ID_employee == employeeID && c.End_hour == null);

                    if (currentRecord != null)
                    {
                        currentRecord.End_hour = DateTime.Now.TimeOfDay;
                        currentRecord.BreakHours = (decimal)_totalPauseDuration.TotalHours;
                        context.SubmitChanges();
                        Console.WriteLine($"Clocking closed with BreakHours: {currentRecord.BreakHours}");
                        _totalPauseDuration = TimeSpan.Zero;
                }

                    _workSessionTimer.Stop(); // Stop auto-close timer
                    Working = false;
                }
                else
                {
                    // Start work session
                    var newClocking = new Clocking
                    {
                        ID_employee = employeeID,
                        Date_of_clocking = DateTime.Now.Date,
                        Start_hour = DateTime.Now.TimeOfDay
                    };

                    context.Clockings.InsertOnSubmit(newClocking);
                    context.SubmitChanges();

                    _workStartTime = DateTime.Now;
                    _workSessionTimer.Start(); // Start auto-close timer
                    Working = true;
                }
            }
        }
        private void SetBreak()
        {
            if (Pause)
            {
                if (_pauseStartTime.HasValue)
                {
                    // Calculăm durata pauzei
                    var pauseDuration = DateTime.Now - _pauseStartTime.Value.Add(TimeSpan.FromSeconds(-3000));
                    _totalPauseDuration += pauseDuration;

                    Console.WriteLine($"Updated pause duration: {_totalPauseDuration.TotalMinutes} minutes");

                    // Resetăm starea pauzei
                    _pauseStartTime = null;
                    Pause = false;
                }
            }
            else
            {
                // Începem o nouă pauză
                _pauseStartTime = DateTime.Now;
                Pause = true;
            }
        }

        private void AutoCloseWorkSession(object sender, ElapsedEventArgs e)
        {
            using (var context = new CompanyDataContext())
            {
                var currentRecord = context.Clockings
                    .FirstOrDefault(c => c.ID_employee == employeeID && c.End_hour == null);

                if (currentRecord != null)
                {
                    currentRecord.End_hour = DateTime.Now.TimeOfDay;
                    currentRecord.BreakHours = (decimal)_totalPauseDuration.TotalHours;
                    context.SubmitChanges();
                }

                Working = false;
                _workSessionTimer.Stop();
            }
            LoadClockingData();
        }
        public void LoadClockingData()
        {
            using (var context = new CompanyDataContext())
            {
                // Fetch clocking records for the current employee, excluding the admin
                var clockingRecords = context.Clockings
                    .Where(c => c.ID_employee == employeeID && employeeID != 0) // Exclude admin
                    .OrderByDescending(c => c.Date_of_clocking) // Sort by date
                    .ToList();

                // Populate the ClockingData collection
                ClockingData = new ObservableCollection<Clocking>(clockingRecords);
            }
        }

    }
}
