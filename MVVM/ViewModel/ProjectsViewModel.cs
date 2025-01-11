using Administrare_firma.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrare_firma.MVVM.ViewModel
{
    public class ProjectsViewModel : ObservableObject
    {
        private ObservableCollection<Project> _projects;
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set
            {
                _projects = value;
                OnPropertyChanged(nameof(Project));
            }
        }
        public ProjectsViewModel() {
            LoadProjects();

        }
        private void LoadProjects()
        {
            using (var context = new CompanyDataContext()) 
            {
                using (var db = new CompanyDataContext()) // Conexiune către baza de date
                {
                    // Obține toate proiectele direct
                    var projectsFromDb = db.Projects.ToList();

                    // Populează colecția ObservableCollection
                    Projects = new ObservableCollection<Project>(projectsFromDb);
                }

            }
        }
    }
}
