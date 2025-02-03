using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Data.Entity;

namespace Administrare_firma.Core
{
    // Contextul bazei de date
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=Administrare_firma.Properties.Settings.CompanyConnectionString")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            this.Database.Log = Console.WriteLine;

        }

        // Definește tabelele (DbSet)
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Clocking> Clockings { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}


namespace Administrare_firma
{
    // Modele
    public enum ContractType
    {
        PartTime,
        FullTime,
        Remote
    }



    [Table("Employee")]
    public class Employee
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Last_name")]
        [MaxLength(50)]
        public string Last_name { get; set; }

        [Column("First_name")]
        [MaxLength(50)]
        public string First_name { get; set; }

        [Column("CNP")]
        [MaxLength(13)]
        public string CNP { get; set; }

        [Column("Date_of_birth")]
        public DateTime? Date_of_birth { get; set; }

        [Column("Home_address")]
        [MaxLength(150)]
        public string Home_address { get; set; }

        [Column("Phone_number")]
        [MaxLength(13)]
        public string Phone_number { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Column("ID_post")]
        public int? ID_post { get; set; }

        [Column("ID_department")]
        public int? ID_department { get; set; }

        [Column("Base_salary")]
        public int? Base_salary { get; set; }

        [Column("Status")]
        [MaxLength(10)]
        public string Status { get; set; }

        [Column("Contract_period")]
        public int? Contract_period { get; set; }

        [Column("Contract_type")]
        [MaxLength(10)]
        public string Contract_type { get; set; }

        [NotMapped]
        public ContractType ContractTypeEnum
        {
            get => (ContractType)Enum.Parse(typeof(ContractType), Contract_type ?? "FullTime", true);
            set => Contract_type = value.ToString();
        }

        [Column("HoursWorkedThisMonth")]
        public int HoursWorkedThisMonth { get; set; }
        // Relații virtuale
        //[ForeignKey("ID_department")]
        //public virtual Department Department { get; set; }

