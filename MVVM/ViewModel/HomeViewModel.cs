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
        public RelayCommand SetBreakTimeCommand { get; }
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

        private decimal _totalAmountDue;
        public decimal TotalAmountDue
        {
            get => _totalAmountDue;
            set
            {
                _totalAmountDue = value;
                OnPropertyChanged(nameof(TotalAmountDue));
            }
        }

        private int _totalWorkedHours;
        public int TotalWorkedHours
        {
            get => _totalWorkedHours;
            set{
                _totalWorkedHours=value;
                OnPropertyChanged(nameof(TotalWorkedHours));
                } 
        }

        private int _necessaryHours;
        public int NecessaryHours
        {
            get => _necessaryHours;
            set
            {
                _necessaryHours = value;
                OnPropertyChanged(nameof(NecessaryHours));
            }
        }

        public HomeViewModel(MainViewModel mainViewModel, int employeeID, bool IsAdmin, bool IsManager)
        {
            _mainViewModel = mainViewModel;
            this.employeeID = employeeID;
            _isAdmin = IsAdmin;
            _isManager = IsManager;
            _isEmployee= !IsAdmin && !IsManager;
            _working = false;
            _pause = false;
            _selectedSortOption = "All";

            ViewRequestDetailsCommand = new RelayCommand(o => NavigateToRequestDetailsView(o));
            CreateRequestCommand=new RelayCommand(o => NavigateToCreateRequestView());
            SetTimeCommand = new RelayCommand(o => SetTime());
            SetBreakTimeCommand = new RelayCommand(o => SetBreak());

            Requests = new ObservableCollection<Request_informations>();
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
            using (var context = new ApplicationDbContext())
            {
                List<Employee> relevantEmployees;

                if (IsAdmin)
                {
                    relevantEmployees = context.Employee.ToList();
                }
                else if (IsManager)
                {
                    var managerDepartmentID = context.Employee
                                                     .Where(e => e.ID == employeeID)
                                                     .Select(e => e.ID_department)
                                                     .FirstOrDefault();

                    if (managerDepartmentID == 0)
                    {
                        throw new InvalidOperationException("Manager not found or department not assigned.");
                    }

                    relevantEmployees = context.Employee
                                               .Where(e => e.ID_department == managerDepartmentID && e.ID != employeeID)
                                               .ToList();
                }
                else
                {
                    return; 
                }

                ActiveEmployeesCount = relevantEmployees.Count(e => e.Status == "active");
                InactiveEmployeesCount = relevantEmployees.Count(e => e.Status == "inactive");
                FullTimeEmployeesCount = relevantEmployees.Count(e => e.Contract_type == "full-time");
                RemoteEmployeesCount = relevantEmployees.Count(e => e.Contract_type == "remote");
                PartTimeEmployeesCount = relevantEmployees.Count(e => e.Contract_type == "part-time");
            }
        }

        private void LoadRequestsFromDatabase()
        {
            using (var context = new ApplicationDbContext())
            {
                var employees = context.Employee.ToList();
                var posts = context.Posts.ToList();

                if (IsAdmin)
                {   var postsWithImportance3 = posts
                                               .Where(p => p.Level_of_importance == 3)
                                               .Select(p => p.ID_post)
                                               .ToList();

                    var departmentsWithoutManagers = context.Departments
                                                .Where(d => !context.Employee.Any(e => e.ID_department == d.ID_department && postsWithImportance3.Contains((int)e.ID_post)))
                                                .Select(d => d.ID_department)
                                                .ToList();


                    var requests = context.Requests
                                          .ToList() // Preluăm toate cererile înainte de a aplica transformarea
                                          .Select(r => new Request_informations
                                          {
                                              Request = r,
                                              Requester = employees.FirstOrDefault(e => e.ID == r.RequesterID),
                                              Important = employees.Any(e => e.ID == r.RequesterID &&
                                                                              (postsWithImportance3.Contains((int)e.ID_post) ||
                                                                               departmentsWithoutManagers.Contains((int)e.ID_department)))
                                          })
                                          .ToList();

                    Requests = new ObservableCollection<Request_informations>(requests);
                }
                else if (IsManager)
                {
                    var managerDepartmentID = employees
                                              .Where(e => e.ID == employeeID)
                                              .Select(e => e.ID_department)
                                              .FirstOrDefault();

                    if (managerDepartmentID == 0)
                    {
                        throw new InvalidOperationException("Manager not found or department not assigned.");
                    }

                    var requestsFromDb = context.Requests
                                       .Where(r => r.RequesterID != employeeID)
                                       .ToList();

                    //var requests = context.Requests
                    //                      .Where(r => employees.Any(e => e.ID == r.RequesterID && e.ID_department == managerDepartmentID && e.ID != employeeID))
                    //                      .ToList()
                    //                      .Select(r => new Request_informations
                    //                      {
                    //                          Request = r,
                    //                          Requester = employees.FirstOrDefault(e => e.ID == r.RequesterID),
                    //                          Important = false
                    //                      })
                    //                      .ToList();

                    var requests = requestsFromDb.Select(r => new Request_informations
                    {
                        Request = r,
                        Requester = employees.FirstOrDefault(e => e.ID == r.RequesterID), // Folosește lista de angajați deja încărcată
                        Important = false
                    }).ToList();

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

                case "All":
                    SortedRequests = new ObservableCollection<Request_informations>(Requests);
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
            using (var context = new ApplicationDbContext())
            {
                if (Working)
                {
                    var currentRecord = context.Clockings
                        .FirstOrDefault(c => c.ID_employee == this.employeeID && c.End_hour == null);

                    if (currentRecord != null)
                    {
                        currentRecord.End_hour = DateTime.Now.TimeOfDay;
                        currentRecord.BreakHours = (decimal)_totalPauseDuration.TotalHours;

                        context.SaveChanges();
                        Console.WriteLine($"Clocking closed with BreakHours: {currentRecord.BreakHours}");

                        TimeSpan duration = (TimeSpan)currentRecord.End_hour - (TimeSpan)currentRecord.Start_hour;
                        double totalHoursWorked = duration.Hours + duration.Minutes / 60.0 + 10;
                        var employee = context.Employee.FirstOrDefault(e => e.ID == this.employeeID);

                        if (employee != null)
                        {
                            employee.HoursWorkedThisMonth += (int)totalHoursWorked;
                            context.SaveChanges();
                        }

                        _totalPauseDuration = TimeSpan.Zero;
                    }

                    _workSessionTimer.Stop();
                    Working = false;
                }
                else
                {
                    var newClocking = new Clocking
                    {
                        ID_employee = this.employeeID,
                        Date_of_clocking = DateTime.Now.Date,
                        Start_hour = DateTime.Now.TimeOfDay
                    };

                    context.Clockings.Add(newClocking);
                    context.SaveChanges();

                    _workStartTime = DateTime.Now;

                    _workSessionTimer.Start();
                    Working = true;
                }
            }
            LoadClockingData();
        }
        private void SetBreak()
        {
            if (Pause)
            {
                if (_pauseStartTime.HasValue)
                {
                    var pauseDuration = DateTime.Now - _pauseStartTime.Value.Add(TimeSpan.FromSeconds(-300));
                    _totalPauseDuration += pauseDuration;

                    Console.WriteLine($"Updated pause duration: {_totalPauseDuration.TotalMinutes} minutes");

                    _pauseStartTime = null;
                    Pause = false;
                }
            }
            else
            {
                _pauseStartTime = DateTime.Now;
                Pause = true;
            }
        }

        private void AutoCloseWorkSession(object sender, ElapsedEventArgs e)
        {
            using (var context = new ApplicationDbContext())
            {
                var currentRecord = context.Clockings
                    .FirstOrDefault(c => c.ID_employee == this.employeeID && c.End_hour == null);

                if (currentRecord != null)
                {
                    currentRecord.End_hour = DateTime.Now.TimeOfDay;
                    currentRecord.BreakHours = (decimal)_totalPauseDuration.TotalHours;
                    context.SaveChanges();
                    Console.WriteLine("Auto-closed work session.");

                    TimeSpan duration = (TimeSpan)currentRecord.End_hour - (TimeSpan)currentRecord.Start_hour;
                    double totalHoursWorked = duration.Hours + duration.Minutes / 60.0 - (double)currentRecord.BreakHours+10;
                    var employee = context.Employee.FirstOrDefault(emp => emp.ID == this.employeeID);

                    if (employee != null)
                    {
                        employee.HoursWorkedThisMonth += (int)totalHoursWorked;
                        context.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine("No active clocking session found.");
                }

                Working = false;
                _workSessionTimer.Stop();
            }
            LoadClockingData();
        }
        public void LoadClockingData()
        {
            using (var context = new ApplicationDbContext())
            {
                var clockingRecords = context.Clockings
                    .Where(c => c.ID_employee == employeeID && employeeID != 0) // Exclude admin
                    .OrderByDescending(c => c.Date_of_clocking) // Sort by date
                    .ThenByDescending(c => c.Start_hour) // Then sort by start hour descending
                    .ToList();

                ClockingData = new ObservableCollection<Clocking>(clockingRecords);

                int totalHoursWorked = context.Employee
                               .Where(e => e.ID == employeeID)
                               .Select(e => e.HoursWorkedThisMonth)
                               .FirstOrDefault();

                int base_salary= context.Employee
                               .Where(e => e.ID == employeeID)
                               .Select(e => (int)e.Base_salary)
                               .FirstOrDefault();


                int free_days =(int)context.Requests
                                .Where(r => r.Status == "Approved" && r.RequesterID == this.employeeID &&
                                           (r.RequestType == "Concediu" || r.RequestType == "Zile Libere"))
                                .Select(r => r.DurationDays)
                                .DefaultIfEmpty(0) 
                                .Sum();
                int? free_hours= (int)context.Requests
                                .Where(r => r.Status == "Approved" && r.RequesterID == this.employeeID && r.RequestType == "Zile Libere" )
                                .Select(r => r.DurationDays ?? 0)
                                .DefaultIfEmpty(0)
                                .Sum();

                Console.WriteLine($"Free Days: {free_days}, Free Hours: {free_hours}");

                this.NecessaryHours = 150-(int)free_hours-8*(int)free_days;
                this.TotalWorkedHours = totalHoursWorked;
                this.TotalAmountDue= totalHoursWorked*base_salary;
            }
        }

    }
}