        //[ForeignKey("ID_post")]
        //public virtual Post Post { get; set; }
    }

    [Table("Requests")]
    public class Requests
    {
        [Key]
        [Column("RequestID")]
        public int RequestID { get; set; }

        [Column("RequestType")]
        public string RequestType { get; set; }

        [Column("RequestDate")]
        public DateTime RequestDate { get; set; }

        [Column("DurationDays")]
        public int? DurationDays { get; set; }

        [Column("Reason")]
        public string Reason { get; set; }

        [Column("Explanation")]
        public string Explanation { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("ReviewedBy")]
        public int? ReviewedBy { get; set; }

        [Column("ReviewDate")]
        public DateTime? ReviewDate { get; set; }

        [Column("Remarks")]
        public string Remarks { get; set; }

        [Column("ProjectID")]
        public int? ProjectID { get; set; }

        [Column("RequesterID")]
        public int RequesterID { get; set; }

       // public virtual Employee Requester { get; set; }
    }

    [Table("Department")]
    public class Department
    {
        [Key]
        [Column("ID_department")]
        public int ID_department { get; set; }

        [Column("Nume")]
        public string Nume { get; set; }

        [Column("Descriere")]
        public string Descriere { get; set; }

        // Relația cu angajații (opțională, dacă vrei să navighezi spre Employees)
       // public virtual ICollection<Employee> Employees { get; set; }
    }

    [Table("Post")]
    public class Post
    {
        [Key]
        [Column("ID_post")]
        public int ID_post { get; set; }

        [Column("Nume")]
        public string Nume { get; set; }

        [Column("Descriere")]
        public string Descriere { get; set; }

        [Column("Level_of_importance")]
        public int? Level_of_importance { get; set; }
       // public virtual ICollection<Employee> Employees { get; set; }
    }

    [Table("Accounts")]
        public class Account
        {
            [Key]
            [Column("AccountID")]
            public int AccountID { get; set; }

            [Column("EmployeeID")]
            public int EmployeeID { get; set; }

            [Column("Username")]
            [Required]
            [MaxLength(50)]
            public string Username { get; set; }

            [Column("Password")]
            [Required]
            [MaxLength(255)]
            public string Password { get; set; }

            [Column("Email")]
            [MaxLength(255)]
            public string Email { get; set; }

            //[ForeignKey("EmployeeID")]

            //public virtual Employee Employee { get; set; }
        }

    [Table("Absence")]
    public class Absence
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ID_employee")]
        public int ID_employee { get; set; }

        [Column("Date_of_absence")]
        public DateTime? Date_of_absence { get; set; }

        [Column("Start_hour")]
        public TimeSpan? Start_hour { get; set; }

        [Column("End_hour")]
        public TimeSpan? End_hour { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        // Relația cu entitatea Employee
        //[ForeignKey("ID_employee")]
        //public virtual Employee Employee { get; set; }
    }

    [Table("Bonus")]
    public class Bonus
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ID_employee")]
        public int ID_employee { get; set; }

        [Column("Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Column("Description")]
        [MaxLength(300)]
        public string Description { get; set; }

        [Column("Value")]
        public int? Value { get; set; }

        [Column("Necessary_conditions")]
        [MaxLength(300)]
        public string NecessaryConditions { get; set; }

        // Relația cu entitatea Employee
        //[ForeignKey("ID_employee")]
        //public virtual Employee Employee { get; set; }
    }

    [Table("Projects")]
    public class Projects
    {
        [Key]
        [Column("ProjectID")]
        public int ProjectID { get; set; }

        [Column("ContractorCompany")]
        [MaxLength(255)]
        public string ContractorCompany { get; set; }

        [Column("Duration")]
        public int Duration { get; set; }

        [Column("Expenses")]
        public decimal Expenses { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }
    }

    [Table("Role")]
    public class Role
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Level_of_importance")]
        public int? Level_of_importance { get; set; }

        [Column("Permissions")]
        [MaxLength(300)]
        public string Permissions { get; set; }

        [Column("Description")]
        [MaxLength(300)]
        public string Description { get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }
    }

    [Table("Training")]
    public class Training
    {
        [Key]
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public DateTime Date { get; set; }

       // public virtual ICollection<Employee> Participants { get; set; }
    }

    [Table("Clocking")]
    public class Clocking
    {
        [Key]
        [Column("ID_clocking")] 
        public int ID_clocking { get; set; }

        [Column("Date_of_clocking")]
        public DateTime Date_of_clocking { get; set; }

        [Column("Start_hour")]
        public TimeSpan Start_hour { get; set; }

        [Column("End_hour")]
        public TimeSpan? End_hour { get; set; }

        [Column("BreakHours")]
        public decimal? BreakHours { get; set; }

        [Column("ID_employee")]
        public int ID_employee { get; set; }

        //public virtual Employee Employee { get; set; }
    }
    [Table("Evaluation")]
    public class Evaluation
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ID_employee")]
        public int ID_employee { get; set; }

        [Column("Date_of_evaluation")]
        public DateTime Date_of_evaluation { get; set; }

        [Column("Scor")]
        public int Scor { get; set; }

        [Column("Feedback")]
        [MaxLength(500)]
        public string Feedback { get; set; }

        [Column("Future_objectives")]
        [MaxLength(500)]
        public string Future_objectives { get; set; }

        [Column("ID_manager")]
        public int ID_manager { get; set; }
    }

    [Table("Notifications")]
    public class Notification : INotifyPropertyChanged
    {
        private bool _seen;

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ID_receiver")]
        public int ID_receiver { get; set; }

        [Column("Sender_Name")]
        [MaxLength(100)]
        public string Sender_Name { get; set; }

        [Column("Sender_Position")]
        [MaxLength(100)]
        public string Sender_Position { get; set; }

        [Column("Notification_Details")]
        [MaxLength(500)]
        public string Notification_Details { get; set; }

        [Column("Seen")]
        public bool Seen
        {
            get => _seen;
            set
            {
                if (_seen != value)
                {
                    _seen = value;
                    OnPropertyChanged(nameof(Seen));
                }
            }
        }

        [Column("Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Relația cu angajatul care primește notificarea
        //[ForeignKey("ID_receiver")]
        //public virtual Employee Receiver { get; set; }
    }
}

